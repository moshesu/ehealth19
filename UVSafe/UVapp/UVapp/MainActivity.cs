using Android.App;
using Android.Content;

using Android.Widget;
using Android.OS;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Timers;

using Microsoft.Band;
using Microsoft.Band.Sensors;

using Android.Support.V4.App;
//using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;

using Android.Gms.Common;
using Android.Gms.Location;
using Android.Support.Design.Widget;
using Android.Views;
using Android;
using Android.Support.V4.Content;
using Android.Content.PM;
using Android.Support.V7.App;
using System.Collections.Generic;

[assembly: UsesPermission(Android.Manifest.Permission.Bluetooth)]
[assembly: UsesPermission(Microsoft.Band.BandClientManager.BindBandService)]

namespace UVapp
{

    [Activity(Label = "UVSafe", ScreenOrientation = ScreenOrientation.Portrait)]

    public class MainActivity : AppCompatActivity
    {

        public static readonly string CHANNEL_ID = "notificationChannel1";
        internal static readonly string Key = "update";
        internal static string UVKey = "-1";
        // public string update = "uv detected";

        public static bool loggedIn = false; //should be replaced with a function -getter- from cloud

        static readonly int LOCATION_PERMISSION_CODE = 1000;

        private HttpClient httpClient;
        private IBandClient bandClient;
        private IBandConnectionCallback bandConnCallback;   // band connection "event listener"

        TextView currUvNumText, currUVWeatherText, bandConnText, accumulatedUVText,  currentlySamplingText;
        TextView  exposureTimeText, skinColorText, additionalTimeText, gettingUvWeatherText;
        ProgressBar accUVProgressBar;
        View rootLayout;
        Button connectBandButton;

        bool firstExposureNotificationSent = false;
        bool halfAllowedUVnotificationSent = false;

        Android.Locations.Location currentLocation;
        FusedLocationProviderClient fusedLocationProviderClient;
        bool locationPermissionGranted = false;

        double currentUV;
        double weatherCurrentUV = -1;
        double peakUV;
        
        double uvRadiationAccumulated = 0;  // The total accumulated radiation the user was exposed to in minutes*UVI
        double allowedUvRadiationLeft;         // The amount of additional radiation the user can safely be exposed to in minutes*UVI
        long exposureMinutesApp;    // The exposure minutes the app measures by itself - this is kept for testing but currently not used for any calculations
        long exposureMinutesBand;   // The exposure minutes the band measures - THE EXPOSURE TIME WE ACTUALLY USE

        double uvBandSamplingIntervalSeconds = 60;
        double bandConnectionTimeoutSeconds = 30;
        double locationSampleIntervalMinutes = 10;
        double weatherRequestIntervalMinutes = 30;  // It's actually limited to 50 requests per day

        User user;
        SkinType userSkinType;

        UVSensor uvSensor;

        Timer uvWeatherTimer;
        Timer uvSampleTimer;
        Timer updateLocationTimer;
        Timer bandConnTimer;
        Timer bandConnTimeoutTimer;
        Timer saveDataToDBTimer;
        DateTime lastUvSampleTime;

        bool connLostSinceLastSample = true;
        bool bandConnPreviouslySuccessful = false;

        // Constant strings that don't need to be retyped
        readonly string bandConnTextBase = "Band: ";
        readonly string samplingIntervalTextBase = "Sampling interval: ";
        readonly string currUVTextBase = "Measured UV: ";
        readonly string currUVWeatherTextBase = "Current UV according to Weather: ";
        readonly string uvMinutesTextBase = "Accumulated UV: ";
        readonly string skinColorTextBase = "Your skin type: ";
        readonly string appExposureTimeTextBase = "App measured exposure mins: ";
        readonly string bandExposureTimeTextBase = "Exposure time today: ";
        readonly string timeYouCanSpendTextBase = "Additional time you can spend under current level: ";
        readonly string uvMinutesLeftTextBase = "UV Minutes Left: ";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            //login
            if (!loggedIn) {
                Intent loginIntent = new Intent(this, typeof(Login_activity));
                StartActivity(loginIntent);
            }

