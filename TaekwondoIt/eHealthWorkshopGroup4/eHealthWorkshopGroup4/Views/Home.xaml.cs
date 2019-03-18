using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using Microcharts;
using Entry = Microcharts.Entry;
using eHealthWorkshopGroup4.Models;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Text;
using System.Net.Http;
using CloudStorage;
using System.Linq;
using Newtonsoft.Json;

namespace eHealthWorkshopGroup4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {

        #region Properties declaration
        private List<String> groups;
        private bool areThereExercises;

        public List<string> MyGroups { get { return groups; }
            private set {
                groups = value;
                OnPropertyChanged();
            }
        }
        private bool viewGroups= true;
        public bool IsCoach
        {
            get { return App.IsCoach && viewGroups; }
            private set
            {
                viewGroups = value;
                OnPropertyChanged();
            }
        }
        private int pickerSelfIndex;
        public bool AreThereExercises
        {
            private set
            {
                areThereExercises = value;
                OnPropertyChanged();
            }
            get { return areThereExercises; }
        }
        String SelectedGroup { get; set; }
        public List<Exercise> Exercises { get; private set; }

        private CloudStorageHandler handler = new CloudStorageHandler();
        private LiveIdCredentials creds = new LiveIdCredentials();
        private static readonly HttpClient client = new HttpClient();
        //
        // The ClientId and ClientSecret are obtained from:
        // https://account.live.com/developers/applications
        //
        // See the conceptual document for the Microsoft Health Service for more information
        // about this topic.
        //
        private const string ClientId = "a965a101-aa3c-479c-aa18-7a308a850315";
        //
        // Choose the minimum set of authorization scopes required by your application.
        //
        private const string Scopes = "mshealth.ReadActivityHistory";
        public const string CoachStatisticsPanel = "Show My Statistics";
        private const string BaseHealthUri = "https://api.microsofthealth.net/v1/me/";
        private const string RedirectUri = "https://login.live.com/oauth20_desktop.srf";

        #endregion

        #region data management
        protected async override void OnAppearing()
        {
            //If coach - get groups
            if (App.IsCoach) {
                picker.SelectedIndexChanged += Picker_SelectedIndexChanged;
                MyGroups = await handler.getGroupsOfCoach(App.MyUserName);
                MyGroups.Add(CoachStatisticsPanel);
                pickerSelfIndex = MyGroups.LastIndexOf(CoachStatisticsPanel);

                OnPropertyChanged();
            }
            // if trainee - get exercises
            else
            {
               Exercises = await handler.getUserExercises(App.MyUserName, DateTime.Now.AddMonths(-1));
                UpdateCharts();
            }
        }

