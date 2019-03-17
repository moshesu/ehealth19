using System;
using Xamarin.Forms;
using System.Web;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using SleepItOff.Cloud.AzureDatabase;
using SleepItOff.Entities;
using System.Collections.Generic;


namespace SleepItOff
{
    public partial class LoadingLabelXaml : ContentPage
    {
        
        private const string client_id = "05c0fa93-a596-4484-a671-98a5e35db4ee";
        private const string scope = "mshealth.ReadDevices mshealth.ReadActivityHistory mshealth.ReadActivityLocation mshealth.ReadDevices mshealth.ReadProfile offline_access";
        private const string redirect_uri = "https://login.microsoftonline.com/common/oauth2/nativeclient";
        public string response_code = ""; //getting after first GET
        private const string BaseHealthUri = "https://api.microsofthealth.net/v1/me/";


        public LoadingLabelXaml()
        {
            //create URL to send
            UriBuilder uri = new UriBuilder("https://login.live.com/oauth20_authorize.srf");
            var query = new StringBuilder();
            query.AppendFormat("redirect_uri={0}", Uri.EscapeDataString(redirect_uri));
            query.AppendFormat("&client_id={0}", Uri.EscapeDataString(client_id));
            query.AppendFormat("&scope={0}", Uri.EscapeDataString(scope));
            query.Append("&response_type=code");
            uri.Query = query.ToString();

            //initializing webview - web window with permisions to msHelath request
            InitializeComponent();
            var layout = new StackLayout();
            _webView.HeightRequest = 1000;
            _webView.WidthRequest = 1000;
            _webView.Source = uri.Uri;
            _webView.Navigated += webviewNavigated;
            layout.Children.Add(_webView);
            Content = layout;               
        }


        // Called when the webview finished navigating.   
        async void webviewNavigated(object sender, WebNavigatedEventArgs e)
        {                     
            string responseURL =  e.Url;
            //checks if this this the final url - where the auth code is
            if (responseURL.StartsWith("https://login.microsoftonline.com/common/oauth2/nativeclient", true,null))
            {
                Uri responseURLtoURI = new Uri(responseURL);
                response_code = HttpUtility.ParseQueryString(responseURLtoURI.Query).Get("code");
                string error = HttpUtility.ParseQueryString(responseURLtoURI.Query).Get("error");
                string errorDesc = HttpUtility.ParseQueryString(responseURLtoURI.Query).Get("errorDesc");

                if (response_code != null) {                    
                     await GettingTokken(sender, e); //after we have the code, trying to get the token
                }
                else if (error != null)
                {
                    if (errorDesc != null)
                    {
                        //todo label of discription
                    };
                };
            }
        }

        private async Task GettingTokken(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ConnectingPage()); //todo - show connecting gif
            //create URL to send
            UriBuilder uri = new UriBuilder("https://login.live.com/oauth20_token.srf");
            var query = new StringBuilder();
            uri.Query = query.ToString();

            //creating POST URL+body in application/x-www-form-urlencoded format
            var httpClient = new HttpClient();
            httpClient.BaseAddress = uri.Uri;
            String urlParameters = "client_id=" + client_id + "&redirect_uri=" + redirect_uri + "&code=" + response_code + "&grant_type=authorization_code";
            StringContent bodyParam = new StringContent(urlParameters, Encoding.UTF8, "application/x-www-form-urlencoded");

            //waiting to response
            var response = await httpClient.PostAsync(uri.Uri, bodyParam);

            //converting the response to JSON format
            var stringResponse = response.Content.ReadAsStringAsync().Result;
            var jsonResponse = JObject.Parse(stringResponse);
            //reading JSON response and updating credentials
            LiveIdCredentials.AccessToken = (string)jsonResponse["access_token"];
            LiveIdCredentials.ExpiresIn = (long)jsonResponse["expires_in"];
            LiveIdCredentials.RefreshToken = (string)jsonResponse["refresh_token"];
            Console.WriteLine(LiveIdCredentials.RefreshToken);
            string error = (string)jsonResponse["error"];
            
            await Utils.GetUserProfile(sender, e);
            await Utils.GetUserId(sender, e);
            await Utils.getSleepStatsOfUser(sender, e);