            accUVProgressBar = FindViewById<ProgressBar>(Resource.Id.accUVProgressBar);
            bandConnText = FindViewById<TextView>(Resource.Id.bandConnectionText);
            currUvNumText = FindViewById<TextView>(Resource.Id.currentUVNumText);
            //currUVWeatherText = FindViewById<TextView>(Resource.Id.currentUVWeatherText);
            accumulatedUVText = FindViewById<TextView>(Resource.Id.acummulatedUVText);
            //samplingIntervalText = FindViewById<TextView>(Resource.Id.samplingIntervalText);
            currentlySamplingText = FindViewById<TextView>(Resource.Id.currentlySamplingText);
            //appExposureTimeText = FindViewById<TextView>(Resource.Id.appExposureTimeText);
            exposureTimeText = FindViewById<TextView>(Resource.Id.ExposureTimeText);
            skinColorText = FindViewById<TextView>(Resource.Id.skinColorText);
            additionalTimeText = FindViewById<TextView>(Resource.Id.additionalTimeText);
            //uvMinutesLeftText = FindViewById<TextView>(Resource.Id.uvMinutesLeftText);
            //gettingUvWeatherText = FindViewById<TextView>(Resource.Id.gettingUvFromWeatherText);

            rootLayout = FindViewById(Resource.Id.root_layout);

            connectBandButton = FindViewById<Button>(Resource.Id.connectbtn);

            connectBandButton.Click += ConnectBand;

            fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);
            
            /* Initialize timers, the elapsed callbacks are defined below
             * In general, once the band successfully connects, the 
             */ 

            bandConnTimer = new Timer(1000);
            bandConnTimer.Elapsed += ConnectBand;
            bandConnTimer.Enabled = true;
            bandConnTimer.AutoReset = false;

            uvSampleTimer = new Timer(1000);   // Initial interval is 1 second and it is changed after first sample
            uvSampleTimer.Elapsed += uvSampleTimerElapsed;
            uvSampleTimer.AutoReset = true;
            uvSampleTimer.Enabled = false; // Will only be enabled when the band connects

            bandConnTimeoutTimer = new Timer(SecondsToMS(bandConnectionTimeoutSeconds));
            bandConnTimeoutTimer.Enabled = false;
            bandConnTimeoutTimer.AutoReset = false;
            bandConnTimeoutTimer.Elapsed += BandConnTimeoutElapsed;

            updateLocationTimer = new Timer(1000);
            updateLocationTimer.Elapsed += updateLocationTimerElapsed;
            updateLocationTimer.AutoReset = true;
            updateLocationTimer.Enabled = true;     // The timer elapsed checks for permission


            uvWeatherTimer = new Timer(1000);     // Initial interval is 1 second and it is changed after first request
            uvWeatherTimer.Elapsed += uvWeatherTimerElapsed;
            uvWeatherTimer.AutoReset = true;
            uvWeatherTimer.Enabled = false;

            saveDataToDBTimer = new Timer(MinutesToMS(3));
            saveDataToDBTimer.Elapsed += saveDataToDB;
            saveDataToDBTimer.AutoReset = true;
            saveDataToDBTimer.Enabled = false; // Enabled after sample

            lastUvSampleTime = DateTime.MinValue;

            string userJson = Intent.GetStringExtra("userJson");
            if (userJson != null)
            {
                user = User.deserializeJson(userJson);
                userSkinType = (SkinType)user.skinType;
                if (user.Date == User.getTodayDateString())
                {
                    uvRadiationAccumulated = user.accumulatedUV;
                    exposureMinutesBand = user.TimeExposed;
                    allowedUvRadiationLeft = userSkinType.UVRadiationToBurn() - uvRadiationAccumulated;
                }
                else
                {
                    allowedUvRadiationLeft = userSkinType.UVRadiationToBurn();
                    uvRadiationAccumulated = 0;
                    exposureMinutesBand = 0;
                }
            }
            else
            {
                if (savedInstanceState == null)
                {
                    allowedUvRadiationLeft = userSkinType.UVRadiationToBurn();
                    uvRadiationAccumulated = 0;
                }
                else
                {
                    allowedUvRadiationLeft = savedInstanceState.GetDouble("uvMinutesLeft", userSkinType.UVRadiationToBurn());
                    uvRadiationAccumulated = savedInstanceState.GetDouble("uvMinutesSpent");
                    exposureMinutesBand = savedInstanceState.GetInt("exposureMinutesBand");
                }
            }
            

            //samplingIntervalText.Text = $"Sampling interval: {samplingIntervalMinutes} minutes";
            //currUVWeatherText.Text = "";
            bandConnText.Text = "";
            currentlySamplingText.Text = "";
            skinColorText.Text = skinColorTextBase + userSkinType.RomanNumeralsName();