        private async void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Update Charts according to selected group.
            if (picker.SelectedIndex == pickerSelfIndex)
            {
                //treat as trainee
                Exercises = await handler.getUserExercises(App.MyUserName, DateTime.Now.AddDays(-7));
                IsCoach = false;
            }
            else
            {
                Exercises = await handler.GetGroupLatestExercises(SelectedGroup);
                IsCoach = true;
            }
            UpdateCharts();

        }

        private Entry Exercise2Peak(Exercise x)
        {
            return new Entry((float)x.peakHR)
            {
                Label = (IsCoach?x.uname:x.startTime.ToShortDateString()),
                Color = SKColors.Red
            };
        }

        private Entry Exercise2Avg(Exercise x)
        {
            return new Entry((float)x.avgHR)
            {
                Label = (IsCoach ? x.uname : x.startTime.ToShortDateString()),
                ValueLabel = x.avgHR.ToString(),
                Color = SKColors.Green
            };
        }

        private Entry Exercise2Duration(Exercise x)
        {
            return new Entry((float)x.duration)
            {
                Label = (IsCoach ? x.uname : x.startTime.ToShortDateString()),
                ValueLabel = x.duration.ToString(),
                Color = SKColors.AliceBlue
            };
        }

        private void UpdateCharts()
        {
            if (IsCoach)
            {
                if (Exercises != null && Exercises.Count > 0)
                {
                    AvgHR.Chart = new BarChart() { Entries = Exercises.Select(x => Exercise2Avg(x)) };
                    PeakHR.Chart = new BarChart() { Entries = Exercises.Select(x => Exercise2Peak(x)) };
                    AreThereExercises = true;
                }
                else
                {
                    AreThereExercises = false;
                }
            }
            else
            {
                if (Exercises != null && Exercises.Count > 0)
                {
                    AvgHR.Chart = new LineChart() { Entries = Exercises.Select(x => Exercise2Avg(x)) };
                    PeakHR.Chart = new LineChart() { Entries = Exercises.Select(x => Exercise2Peak(x)) };
                    Duration.Chart = new BarChart() { Entries = Exercises.Select(x => Exercise2Duration(x)) };
                    AreThereExercises = true;
                }

                else
                {
                    AreThereExercises = false;
                }
            }

        }
        #endregion


        #region API requests

        private async void GetAcitvities()
        {
            string startTime = Uri.EscapeDataString(DateTime.UtcNow.AddDays(-29).ToString("O"));
            string endtime = Uri.EscapeDataString(DateTime.UtcNow.ToString("O"));

            await this.PerformRequest(
                "Activities",
                string.Format("startTime={0}&endTime={1}&activityTypes=GuidedWorkout", startTime, endtime));
        }

        private async Task PerformRequest(string relativePath, string queryParams = null)
        {
            try
            {

                var uriBuilder = new UriBuilder(BaseHealthUri);

                uriBuilder.Path += relativePath;

                uriBuilder.Query = queryParams;

                var request = HttpWebRequest.Create(uriBuilder.Uri);

                request.Headers[HttpRequestHeader.Authorization] = string.Format("bearer {0}", this.creds.AccessToken);

                using (var response = await request.GetResponseAsync())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var json = reader.ReadToEnd();
                            MSHealthActivities activities =  JsonConvert.DeserializeObject<MSHealthActivities>(json);
                            var myExercises = activities.ToExercises();
                            //push new exercises
                            await handler.addExercisesAsync(myExercises);
                            //fetch new exercises 
                            Exercises = await handler.getUserExercises(App.MyUserName, DateTime.Now.AddDays(-7));
                            //update picker index
                            picker.SelectedIndex = pickerSelfIndex;
                            UpdateCharts();

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion



        private void SyncButton_Click(object sender, EventArgs e)
        {
            UriBuilder uri = new UriBuilder("https://login.live.com/oauth20_authorize.srf");
            var query = new StringBuilder();

            query.AppendFormat("redirect_uri={0}", Uri.EscapeDataString(RedirectUri));
            query.AppendFormat("&client_id={0}", Uri.EscapeDataString(ClientId));

            query.AppendFormat("&scope={0}", Uri.EscapeDataString(Scopes));
            query.Append("&response_type=code");

            uri.Query = query.ToString();
            webView.Source = uri.Uri;
            webView.IsVisible =true;
        }

        private void SignoutButton_Click(object sender, EventArgs e)
        {
            UriBuilder uri = new UriBuilder("https://login.live.com/oauth20_logout.srf");
            var query = new StringBuilder();

            query.AppendFormat("redirect_uri={0}", Uri.EscapeDataString(RedirectUri));
            query.AppendFormat("&client_id={0}", Uri.EscapeDataString(ClientId));

            uri.Query = query.ToString();

            this.webView.Source = uri.Uri;
        }

        private async void WebView_NavigationCompleted(WebView sender, WebNavigatedEventArgs args)
        {
            //
            // When the web view navigates to our redirect URI, extract the authorization code from
            // the URI and use it to fetch our access token. If no authorization code is present,
            // we're completing a sign-out flow.
            //
            Uri uri = new Uri(args.Url);
            String localPath = uri.LocalPath;
            {
                var decoder = HttpUtility.ParseQueryString(uri.Query);
                var code = decoder["code"];

                var error = decoder["error"];
                var errorDesc = decoder["error_description"];

                // Check the code to see if this is sign-in or sign-out
                if (code != null)
                {
                    // Hide the browser again, no matter what happened...
                    sender.IsVisible = false;

                    if (error != null)
                    {
                        return;
                    }

                    var tokenError = await this.GetToken(code, false);

                    if (string.IsNullOrEmpty(tokenError))
                    {
                        GetAcitvities();
                    }
                }
            }
        }

        private async Task<string> GetToken(string code, bool isRefresh)
        {
            UriBuilder uri = new UriBuilder("https://login.live.com/oauth20_token.srf");
            var query = new StringBuilder();

            query.AppendFormat("redirect_uri={0}", Uri.EscapeUriString(RedirectUri));
            query.AppendFormat("&client_id={0}", Uri.EscapeDataString(ClientId));
            if (isRefresh)
            {
                query.AppendFormat("&refresh_token={0}", Uri.EscapeDataString(code));
                query.Append("&grant_type=refresh_token");
            }
            else
            {
                query.AppendFormat("&code={0}", Uri.EscapeDataString(code));
                query.Append("&grant_type=authorization_code");
            }

            string postData = query.ToString();
            var uriInstance = uri.Uri;
            var request = WebRequest.Create(uriInstance);
            request.Method = "POST";
            // Create POST data and convert it to a byte array.  
            uri.Query =postData;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.  
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.  
            request.ContentLength = byteArray.Length;
            // Get the request stream.  
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.  
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.  
            dataStream.Close();
            try
            {
                using (var response = await request.GetResponseAsync())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            var responseString = streamReader.ReadToEnd();
                            var jsonResponse = JObject.Parse(responseString);
                            this.creds.AccessToken = (string)jsonResponse["access_token"];
                            this.creds.ExpiresIn = (long)jsonResponse["expires_in"];
                            this.creds.RefreshToken = (string)jsonResponse["refresh_token"];
                            string error = (string)jsonResponse["error"];

                            return error;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Home ()
		{
			InitializeComponent();
            picker.IsVisible = App.IsCoach;   
        }
	}
}