            /* checking if the userId is already in the ManageUsers table in the database.
               If not (in case it's a new user), add the user to 2 tables:
                    1. ManageUsers
                    2. UserDetails                                                      */
            UserDetailsRecord userDetails = new UserDetailsRecord(LiveIdCredentials.userId, LiveIdCredentials.firstName,
                                                       LiveIdCredentials.lastName, LiveIdCredentials.gender,
                                                       Utils.calculateUserAge(), LiveIdCredentials.height, LiveIdCredentials.weight);

            bool userInDB = dbUtils.assertUserInDB(LiveIdCredentials.userId);
            if (!userInDB)
            {
                UserRecord user = new UserRecord(LiveIdCredentials.userId,  DateTime.Now);
                dbUtils.addUserToDB(user, userDetails);
            }
            else dbUtils.addUserToUserDetailsTable(userDetails);

            /* after each sign in, update the SleepQuality table in the database */
            UserSleepQualityRecord userSleepQuality = new UserSleepQualityRecord(LiveIdCredentials.userId,
                                                                                  LiveIdCredentials.mean_num_of_wakeups,
                                                                                  LiveIdCredentials.mean_sleep_efficience);
            dbUtils.updateUserSleepQualityDetails(userSleepQuality);

            /* after each sign in, update the SleepSegmentsStats table in the database */
            SegmentSummaryTable.Awake = new SegmentSummary();
            SegmentSummaryTable.Doze = new SegmentSummary();
            SegmentSummaryTable.Snooze = new SegmentSummary();
            SegmentSummaryTable.RestlessSleep = new SegmentSummary();
            SegmentSummaryTable.RestfulSleep = new SegmentSummary();
            SegmentSummaryTable.REMSleep = new SegmentSummary();

            SleepSegmentsStatsRepository repo = new SleepSegmentsStatsRepository();
            string UserId = LiveIdCredentials.userId;
            DateTime last_updated_time_of_segments_stats = new DateTime(1990,1,1);

            UserSleepSegmentsStatsRecord SleepSegmentsStatsRecord = repo.GetUserSleepSegmentsStats(UserId);
            if (SleepSegmentsStatsRecord == null) //there is no data exists on the user in SleepSegmentsStats table
            {
                await Utils.getSleepSegmentsStats(sender, e, last_updated_time_of_segments_stats);
                //bool saveSuccess = handleUserSleepSegmentsData(repo, UserId, 0); //insert user row to table
                bool saveSuccess = Utils.handleUserSleepSegmentsData(repo, UserId, 0); //insert user row to table
                if (!saveSuccess) { } //error
                }
            else
            {
                last_updated_time_of_segments_stats = SleepSegmentsStatsRecord.lastUpdated;
                //updateSegmentSummaryTableFromDB(SleepSegmentsStatsRecord); // update SegmentSummaryTable object from DB data
                Utils.updateSegmentSummaryTableFromDB(SleepSegmentsStatsRecord); // update SegmentSummaryTable object from DB data
                await Utils.getSleepSegmentsStats(sender, e, last_updated_time_of_segments_stats);
                //bool updateSuccess = handleUserSleepSegmentsData(repo, UserId, 1); //update user row in table
                bool updateSuccess = Utils.handleUserSleepSegmentsData(repo, UserId, 1); //update user row in table
                if (!updateSuccess) { } //error
            }

            /* update segment summary table by user seniority in eSleeping and avg of other same users */
            float seniority = SegmentSummaryTable.getUserSeniority();
            TimeSpan ageTime = DateTime.Now.Subtract(LiveIdCredentials.birthdate);
            int age = ageTime.Days / 365;
            
            List<int> weightedSleepSegmentsStats = dbUtils.defineUserSegmentsTransitionsProbabilitiesCombined(LiveIdCredentials.userId, age, LiveIdCredentials.height, LiveIdCredentials.weight, LiveIdCredentials.gender, seniority);

            SegmentSummaryTable.updateTableByList(weightedSleepSegmentsStats);