            updateGUI();
            //appExposureTimeText.Text = appExposureTimeTextBase + 0;
            //uvMinutesLeftText.Text = uvMinutesLeftTextBase + (int)uvMinutesLeft;
            //gettingUvWeatherText.Text = "";
        }

        void updateGUI()
        {
            int roundedUV = (int)Math.Round(currentUV, 0, MidpointRounding.AwayFromZero);
            exposureTimeText.Text = FormatTimeAmountForUser(exposureMinutesBand);
            currUvNumText.Text = $"{roundedUV} ({UVvalues.UvIntToEnum(roundedUV)})";
            int uvPercentage = (int)(uvRadiationAccumulated * 100 / userSkinType.UVRadiationToBurn());


            accumulatedUVText.Text = $"{uvPercentage}%";
            accUVProgressBar.Progress = uvPercentage;
            //uvMinutesLeftText.Text = uvMinutesLeftTextBase + (int)uvMinutesLeft;
            if (currentUV != 0)
            {

                additionalTimeText.Text = FormatTimeAmountForUser(allowedUvRadiationLeft / currentUV);
            }
            else
            {
                additionalTimeText.Text = "Safe";
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutDouble("currentUV", currentUV);
            outState.PutDouble("uvMinutesSpent", uvRadiationAccumulated);
            outState.PutDouble("uvMinutesLeft", allowedUvRadiationLeft);
            outState.PutLong("exposureMinutesApp", exposureMinutesApp);
            outState.PutLong("exposureMinutesBand", exposureMinutesBand);
        }
       
        /*
        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);

            currentUV = savedInstanceState.GetDouble("currentUV");
            uvRadiationAccumulated = savedInstanceState.GetDouble("uvMinutesSpent");
            allowedUvRadiationLeft = savedInstanceState.GetDouble("uvMinutesLeft");
            exposureMinutesApp = savedInstanceState.GetLong("exposureMinutesApp");
            exposureMinutesBand = savedInstanceState.GetLong("exposureMinutesBand");
        }
        */
        private async void ConnectBand(object sender, System.EventArgs e)
        {
            RunOnUiThread(() =>
            {
                bandConnText.Text = bandConnTextBase + "\nConnecting...";
            });

            IBandInfo[] pairedBands = BandClientManager.Instance.GetPairedBands();

            if (pairedBands.Length < 1)
            {
                RunOnUiThread(() =>
                {
                    bandConnText.Text = "Band not paired!";
                });
                
                return;
            }

            if (bandClient == null)
                bandClient = BandClientManager.Instance.Create(BaseContext, pairedBands[0]);

            ConnectionState connState = bandClient.ConnectionState;

            if (!bandClient.IsConnected)
            {
                if (bandConnCallback != null)
                {
                    bandClient.UnregisterConnectionCallback();
                }

                bandConnCallback = bandClient.RegisterConnectionCallback(async connectionState =>
                {
                    
                    
                    if (connectionState == ConnectionState.Connected)
                    {
                        RunOnUiThread(() =>
                        {
                            bandConnText.Text = bandConnTextBase + "Connected";
                            connectBandButton.Clickable = false;
                            connectBandButton.Visibility = ViewStates.Invisible;
                        });

                        bandConnPreviouslySuccessful = true;
                        bandConnTimer.Enabled = false;
                        bandConnTimeoutTimer.Enabled = false;
                        uvSampleTimer.Interval = 1000;
                        uvSampleTimer.Start();
                    }
                    else
                    {
                        RunOnUiThread(() =>
                        {
                            bandConnText.Text = bandConnTextBase + "\nConnecting...";
                        });
                        connLostSinceLastSample = true;

                        if (bandConnPreviouslySuccessful)
                        {
                            bandConnTimer.Enabled = true;
                        }
                        
                        bandConnTimeoutTimer.Enabled = true;
                        
                        RunOnUiThread(() =>
                        {
                            currentlySamplingText.Text = "";
                        });
                        
                        uvSampleTimer.Stop();
                    }
                });

                try
                {
                    connState = await bandClient.ConnectTaskAsync();
                    if (connState != ConnectionState.Connected)
                    {
                        RunOnUiThread(() =>
                        {
                            bandConnText.Text += "\nBand connection failed";
                        });
                    }
                }
                catch(BandException ex)
                {
                    RunOnUiThread(() =>
                    {
                        bandConnText.Text = "Band Connection Error: " + ex.Message;
                    });
                }
            } 
        }

        private async Task SampleBandUV()
        {
            try
            {
                
                if (bandClient == null)
                {
                    return;
                }

                if (uvSensor == null)
                    uvSensor = bandClient.SensorManager.CreateUVSensor();

                UVIndexLevel uviBandEnum = null;

                uvSensor.ReadingChanged += (o, args) =>
                {
                    
                    uviBandEnum = args.SensorReading.UVIndexLevel;
                    double uviNum = WeatherUV.CompareBandAndWeatherUV(uviBandEnum, weatherCurrentUV);
                    

                    long prevExposureMinutes = exposureMinutesBand;
                    exposureMinutesBand = args.SensorReading.UVExposureToday;
                    long exposureSinceLastSample = exposureMinutesBand - prevExposureMinutes;

                    currentUV = uviNum;

                    // This updates our internal time measure which is currently not used
                    if (!connLostSinceLastSample)
                    {
                        TimeSpan timeSinceLastSample = DateTime.Now - lastUvSampleTime;
                        
                        if (currentUV != 0)
                            exposureMinutesApp += (long)timeSinceLastSample.TotalMinutes;
                        
                    }


                    // Update accumulated radiation
                    if (currentUV != 0 || weatherCurrentUV ==-1)
                    {
                        uvRadiationAccumulated += currentUV * exposureSinceLastSample;
                        allowedUvRadiationLeft -= currentUV * exposureSinceLastSample;
                    }
                    else
                    {
                        /* If the last uv sample was 0, we use the weather UV
                         * Note that exposureSinceLastSample may be 0 if there
                         * was no exposure
                         */
                        uvRadiationAccumulated += weatherCurrentUV * exposureSinceLastSample;
                        allowedUvRadiationLeft -= weatherCurrentUV * exposureSinceLastSample;
                    }

                    
                    connLostSinceLastSample = false;
                    lastUvSampleTime = DateTime.Now;

                    saveDataToDBTimer.Start();

                    if (peakUV < currentUV)
                    {
                        peakUV = currentUV;
                    }

                    int roundedUV = (int)Math.Round(currentUV, 0, MidpointRounding.AwayFromZero);

                    // Send notification for first UV detected
                    if (currentUV > 0 && !firstExposureNotificationSent) {
                        NotifyUser($"UV level of {roundedUV} detected!", $"If you are going to be exposed to the sun for more than {FormatTimeAmountForUser(userSkinType.MinutesToBurn(currentUV))}, wear protective clothing, hat and UV-blocking sunglasses and apply SPF 30+ sunscreen");
                        firstExposureNotificationSent = true;
                    }

                    // Send notification for exposure to half of the allowed radiation
                    if (allowedUvRadiationLeft < userSkinType.UVRadiationToBurn()/2 && !halfAllowedUVnotificationSent)
                    {
                        NotifyUser("You've been exposed to over 50% of your allowed UV today!", $"Try to avoid additional sun exposure and make sure to apply SPF 30+ sunscreen and wear protective clothing, especially if you are going to be exposed for more than { FormatTimeAmountForUser(allowedUvRadiationLeft / currentUV)}");
                        halfAllowedUVnotificationSent = true;
                    }

                    RunOnUiThread(() => {      // To access the text, you need to run on ui thread
                        updateGUI();

                        currentlySamplingText.Text = "";
                    });
                };
                
                uvSensor.StartReadings();
            }
            catch (BandException ex)
            {
                currUvNumText.Text = "Error Reading UV: " + ex.Message;
            }
        }

        private async void uvSampleTimerElapsed(object sender, System.EventArgs args)
        {
           
            RunOnUiThread(() => {      // To access the text, you need to run on ui thread
                currentlySamplingText.Text = "Sampling UV...";
            });
            
            uvSampleTimer.Interval = SecondsToMS(uvBandSamplingIntervalSeconds);
            await SampleBandUV();
            
        }

        private async void uvWeatherTimerElapsed(object sender, System.EventArgs args)
        {
            uvWeatherTimer.Interval = MinutesToMS(weatherRequestIntervalMinutes);

            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }

            /*
            RunOnUiThread(() =>
            {
                gettingUvWeatherText.Text = "Getting UV from weather...";
            });
            */
            if (currentLocation != null)
                weatherCurrentUV = await WeatherUV.GetWeatherUvAsync(httpClient, currentLocation.Latitude, currentLocation.Longitude);
            
            /*
            RunOnUiThread(() =>
            {
                gettingUvWeatherText.Text = "";
                if (weatherCurrentUV != -1)
                    currUVWeatherText.Text = currUVWeatherTextBase + (int)Math.Round(weatherCurrentUV, 0, MidpointRounding.AwayFromZero);
                else
                    currUVWeatherText.Text = "Error getting weather UV!";
            });
            */
        }

        private async void updateLocationTimerElapsed(object sender, System.EventArgs args)
        {
            updateLocationTimer.Interval = MinutesToMS(locationSampleIntervalMinutes);
            await UpdateLocation(true);   // permissionCheck = true
            /*
            if (!locationPermissionGranted)
            {
                RunOnUiThread(() => { currUVWeatherText.Text = currUVWeatherTextBase + "Location permission not granted"; });
                return;
            }
            

            if (currentLocation == null)
            {
                RunOnUiThread(() => { currUVWeatherText.Text = currUVWeatherTextBase + "Error getting location"; });
                return;
            }
            */
        }

        private void BandConnTimeoutElapsed(object sender, System.EventArgs args)
        {
            RunOnUiThread(() =>
            {
                bandConnText.Text = bandConnTextBase + "\nCould not connect";
                connectBandButton.Clickable = true;
                connectBandButton.Visibility = ViewStates.Visible;
            });
            
            // TODO: show "Could not connect band" message and show a retry button. Use ConnectBand for the click event
            if (bandConnPreviouslySuccessful)
                NotifyUser("Band Connection Lost", "Check your bluetooth and band and reconnect");
        }

        private async void saveDataToDB(object sender, System.EventArgs args)
        {
            /*
            if (currentUV == 0)
            {
                // Don't save if there is no exposure to prevent unnecessary accesses to DB
                saveDataToDBTimer.Enabled = false;
            }
            */
            user.TimeExposed = exposureMinutesBand;
            user.accumulatedUV = uvRadiationAccumulated;
            user.Date = User.getTodayDateString();

            await UserManager.UpdateUser(user);
        }
        

        private async Task<string> GetEnumUVRecommendation(UVIndexLevel uvi)
        {
            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }
            return ClientRecommendations.getEnumUVRecommendation(uvi);
        }

        private string GetIntUVRecommendation(int uv)
        {
            return ClientRecommendations.getIntUVRecommendation(uv);
        }


        ////////////////////// Utility functions /////////////////////

        public static string FormatTimeAmountForUser(double minutes)
        {
            if (minutes < 60)
            {
                return $"{(int)minutes} minutes";
            }
            else if (minutes < 120)
            {
                return $"{(int)(minutes / 60)}h {((int)minutes)%60}m";
            }
            else
            {
                return $"{(int)(minutes / 60)}h {((int)minutes) % 60}m";
            }
        }
        double MinutesToMS(double minutes)
        {
            return 60 * 1000 * minutes;
        }
        double MsToMinutes(double milliseconds)
        {
            return milliseconds / (60 * 1000);
        }
        double SecondsToMS(double seconds)
        {
            return seconds * 1000;
        }


        ////////////////////// PERMISSIONS /////////////////////

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == LOCATION_PERMISSION_CODE)
            {
                if (grantResults.Length == 1 && grantResults[0] == Permission.Granted)
                {
                    UpdateLocation(false);
                }
                else
                {
                    ShowLocationPermissionRequestSnackbar();
                }
            }

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void ShowLocationPermissionRequestSnackbar()
        {
            /* Snackbar requires the theme to be AppCompat or a descendant.
             * If you want to use a different theme,
             * replace this snackbar with something that works with it
             */ 
            Snackbar.Make(rootLayout, "Location is needed to determine local UV through weather", Snackbar.LengthIndefinite)
                        .SetAction("ok",
                                   delegate
                                   {
                                       ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.AccessCoarseLocation }, LOCATION_PERMISSION_CODE);
                                   })
                        .Show();
        }

        void RequestLocationPermission()
        {
            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessCoarseLocation))
            {
                ShowLocationPermissionRequestSnackbar();
            }
            else
            {
                ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.AccessCoarseLocation }, LOCATION_PERMISSION_CODE);
            }
        }

        async Task UpdateLocation(bool permissionCheck)
        {
            if (!permissionCheck || ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) == Permission.Granted)
            {
                currentLocation = await fusedLocationProviderClient.GetLastLocationAsync();
                locationPermissionGranted = true;
                uvWeatherTimer.Enabled = true;
            }
            else
            {
                RequestLocationPermission();
            }
        }

        ////////////////////// NOTIFICATIONS /////////////////////
        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var name = Resources.GetString(Resource.String.channel_name);
            var description = GetString(Resource.String.channel_describtion);
            var channel = new NotificationChannel(CHANNEL_ID, name, NotificationImportance.Default)
            {
                Description = description
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }



        void NotifyUser(string title, string update)
        {

            Intent notifyIntent = new Intent(this, typeof(NotificationService));
            notifyIntent.PutExtra("update" , update);
            notifyIntent.PutExtra("title", title);
            this.StartService(notifyIntent);

        }
    }
}