            /* updating needed values for the Statistics Pages */
            var sleepQualityRepo = new SleepQualityRepository();
            var quality = sleepQualityRepo.checkUserSleepQualityForHisAge(LiveIdCredentials.userId, Utils.calculateUserAge());
            StatisticsPage.sleepEffAge = (int)quality.Item1;
            StatisticsPage.wakeUpAge = (int)quality.Item2;
            StatisticsPage.userSleepQualityForHisAgeGraphValue = sleepQualityRepo.calculateQualityLevel((int)quality.Item1, (int)quality.Item2);
            quality = sleepQualityRepo.checkUserSleepQualityForHisGender(LiveIdCredentials.userId, LiveIdCredentials.gender);
            StatisticsPage.sleepEffGender = (int)quality.Item1;
            StatisticsPage.wakeUpGender = (int)quality.Item2;
            StatisticsPage.userSleepQualityForHisGenderGraphValue = sleepQualityRepo.calculateQualityLevel((int)quality.Item1, (int)quality.Item2);
            quality = sleepQualityRepo.checkUserSleepQualityForHisGenderAndAge(LiveIdCredentials.userId, Utils.calculateUserAge(), LiveIdCredentials.gender);
            StatisticsPage.userStressLevelGraphValue = (int)quality.Item1;
            /* done updating needed values for the Statistics Pages */

            await Navigation.PushAsync(new MainPage()); //back to main

            startRefreshing(sender, e); //refreshing tokken
        }

        private async void startRefreshing(object sender, EventArgs e)
        {
            while (true)
            {
                await Task.Delay(new TimeSpan(0, 55, 0));
                await RefreshToken(sender, e);
            }
        }

        private async Task RefreshToken(object sender, EventArgs e)
        {
            //create URL to send
            UriBuilder uri = new UriBuilder("https://login.live.com/oauth20_token.srf");
            var query = new StringBuilder();
            uri.Query = query.ToString();

            //creating POST URL+body in application/x-www-form-urlencoded format
            var httpClient = new HttpClient();
            httpClient.BaseAddress = uri.Uri;
            String urlParameters = "client_id=" + client_id + "&redirect_uri=" + redirect_uri + "&refresh_token=" + LiveIdCredentials.RefreshToken + "&grant_type=refresh_token";
            StringContent bodyParam = new StringContent(urlParameters, Encoding.UTF8, "application/x-www-form-urlencoded");

            //waiting to response
            var response = await httpClient.PostAsync(uri.Uri, bodyParam);
            var stringResponse = response.Content.ReadAsStringAsync().Result;
            //var stringResponse = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            
            //reading JSON response and updating credentials
            var jsonResponse = JObject.Parse(stringResponse);
            
            LiveIdCredentials.AccessToken = (string)jsonResponse["access_token"];
            LiveIdCredentials.ExpiresIn = (long)jsonResponse["expires_in"];
            LiveIdCredentials.RefreshToken = (string)jsonResponse["refresh_token"];
            string error = (string)jsonResponse["error"];
        }

        //todo - didnt implemented the button
        public static async Task SignoutButton_ClickAsync(object sender, EventArgs e)
        {
            UriBuilder uri = new UriBuilder("https://login.live.com/oauth20_logout.srf");
            var query = new StringBuilder();
            query.AppendFormat("redirect_uri={0}", Uri.EscapeDataString(redirect_uri));
            query.AppendFormat("&client_id={0}", Uri.EscapeDataString(client_id));
            uri.Query = query.ToString();

            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri.Uri);
            var httpResponse = await httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);

            DependencyService.Get<IClearCookies>().ClearAllCookies(); //clear cookies - sign out from microsoft user
            LiveIdCredentials.ClearCreds(); //clear LiveIdCredentials
            SegmentSummaryTable.ClearSegmentTable(); //clearSleepStats

        }

        /*
         * @param exp_sleep_start_time - The time when the user pressed to start sleep tracking. The time in DateTime format
         * @after: liveIdCredentials.Last_Sleep_Segment field updated
         */
        public static async Task getLastSleepActivity(object sender, EventArgs e, DateTime exp_sleep_start_time)
        {
            //create URL to send
            UriBuilder uri = new UriBuilder("https://api.microsofthealth.net/v1/me/Activities/");
            var query = new StringBuilder();
            query.AppendFormat("startTime={0}", Uri.EscapeDataString(exp_sleep_start_time.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")));
            //query.AppendFormat("&activityTypes={0}", Uri.EscapeDataString("sleep"));
            query.AppendFormat("&ActivityIncludes={0}", Uri.EscapeDataString("Details"));                        
            uri.Query = query.ToString();

            //sending URL and waiting for response code
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri.Uri);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LiveIdCredentials.AccessToken);
            var httpResponse = httpClient.SendAsync(httpRequestMessage);
            
            //converting the response to JSON format
            var stringResponse = httpResponse.Result.Content.ReadAsStringAsync().Result;
            if (stringResponse != null) { 
                ActivityResponse activityResponse = JsonConvert.DeserializeObject<ActivityResponse>(stringResponse);
                if (activityResponse != null &&activityResponse.sleepActivities!=null)
                {
                    LiveIdCredentials.Last_Sleep_Segment = activityResponse.sleepActivities.Last().activitySegments.Last();
                    LiveIdCredentials.SleepStart = activityResponse.sleepActivities.Last().fallAsleepTime;
                }
                
            }                

        }

        /*@prev LiveIdCredwntials.Last_Sleep_Segment was updated
        * @param last_alarm_time - The time when the user wanted to wake up.
        * @param alarmType - (1-standart smart alarm. 2-combat nap. 3-creative sleep)
        * criterions for wake up:
             * Standart smart alarm (light sleep):
             *      1. Previous segment was deep sleep. 
             *          (All other segments are good to wake up, 
             *          so if the previous segment was deep, the current segment is not deep)    
             *      2. Less than 30 minutes before the alarm clock.
             *      3. More than 15 minutes from the end of last deep sleep segment.
             * Combat Nap (light sleep) - one of them:
             *      1. Previous segment was Snooze or LightSleep. 
             *          1.1 Less then 15 minutes to last_alarm_time.
             *      2. Previous segment was deep sleep.
             *          2.1 Less then 30 minutes from the end of that segment to last_alarm_time. (no checking for 15 minutes like in smart alarm, because the cicle is shorter at the start of the sleeping activity)
             * Creative sleep (REM sleep):
             *      1. At least 90 minutes from asleep.
             *      2. Previuos segment was deep sleep.
             *      3. At least 40 minutes from the end of the last segment.
        * @return value: 1 - iff it is a time to wake up (by the alarm type). 2 - if creative sleep or smart alarm and 1+2 criterions are true (time to refresh recently). 0 - otherwise
        */
        public static int WakeUpLogic(int alarmType, DateTime last_alarm_time)
        {            
            if (alarmType == (int)AlarmType.SmartAlarm)
            {
                if (LiveIdCredentials.Last_Sleep_Segment!=null && LiveIdCredentials.Last_Sleep_Segment.sleepType == "RestfulSleep")
                {
                    if (DateTime.Now.AddMinutes(30).CompareTo(last_alarm_time) == 1) //less than 30 minutes before last alarm time
                    {
                        if (DateTime.Now.AddMinutes(-15).CompareTo(LiveIdCredentials.Last_Sleep_Segment.endTime) == 1)//more than 15 minutes from the end of the last segment
                        {
                            return 1;
                        }
                        else
                        {
                            return 2;
                        }
                    }
                }
            }
            else if(alarmType == (int)AlarmType.CombatNap)
            {
                if ((LiveIdCredentials.Last_Sleep_Segment.segmentType == "Snooze") || (LiveIdCredentials.Last_Sleep_Segment.sleepType == "RestlessSleep"))
                {
                    if (DateTime.Now.AddMinutes(15).CompareTo(last_alarm_time) == 1) //less than 15 minutes before last alarm time
                    {
                        return 1;
                    }
                }
                else if((LiveIdCredentials.Last_Sleep_Segment.sleepType == "RestfulSleep"))
                {
                    if (DateTime.Now.AddMinutes(30).CompareTo(last_alarm_time) == 1) //less than 30 minutes before last alarm time
                    {
                        return 1;
                    }
                }
            }
            else if(alarmType == (int)AlarmType.CreativeSleep)
            {
                if (DateTime.Now.AddMinutes(-90).CompareTo(LiveIdCredentials.SleepStart) == 1)  //more than 90 minutes after the user fall asleep
                {
                    if (LiveIdCredentials.Last_Sleep_Segment.sleepType == "RestfulSleep")
                    {
                        if (DateTime.Now.AddMinutes(-40).CompareTo(LiveIdCredentials.Last_Sleep_Segment.endTime) == 1)//more than 40 minutes from the end of the last segment
                        {
                            return 1;
                        }
                        else
                        {
                            return 2;
                        }
                    }
                }
            }
            return 0;
        }

        /*
         *@param  exp_sleep_start_time  - when the user started the alarm (just before going to sleep)
         */
        public static async Task CheckToWakeUpAsync(object sender, EventArgs e, int alarmType, DateTime last_alarm_time, DateTime exp_sleep_start_time, CancellationTokenSource tokenSource
            ,CancellationTokenSource token_for_logic,Label txt)
        {
            int logic_response;
            if ((alarmType == (int)AlarmType.SmartAlarm) || (alarmType == (int)AlarmType.CombatNap))
            {

                //wait to last_alarm_time.AddMinutes(-34)
                TimeSpan time_to_wait = ((last_alarm_time.TimeOfDay - DateTime.Now.TimeOfDay) - new TimeSpan(0, 34, 0));
                //TOFO check if time to wait should be in next day if yes add 24
                if (time_to_wait.CompareTo(TimeSpan.Zero) >0)
                {
                    await Task.Delay(time_to_wait,token_for_logic.Token);
                }
                //else return; //todo - start alarm outisde?               

                //sync band with the cloud - takes 4 minutes(Omer)
                await Utils.SyncBandWithCloud(token_for_logic);

                //optional - wait and check if the band was synced(Grisha)
                await getLastSleepActivity(sender, e, exp_sleep_start_time);
                 logic_response = WakeUpLogic(alarmType, last_alarm_time);
                 if (logic_response == 1)
                 {
                    //wake up now (Kazula)
                    Utils.WakeUpNow(tokenSource,token_for_logic,txt);
                    return;
                 }
                 else if(logic_response == 2)
                 {
                    //awake at - Last_Sleep_Segment.endTime + 15 minutees (Kazula)
                    time_to_wait = ((LiveIdCredentials.Last_Sleep_Segment.endTime.AddMinutes(15).TimeOfDay - DateTime.Now.TimeOfDay));
                    if (time_to_wait.CompareTo(TimeSpan.Zero) >0)
                    {
                        await Task.Delay(time_to_wait,token_for_logic.Token);
                    }
                    Utils.WakeUpNow(tokenSource,token_for_logic,txt);
                    return;
                }
                else
                 {
                    //not waiting - the sync with the band takes 4 minutes
                    await CheckToWakeUpAsync(sender, e, alarmType, last_alarm_time, exp_sleep_start_time, tokenSource,token_for_logic,txt);
                 }
            }
            else if(alarmType == (int)AlarmType.CreativeSleep)
            {
                //wait to exp_sleep_start_time + 90 minutes (Kazula)
                //todo - change to 90 minutes from asleep
                TimeSpan time_to_wait = ((exp_sleep_start_time.AddMinutes(90).TimeOfDay - DateTime.Now.TimeOfDay));
                if (time_to_wait.CompareTo(TimeSpan.Zero) >0)
                {
                    await Task.Delay(time_to_wait,token_for_logic.Token);
                }

                await getLastSleepActivity(sender, e, exp_sleep_start_time);
                logic_response = WakeUpLogic(alarmType, last_alarm_time);
                if (logic_response == 1)
                {
                    //wake up now (Kazula)
                    Utils.WakeUpNow(tokenSource,token_for_logic,txt);
                    return;
                }
                else if(logic_response == 2)
                {
                    //sync band with the cloud - takes 4 minutes(Omer)
                    await Utils.SyncBandWithCloud(token_for_logic);
                    //optional - wait and check if the band was synced(Grisha)
                    await CheckToWakeUpAsync(sender, e, alarmType, last_alarm_time, exp_sleep_start_time, tokenSource,token_for_logic,txt);
                }
                else
                {
                    //wait 30 minutes (Kazula)
                    await Task.Delay(new TimeSpan(0,30,0),token_for_logic.Token);
                    await CheckToWakeUpAsync(sender, e, alarmType, last_alarm_time, exp_sleep_start_time, tokenSource,token_for_logic,txt);
                }
            }
        }

        //@return expected stage and how much time before end of simulation (last time to wake up) the last segment started
        //@param findREM: if findREM==creative sleep, stops when find REM stage and returns how much time needed to wait
        public static Tuple<int, TimeSpan> SimulateExpectedStage(TimeSpan timeToSimulateForward, int initialSegment, int findREM)
        {
            TimeSpan timeThatSimulated = new TimeSpan(0, 0, 0);
            TimeSpan timeThatSimulatedTemp = new TimeSpan(0, 0, 0); 
            int currentSegment = initialSegment;
            int nextSegment = currentSegment;            
            while (TimeSpan.Compare(timeToSimulateForward, timeThatSimulatedTemp)>=0)  //timeThatSimulated < timeToSimulateForward
            {
                currentSegment = nextSegment;
                timeThatSimulated = timeThatSimulatedTemp;
                switch (currentSegment)
                {
                    case (int)SegmentStage.Awake:
                        timeThatSimulatedTemp = timeThatSimulatedTemp.Add(new TimeSpan(0, SegmentSummaryTable.Awake.avgDuration(), 0));
                        nextSegment = SegmentSummaryTable.Awake.maxChanceToNextStage();
                        break;
                    case (int)SegmentStage.Doze:
                        timeThatSimulatedTemp = timeThatSimulatedTemp.Add(new TimeSpan(0, SegmentSummaryTable.Doze.avgDuration(), 0));
                        nextSegment = SegmentSummaryTable.Doze.maxChanceToNextStage();
                        break;
                    case (int)SegmentStage.Snooze:
                        timeThatSimulatedTemp = timeThatSimulatedTemp.Add(new TimeSpan(0, SegmentSummaryTable.Snooze.avgDuration(), 0));
                        nextSegment = SegmentSummaryTable.Snooze.maxChanceToNextStage();
                        break;
                    case (int)SegmentStage.RestlessSleep:
                        timeThatSimulatedTemp = timeThatSimulatedTemp.Add(new TimeSpan(0, SegmentSummaryTable.RestlessSleep.avgDuration(), 0));
                        nextSegment = SegmentSummaryTable.RestlessSleep.maxChanceToNextStage();
                        break;
                    case (int)SegmentStage.REM:
                        if (findREM == (int)AlarmType.CreativeSleep)
                        {
                            return Tuple.Create((int)SegmentStage.REM, timeThatSimulated);
                        }
                        timeThatSimulatedTemp = timeThatSimulatedTemp.Add(new TimeSpan(0, SegmentSummaryTable.REMSleep.avgDuration(), 0));
                        nextSegment = SegmentSummaryTable.REMSleep.maxChanceToNextStage();
                        break;
                    case (int)SegmentStage.RestfulSleep:
                        timeThatSimulatedTemp = timeThatSimulatedTemp.Add(new TimeSpan(0, SegmentSummaryTable.RestfulSleep.avgDuration(), 0));
                        nextSegment = SegmentSummaryTable.RestfulSleep.maxChanceToNextStage();
                        break;
                }                
            }
            return Tuple.Create(currentSegment, timeToSimulateForward.Subtract(timeThatSimulated));//tuple2 - how much time before end of simulation (last time to wake up) the last segment started
        }

        //smart alarm and combat nap:
        //first time to fall asleep - nowTime

        //simulating awake time:
        //simulate stage and its time that will be when alarm set based on asleep time              
        //if the stage is restfulsleep - set alarm to 5 minutes before the stage if is 30 min before alarm                
        //check with sdk(accelometr) if asleep every 60 min (10 minutes for combat nao). and get new falled asleep time (maybe after awake in the middle of the night) without doze                   
        //if got new time, back to "simulating awake time"

        //Creative sleep
        //first time to fall asleep - nowTime

        //simulating awake time:
        //simulate stage until get REM and wake up then
        //check with sdk(accelometr) if asleep every 60 min. and get first falled asleep time (maybe after awake in the middle of the night) without doze                   
        //if got new time, back to "simulating awake time"
        public static async Task CheckToWakeUpAsync2(object sender, EventArgs e, int alarmType, DateTime last_alarm_time, DateTime exp_sleep_start_time, CancellationTokenSource tokenSource
            , CancellationTokenSource token_for_logic, Label txt)
        {
            int expectedSegment = 0;
            TimeSpan segmantTime; //last segment time actualy
            TimeSpan minutes0 = new TimeSpan(0, 0, 0);
            TimeSpan minutes10 = new TimeSpan(0, 10, 0);
            TimeSpan minutes20 = new TimeSpan(0, 20, 0);
            TimeSpan minutes30 = new TimeSpan(0, 30, 0);
            TimeSpan minutes60 = new TimeSpan(0, 60, 0);
            TimeSpan minutes70 = new TimeSpan(1, 10, 0);
            TimeSpan tempTime;
            DateTime newAlarmTime = last_alarm_time;
            bool first_sleep_searching = true;
            int real_sleep_response;
            int simulator_segment_start = (int)SegmentStage.Awake;
                while (true)
                {                
                    (expectedSegment, segmantTime) = SimulateExpectedStage(last_alarm_time.Subtract(exp_sleep_start_time), simulator_segment_start, alarmType);
                    if ((alarmType != (int)AlarmType.CreativeSleep) && (expectedSegment == (int)SegmentStage.RestfulSleep))
                    {
                        if (TimeSpan.Compare(minutes30, segmantTime) > 0) //segment time is less then 30 minutes
                        {
                            segmantTime = segmantTime.Add(new TimeSpan(0, 5, 0)); //adding 5 minutes to be sure that awakment will be before starting restful segment
                            newAlarmTime = last_alarm_time.Subtract(segmantTime);
                        }
                    }
                    else if ((alarmType == (int)AlarmType.CreativeSleep) && (expectedSegment == (int)SegmentStage.REM))
                    {
                        newAlarmTime = exp_sleep_start_time.Add(segmantTime); //segment time in this case means how much time to wait before REM stage starts
                    }
                    
                    if (TimeSpan.Compare(minutes0, newAlarmTime.Subtract(DateTime.Now)) == 1) //edge case that we passed the time
                    {
                        tempTime = minutes0;
                    }
                    else
                    {
                        tempTime = newAlarmTime.Subtract(DateTime.Now); //time to wait before waking up
                    }

                    if ((((TimeSpan.Compare(minutes70, tempTime)) == 1) && (alarmType != (int)AlarmType.CombatNap)) //for smartAlarm and creative sleep - don't check less then 70 minutes
                       || (((TimeSpan.Compare(minutes20, tempTime)) == 1) && (alarmType == (int)AlarmType.CombatNap))){ //for combatNap
                        await Task.Delay(tempTime, token_for_logic.Token);
                        Utils.WakeUpNow(tokenSource, token_for_logic,txt);
                        break;
                    }
                    else
                    {
                        if (first_sleep_searching == false)
                        {
                            if (alarmType == (int)AlarmType.CombatNap)
                            {
                                    await Task.Delay(minutes10, token_for_logic.Token);
                            }
                            else//smart alarm / creative sleep
                            {
                                await Task.Delay(minutes60, token_for_logic.Token);
                            }
                        }
                    
                        real_sleep_response = await checkMovement.CheckSleepness(first_sleep_searching, (last_alarm_time.Subtract(DateTime.Now)).Minutes - 10);//tempTime.Minutes-10  - dont check after last time to awake
                        //takes checkMovement.timeSpent minutes
                        
                        if (first_sleep_searching)
                        {
                            if (real_sleep_response == 1)
                            {
                                first_sleep_searching = false;
                                exp_sleep_start_time = checkMovement.asleepTime;
                                simulator_segment_start = (int)SegmentStage.Doze;
                            }
                            else { }//do nothing. probably will awake at regular last_awake_time
                        }
                        else if (real_sleep_response == 0)//was sleeping but awake now
                        {
                          first_sleep_searching = true;
                          exp_sleep_start_time = DateTime.Now;
                          simulator_segment_start = (int)SegmentStage.Awake;
                        }
                        else { }//still sleeping. do nothing

                    }
                }
        }
    }
}