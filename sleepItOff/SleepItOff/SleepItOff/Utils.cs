using Newtonsoft.Json;
using SleepItOff.Cloud.AzureDatabase;
using SleepItOff.Entities;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace SleepItOff
{
    class Utils
    {
        public static String song = "alarm.mp3";

        public static void WakeUpNow(CancellationTokenSource tokenSource, CancellationTokenSource checkTok,Label txt)
        {
            if (checkTok.IsCancellationRequested)
                return;
            if (tokenSource != null)
                tokenSource.Cancel();
            txt.Text = "no current alarm";
            playSong(true);
        }
        public static void playSong(bool check=false)
        {
            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load(song);
            player.Play();
            displayPop(player,check);
        }
        public static void songSelection(Picker picker)
        {
            switch (picker.SelectedIndex)
            {
                case (0):
                    Utils.song = "alarm.mp3";
                    break;
                case (1):
                    Utils.song = "annoying.mp3";
                    break;
                case (2):
                    Utils.song = "Neon.ogg";
                    break;
                case (3):
                    Utils.song = "Awaken.ogg";
                    break;
                case (4):
                    Utils.song = "fire.mp3";
                    break;
            }
            Utils.playSong();
        }
        private static async void displayPop(Plugin.SimpleAudioPlayer.ISimpleAudioPlayer player,bool check)
        {
            if (check)
            {
                await App.Current.MainPage.DisplayAlert("Wake up", "good morning", "OK");
            }
            else
            {
               await Task.Delay(new TimeSpan(0, 0, 1));
            }
            player.Stop();
        }
        public static async void buttonClick(object sender, EventArgs e, TimePicker picker, Label txt, CancellationTokenSource tokenSource
        , CancellationTokenSource token_for_logic,int type)
        {
            txt.Text = "alarm at: "+picker.Time.ToString();

            TimeSpan timeToWait = picker.Time - DateTime.Now.TimeOfDay;

            if (timeToWait.CompareTo(TimeSpan.Zero) < 0)
            {
                var temp = new TimeSpan(24, 0, 0);  // next day 
                timeToWait = temp + timeToWait;
            }
            //SetTimer(timeToWait, tokenSource, token_for_logic,txt);
            //LoadingLabelXaml.CheckToWakeUpAsync(sender, e, type, DateTime.Now.Add(timeToWait), DateTime.Now, tokenSource, token_for_logic,txt);
            try
            {
                await LoadingLabelXaml.CheckToWakeUpAsync2(sender, e, type, DateTime.Now.Add(timeToWait), DateTime.Now, tokenSource, token_for_logic, txt);
            }
            catch (Exception exp)
            {

            }

        }
        public static async Task SetTimer(TimeSpan timeToWait, CancellationTokenSource token, CancellationTokenSource token_for_logic,Label txt)
        {
            try
            {
                await Task.Delay(timeToWait, token.Token);  
            }
            catch
            {

            }
            Utils.WakeUpNow(token_for_logic, token,txt);

        }
        public static void cancel(CancellationTokenSource tokenSource, CancellationTokenSource token_for_logic)
        {
            if (tokenSource != null)
                tokenSource.Cancel();
            if (token_for_logic != null)
                token_for_logic.Cancel();
        }

        public static void updateSegmentSummaryTableFromDB(UserSleepSegmentsStatsRecord record)
        {
            SegmentSummaryTable.userID = record.userId;
            SegmentSummaryTable.lastUpdated = record.lastUpdated;

            SegmentSummaryTable.Awake.countTimes = record.awakeCountTimes;
            SegmentSummaryTable.Awake.totalDuration = record.awakeTotalDuration;
            SegmentSummaryTable.Awake.timesToAwake = record.awakeToAwakeCount;
            SegmentSummaryTable.Awake.timesToSnooze = record.awakeToSnoozeCount;
            SegmentSummaryTable.Awake.timesToDoze = record.awakeToDozeCount;
            SegmentSummaryTable.Awake.timesToRestlessSleep = record.awakeToRestlessSleepCount;
            SegmentSummaryTable.Awake.timesToRestfulSleep = record.awakeToRestfulSleepCount;
            SegmentSummaryTable.Awake.timesToREM = record.awakeToREMCount;
            SegmentSummaryTable.Snooze.countTimes = record.snoozeCountTimes;
            SegmentSummaryTable.Snooze.totalDuration = record.snoozeTotalDuration;
            SegmentSummaryTable.Snooze.timesToAwake = record.snoozeToAwakeCount;
            SegmentSummaryTable.Snooze.timesToSnooze = record.snoozeToSnoozeCount;
            SegmentSummaryTable.Snooze.timesToDoze = record.snoozeToDozeCount;
            SegmentSummaryTable.Snooze.timesToRestlessSleep = record.snoozeToRestlessSleepCount;
            SegmentSummaryTable.Snooze.timesToRestfulSleep = record.snoozeToRestfulSleepCount;
            SegmentSummaryTable.Snooze.timesToREM = record.snoozeToREMCount;
            SegmentSummaryTable.Doze.countTimes = record.dozeCountTimes;
            SegmentSummaryTable.Doze.totalDuration = record.dozeTotalDuration;
            SegmentSummaryTable.Doze.timesToAwake = record.dozeToAwakeCount;
            SegmentSummaryTable.Doze.timesToSnooze = record.dozeToSnoozeCount;
            SegmentSummaryTable.Doze.timesToDoze = record.dozeToDozeCount;
            SegmentSummaryTable.Doze.timesToRestlessSleep = record.dozeToRestlessSleepCount;
            SegmentSummaryTable.Doze.timesToRestfulSleep = record.dozeToRestfulSleepCount;
            SegmentSummaryTable.Doze.timesToREM = record.dozeToREMCount;
            SegmentSummaryTable.RestlessSleep.countTimes = record.restlessSleepCountTimes;
            SegmentSummaryTable.RestlessSleep.totalDuration = record.restlessSleepTotalDuration;
            SegmentSummaryTable.RestlessSleep.timesToAwake = record.restlessSleepToAwakeCount;
            SegmentSummaryTable.RestlessSleep.timesToSnooze = record.restlessSleepToSnoozeCount;
            SegmentSummaryTable.RestlessSleep.timesToDoze = record.restlessSleepToDozeCount;
            SegmentSummaryTable.RestlessSleep.timesToRestlessSleep = record.restlessSleepToRestlessSleepCount;
            SegmentSummaryTable.RestlessSleep.timesToRestfulSleep = record.restlessSleepToRestfulSleepCount;
            SegmentSummaryTable.RestlessSleep.timesToREM = record.restlessSleepToREMCount;
            SegmentSummaryTable.RestfulSleep.countTimes = record.restfulSleepCountTimes;
            SegmentSummaryTable.RestfulSleep.totalDuration = record.restfulSleepTotalDuration;
            SegmentSummaryTable.RestfulSleep.timesToAwake = record.restfulSleepToAwakeCount;
            SegmentSummaryTable.RestfulSleep.timesToSnooze = record.restfulSleepToSnoozeCount;
            SegmentSummaryTable.RestfulSleep.timesToDoze = record.restfulSleepToDozeCount;
            SegmentSummaryTable.RestfulSleep.timesToRestlessSleep = record.restfulSleepToRestlessSleepCount;
            SegmentSummaryTable.RestfulSleep.timesToRestfulSleep = record.restfulSleepToRestfulSleepCount;
            SegmentSummaryTable.RestfulSleep.timesToREM = record.restfulSleepToREMCount;
            SegmentSummaryTable.REMSleep.countTimes = record.REMSleepCountTimes;
            SegmentSummaryTable.REMSleep.totalDuration = record.REMSleepTotalDuration;
            SegmentSummaryTable.REMSleep.timesToAwake = record.REMSleepToAwakeCount;
            SegmentSummaryTable.REMSleep.timesToSnooze = record.REMSleepToSnoozeCount;
            SegmentSummaryTable.REMSleep.timesToDoze = record.REMSleepToDozeCount;
            SegmentSummaryTable.REMSleep.timesToRestlessSleep = record.REMSleepToRestlessSleepCount;
            SegmentSummaryTable.REMSleep.timesToRestfulSleep = record.REMSleepToRestfulSleepCount;
            SegmentSummaryTable.REMSleep.timesToREM = record.REMSleepToREMCount;
        }

        //mission '0' = insert row to table
        //mission '1' = update row in table
        public static bool handleUserSleepSegmentsData(SleepSegmentsStatsRepository repo, string UserId, int mission)
        {
            if (mission == 0)
            {
                repo.SaveUserSleepSegmentsStats(UserId, SegmentSummaryTable.lastUpdated,
                                             SegmentSummaryTable.Awake.countTimes, SegmentSummaryTable.Awake.totalDuration,
                                             SegmentSummaryTable.Awake.timesToAwake, SegmentSummaryTable.Awake.timesToSnooze,
                                             SegmentSummaryTable.Awake.timesToDoze, SegmentSummaryTable.Awake.timesToRestlessSleep,
                                             SegmentSummaryTable.Awake.timesToRestfulSleep, SegmentSummaryTable.Awake.timesToREM,
                                             SegmentSummaryTable.Snooze.countTimes, SegmentSummaryTable.Snooze.totalDuration,
                                             SegmentSummaryTable.Snooze.timesToAwake, SegmentSummaryTable.Snooze.timesToSnooze,
                                             SegmentSummaryTable.Snooze.timesToDoze, SegmentSummaryTable.Snooze.timesToRestlessSleep,
                                             SegmentSummaryTable.Snooze.timesToRestfulSleep, SegmentSummaryTable.Snooze.timesToREM,
                                             SegmentSummaryTable.Doze.countTimes, SegmentSummaryTable.Doze.totalDuration,
                                             SegmentSummaryTable.Doze.timesToAwake, SegmentSummaryTable.Doze.timesToSnooze,
                                             SegmentSummaryTable.Doze.timesToDoze, SegmentSummaryTable.Doze.timesToRestlessSleep,
                                             SegmentSummaryTable.Doze.timesToRestfulSleep, SegmentSummaryTable.Doze.timesToREM,
                                             SegmentSummaryTable.RestlessSleep.countTimes, SegmentSummaryTable.RestlessSleep.totalDuration,
                                             SegmentSummaryTable.RestlessSleep.timesToAwake, SegmentSummaryTable.RestlessSleep.timesToSnooze,
                                             SegmentSummaryTable.RestlessSleep.timesToDoze, SegmentSummaryTable.RestlessSleep.timesToRestlessSleep,
                                             SegmentSummaryTable.RestlessSleep.timesToRestfulSleep, SegmentSummaryTable.RestlessSleep.timesToREM,
                                             SegmentSummaryTable.RestfulSleep.countTimes, SegmentSummaryTable.RestfulSleep.totalDuration,
                                             SegmentSummaryTable.RestfulSleep.timesToAwake, SegmentSummaryTable.RestfulSleep.timesToSnooze,
                                             SegmentSummaryTable.RestfulSleep.timesToDoze, SegmentSummaryTable.RestfulSleep.timesToRestlessSleep,
                                             SegmentSummaryTable.RestfulSleep.timesToRestfulSleep, SegmentSummaryTable.RestfulSleep.timesToREM,
                                             SegmentSummaryTable.REMSleep.countTimes, SegmentSummaryTable.REMSleep.totalDuration,
                                             SegmentSummaryTable.REMSleep.timesToAwake, SegmentSummaryTable.REMSleep.timesToSnooze,
                                             SegmentSummaryTable.REMSleep.timesToDoze, SegmentSummaryTable.REMSleep.timesToRestlessSleep,
                                             SegmentSummaryTable.REMSleep.timesToRestfulSleep, SegmentSummaryTable.REMSleep.timesToREM);
                return true;
            }
            else if (mission == 1)
            {
                repo.UpdateUserSleepSegmentsStats(UserId, SegmentSummaryTable.lastUpdated,
                                             SegmentSummaryTable.Awake.countTimes, SegmentSummaryTable.Awake.totalDuration,
                                             SegmentSummaryTable.Awake.timesToAwake, SegmentSummaryTable.Awake.timesToSnooze,
                                             SegmentSummaryTable.Awake.timesToDoze, SegmentSummaryTable.Awake.timesToRestlessSleep,
                                             SegmentSummaryTable.Awake.timesToRestfulSleep, SegmentSummaryTable.Awake.timesToREM,
                                             SegmentSummaryTable.Snooze.countTimes, SegmentSummaryTable.Snooze.totalDuration,
                                             SegmentSummaryTable.Snooze.timesToAwake, SegmentSummaryTable.Snooze.timesToSnooze,
                                             SegmentSummaryTable.Snooze.timesToDoze, SegmentSummaryTable.Snooze.timesToRestlessSleep,
                                             SegmentSummaryTable.Snooze.timesToRestfulSleep, SegmentSummaryTable.Snooze.timesToREM,
                                             SegmentSummaryTable.Doze.countTimes, SegmentSummaryTable.Doze.totalDuration,
                                             SegmentSummaryTable.Doze.timesToAwake, SegmentSummaryTable.Doze.timesToSnooze,
                                             SegmentSummaryTable.Doze.timesToDoze, SegmentSummaryTable.Doze.timesToRestlessSleep,
                                             SegmentSummaryTable.Doze.timesToRestfulSleep, SegmentSummaryTable.Doze.timesToREM,
                                             SegmentSummaryTable.RestlessSleep.countTimes, SegmentSummaryTable.RestlessSleep.totalDuration,
                                             SegmentSummaryTable.RestlessSleep.timesToAwake, SegmentSummaryTable.RestlessSleep.timesToSnooze,
                                             SegmentSummaryTable.RestlessSleep.timesToDoze, SegmentSummaryTable.RestlessSleep.timesToRestlessSleep,
                                             SegmentSummaryTable.RestlessSleep.timesToRestfulSleep, SegmentSummaryTable.RestlessSleep.timesToREM,
                                             SegmentSummaryTable.RestfulSleep.countTimes, SegmentSummaryTable.RestfulSleep.totalDuration,
                                             SegmentSummaryTable.RestfulSleep.timesToAwake, SegmentSummaryTable.RestfulSleep.timesToSnooze,
                                             SegmentSummaryTable.RestfulSleep.timesToDoze, SegmentSummaryTable.RestfulSleep.timesToRestlessSleep,
                                             SegmentSummaryTable.RestfulSleep.timesToRestfulSleep, SegmentSummaryTable.RestfulSleep.timesToREM,
                                             SegmentSummaryTable.REMSleep.countTimes, SegmentSummaryTable.REMSleep.totalDuration,
                                             SegmentSummaryTable.REMSleep.timesToAwake, SegmentSummaryTable.REMSleep.timesToSnooze,
                                             SegmentSummaryTable.REMSleep.timesToDoze, SegmentSummaryTable.REMSleep.timesToRestlessSleep,
                                             SegmentSummaryTable.REMSleep.timesToRestfulSleep, SegmentSummaryTable.REMSleep.timesToREM);
                return true;
            }
            else
            {
                //error
                return false;
            }
        }

        public static int calculateUserAge()
        {
            int userAge = DateTime.Today.Year - LiveIdCredentials.birthdate.Year;
            // Go back to the year the user was born in case of a leap year
            if (LiveIdCredentials.birthdate > DateTime.Today.AddYears(-userAge)) userAge--;
            return userAge;
        }

        public static async Task SyncBandWithCloud(CancellationTokenSource token_for_logic)
        {
            IBluetoothService ibs = DependencyService.Get<IBluetoothService>().CreateBluetoothService();
            int bt = ibs.EnableBluetooth();
            if (bt > 0) // if we actually turned on the bluetooth
            {
                await Task.Delay(90000, token_for_logic.Token);
            }
            else // if bluetooth was already turned on
            {
                ibs.DisableBluetooth();
                await Task.Delay(30000, token_for_logic.Token); // wait for 30 seconds
                bt = ibs.EnableBluetooth();
                await Task.Delay(90000, token_for_logic.Token); // wait for 90 seconds
            }
            ibs.DisableBluetooth();
            await Task.Delay(30000, token_for_logic.Token); // wait for 30 seconds
            bt = ibs.EnableBluetooth(); // enable bluetooth for the second time
            await Task.Delay(90000, token_for_logic.Token); // wait for 90 seconds
            ibs.DisableBluetooth(); // disable bluetooth at the end
            return;
        }

        public static async Task GetUserId(object sender, EventArgs e)
        {
            //create URL to send
            UriBuilder uri = new UriBuilder("https://api.microsofthealth.net/v1/me/Summaries/Daily");
            var query = new StringBuilder();
            uri.Query = query.ToString();

            //sending URL and waiting for response code
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri.Uri);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LiveIdCredentials.AccessToken);
            var httpResponse = httpClient.SendAsync(httpRequestMessage);

            //converting the response to JSON format
            var stringResponse = httpResponse.Result.Content.ReadAsStringAsync().Result;
            if (stringResponse != null)
            {
                SummaryResponse summaryResponse = JsonConvert.DeserializeObject<SummaryResponse>(stringResponse);
                if (summaryResponse != null && summaryResponse.summaries != null)
                {
                    LiveIdCredentials.userId = summaryResponse.summaries.First().userId;
                }
            }
        }

        public static async Task GetUserProfile(object sender, EventArgs e)
        {
            //create URL to send
            UriBuilder uri = new UriBuilder("https://api.microsofthealth.net/v1/me/Profile/");
            var query = new StringBuilder();
            uri.Query = query.ToString();

            //sending URL and waiting for response code
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri.Uri);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LiveIdCredentials.AccessToken);
            var httpResponse = httpClient.SendAsync(httpRequestMessage);

            //converting the response to JSON format
            var stringResponse = httpResponse.Result.Content.ReadAsStringAsync().Result;
            if (stringResponse != null)
            {
                ProfileResponse profileResponse = JsonConvert.DeserializeObject<ProfileResponse>(stringResponse);
                if (profileResponse != null)
                {
                    if (profileResponse.firstName != null) LiveIdCredentials.firstName = profileResponse.firstName;
                    if (profileResponse.lastName != null) LiveIdCredentials.lastName = profileResponse.lastName;
                    if (profileResponse.middleName != null) LiveIdCredentials.middleName = profileResponse.middleName;
                    if (profileResponse.gender != null) LiveIdCredentials.gender = profileResponse.gender;
                    LiveIdCredentials.height = (profileResponse.height / 10); //int cannot be null. height in mm->cm                    
                    LiveIdCredentials.weight = profileResponse.weight / 1000; //int cannot be null. weight in g->kg
                    if (profileResponse.birthdate != null) LiveIdCredentials.birthdate = profileResponse.birthdate;
                }
            }
        }

        public static async Task getSleepStatsOfUser(object sender, EventArgs e)
        {
            //create URL to send
            UriBuilder uri = new UriBuilder("https://api.microsofthealth.net/v1/me/Activities/");
            var query = new StringBuilder();
            query.AppendFormat("startTime={0}", Uri.EscapeDataString((new DateTime(1990, 1, 1)).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")));
            query.AppendFormat("&activityTypes={0}", Uri.EscapeDataString("sleep"));
            uri.Query = query.ToString();

            //sending URL and waiting for response code
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri.Uri);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LiveIdCredentials.AccessToken);
            var httpResponse = httpClient.SendAsync(httpRequestMessage);

            //converting the response to JSON format
            var stringResponse = httpResponse.Result.Content.ReadAsStringAsync().Result;
            if (stringResponse != null)
            {
                ActivityResponse activityResponse = JsonConvert.DeserializeObject<ActivityResponse>(stringResponse);
                if (activityResponse != null && activityResponse.sleepActivities != null)
                {
                    int count = 0;
                    int sumEff = 0;
                    int sumWA = 0;
                    foreach (SleepActivity sleepActivity in activityResponse.sleepActivities)
                    {
                        sumEff += sleepActivity.sleepEfficiencyPercentage;
                        StatisticsPage.sleepEfficiencyAcrossTime.Add(sleepActivity.sleepEfficiencyPercentage);
                        sumWA += sleepActivity.numberOfWakeups;
                        StatisticsPage.wakeUpsAcrossTime.Add(sleepActivity.numberOfWakeups);
                        count++;
                    }
                    if (count > 0)//prevent div by zero
                    {
                        LiveIdCredentials.mean_sleep_efficience = sumEff / count;
                        LiveIdCredentials.mean_num_of_wakeups = sumWA / count;
                    }
                }
            }
        }

        public static async Task getSleepSegmentsStats(object sender, EventArgs e, DateTime last_updated_time_of_segments_stats)
        {
            //create URL to send
            UriBuilder uri = new UriBuilder("https://api.microsofthealth.net/v1/me/Activities/");
            var query = new StringBuilder();
            query.AppendFormat("startTime={0}", Uri.EscapeDataString(last_updated_time_of_segments_stats.AddDays(1).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")));
            query.AppendFormat("&activityTypes={0}", Uri.EscapeDataString("sleep"));
            query.AppendFormat("&ActivityIncludes={0}", Uri.EscapeDataString("Details"));
            uri.Query = query.ToString();

            //sending URL and waiting for response code
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri.Uri);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LiveIdCredentials.AccessToken);
            var httpResponse = httpClient.SendAsync(httpRequestMessage);

            //converting the response to JSON format
            var stringResponse = httpResponse.Result.Content.ReadAsStringAsync().Result;
            if (stringResponse != null)
            {
                ActivityResponse activityResponse = JsonConvert.DeserializeObject<ActivityResponse>(stringResponse);
                if (activityResponse != null && activityResponse.sleepActivities != null)
                {
                    foreach (SleepActivity sleepActivity in activityResponse.sleepActivities)
                    {
                        if (minFromDur(sleepActivity.duration) > 30)//statistics from real sleep activities - 30+ minutes
                        {
                            for (int i = 0; i < sleepActivity.activitySegments.Count; i++)
                            {
                                int curStage = 0;
                                int nextStage = 0;
                                if (sleepActivity.activitySegments[i].segmentType == "Awake") curStage = (int)SegmentStage.Awake;
                                if (sleepActivity.activitySegments[i].segmentType == "Doze") curStage = (int)SegmentStage.Doze;
                                if (sleepActivity.activitySegments[i].segmentType == "Snooze") curStage = (int)SegmentStage.Snooze;
                                if (sleepActivity.activitySegments[i].segmentType == "Sleep")
                                {
                                    if (sleepActivity.activitySegments[i].segmentType == "Sleep")
                                    {
                                        if (sleepActivity.activitySegments[i].sleepType == "RestlessSleep") curStage = (int)SegmentStage.RestlessSleep;
                                        if (sleepActivity.activitySegments[i].sleepType == "RestfulSleep") curStage = (int)SegmentStage.RestfulSleep;
                                    }
                                }
                                if (i < (sleepActivity.activitySegments.Count - 1))
                                {
                                    if (sleepActivity.activitySegments[i + 1].segmentType == "Awake") nextStage = (int)SegmentStage.Awake;
                                    if (sleepActivity.activitySegments[i + 1].segmentType == "Doze") nextStage = (int)SegmentStage.Doze;
                                    if (sleepActivity.activitySegments[i + 1].segmentType == "Snooze") nextStage = (int)SegmentStage.Snooze;
                                    if (sleepActivity.activitySegments[i + 1].segmentType == "Sleep")
                                    {
                                        if (sleepActivity.activitySegments[i + 1].segmentType == "Sleep")
                                        {
                                            if (sleepActivity.activitySegments[i + 1].sleepType == "RestlessSleep") nextStage = (int)SegmentStage.RestlessSleep;
                                            if (sleepActivity.activitySegments[i + 1].sleepType == "RestfulSleep") nextStage = (int)SegmentStage.RestfulSleep;
                                        }
                                    }
                                }
                                switch (curStage)
                                {
                                    case (int)SegmentStage.Awake:
                                        SegmentSummaryTable.Awake.countTimes++;
                                        SegmentSummaryTable.Awake.totalDuration += minFromDur(sleepActivity.activitySegments[i].duration);
                                        increaseNextSegmentCounter(curStage, nextStage);
                                        break;
                                    case (int)SegmentStage.Doze:
                                        SegmentSummaryTable.Doze.countTimes++;
                                        SegmentSummaryTable.Doze.totalDuration += minFromDur(sleepActivity.activitySegments[i].duration);
                                        increaseNextSegmentCounter(curStage, nextStage);
                                        break;
                                    case (int)SegmentStage.Snooze:
                                        SegmentSummaryTable.Snooze.countTimes++;
                                        SegmentSummaryTable.Snooze.totalDuration += minFromDur(sleepActivity.activitySegments[i].duration);
                                        increaseNextSegmentCounter(curStage, nextStage);
                                        break;
                                    case (int)SegmentStage.RestlessSleep:
                                        int isREM = 0;
                                        //for REM sleep:
                                        //the segmented started later then after 90 minutes from falling asleep
                                        if (DateTime.Compare(sleepActivity.activitySegments[i].startTime, sleepActivity.fallAsleepTime.AddMinutes(90)) >= 0)
                                        {
                                            //if the segment is longer then 40 minutes - good possibility to REM sleep
                                            if (minFromDur(sleepActivity.activitySegments[i].duration) > 40)
                                            {
                                                isREM = 1;
                                                SegmentSummaryTable.RestlessSleep.countTimes++;
                                                SegmentSummaryTable.RestlessSleep.totalDuration += 40;
                                                SegmentSummaryTable.RestlessSleep.timesToREM++;
                                                SegmentSummaryTable.REMSleep.countTimes++;
                                                SegmentSummaryTable.REMSleep.totalDuration += minFromDur(sleepActivity.activitySegments[i].duration);
                                                increaseNextSegmentCounter((int)SegmentStage.REM, nextStage);
                                            }
                                        }
                                        if (isREM == 0)
                                        {
                                            SegmentSummaryTable.RestlessSleep.countTimes++;
                                            SegmentSummaryTable.RestlessSleep.totalDuration += minFromDur(sleepActivity.activitySegments[i].duration);
                                            increaseNextSegmentCounter(curStage, nextStage);
                                        }
                                        break;
                                    case (int)SegmentStage.RestfulSleep:
                                        SegmentSummaryTable.RestfulSleep.countTimes++;
                                        SegmentSummaryTable.RestfulSleep.totalDuration += minFromDur(sleepActivity.activitySegments[i].duration);
                                        increaseNextSegmentCounter(curStage, nextStage);
                                        break;
                                }
                            }
                        }
                    }
                    SegmentSummaryTable.userID = LiveIdCredentials.userId;
                    SegmentSummaryTable.lastUpdated = activityResponse.sleepActivities.Last().dayId;
                }
            }
        }

        private static void increaseNextSegmentCounter(int curStage, int nextStage)
        {
            switch (curStage)
            {
                case (int)SegmentStage.Awake:
                    switch (nextStage)
                    {
                        case (int)SegmentStage.Awake:
                            SegmentSummaryTable.Awake.timesToAwake++;
                            break;
                        case (int)SegmentStage.Doze:
                            SegmentSummaryTable.Awake.timesToDoze++;
                            break;
                        case (int)SegmentStage.Snooze:
                            SegmentSummaryTable.Awake.timesToSnooze++;
                            break;
                        case (int)SegmentStage.RestlessSleep:
                            SegmentSummaryTable.Awake.timesToRestlessSleep++;
                            break;
                        case (int)SegmentStage.RestfulSleep:
                            SegmentSummaryTable.Awake.timesToRestfulSleep++;
                            break;
                    }
                    break;
                case (int)SegmentStage.Doze:
                    switch (nextStage)
                    {
                        case (int)SegmentStage.Awake:
                            SegmentSummaryTable.Doze.timesToAwake++;
                            break;
                        case (int)SegmentStage.Doze:
                            SegmentSummaryTable.Doze.timesToDoze++;
                            break;
                        case (int)SegmentStage.Snooze:
                            SegmentSummaryTable.Doze.timesToSnooze++;
                            break;
                        case (int)SegmentStage.RestlessSleep:
                            SegmentSummaryTable.Doze.timesToRestlessSleep++;
                            break;
                        case (int)SegmentStage.RestfulSleep:
                            SegmentSummaryTable.Doze.timesToRestfulSleep++;
                            break;
                    }
                    break;
                case (int)SegmentStage.Snooze:
                    switch (nextStage)
                    {
                        case (int)SegmentStage.Awake:
                            SegmentSummaryTable.Snooze.timesToAwake++;
                            break;
                        case (int)SegmentStage.Doze:
                            SegmentSummaryTable.Snooze.timesToDoze++;
                            break;
                        case (int)SegmentStage.Snooze:
                            SegmentSummaryTable.Snooze.timesToSnooze++;
                            break;
                        case (int)SegmentStage.RestlessSleep:
                            SegmentSummaryTable.Snooze.timesToRestlessSleep++;
                            break;
                        case (int)SegmentStage.RestfulSleep:
                            SegmentSummaryTable.Snooze.timesToRestfulSleep++;
                            break;
                    }
                    break;
                case (int)SegmentStage.REM:
                    switch (nextStage)
                    {
                        case (int)SegmentStage.Awake:
                            SegmentSummaryTable.REMSleep.timesToAwake++;
                            break;
                        case (int)SegmentStage.Doze:
                            SegmentSummaryTable.REMSleep.timesToDoze++;
                            break;
                        case (int)SegmentStage.Snooze:
                            SegmentSummaryTable.REMSleep.timesToSnooze++;
                            break;
                        case (int)SegmentStage.RestlessSleep:
                            SegmentSummaryTable.REMSleep.timesToRestlessSleep++;
                            break;
                        case (int)SegmentStage.RestfulSleep:
                            SegmentSummaryTable.REMSleep.timesToRestfulSleep++;
                            break;
                    }
                    break;
                case (int)SegmentStage.RestlessSleep:
                    switch (nextStage)
                    {
                        case (int)SegmentStage.Awake:
                            SegmentSummaryTable.RestlessSleep.timesToAwake++;
                            break;
                        case (int)SegmentStage.Doze:
                            SegmentSummaryTable.RestlessSleep.timesToDoze++;
                            break;
                        case (int)SegmentStage.Snooze:
                            SegmentSummaryTable.RestlessSleep.timesToSnooze++;
                            break;
                        case (int)SegmentStage.RestlessSleep:
                            SegmentSummaryTable.RestlessSleep.timesToRestlessSleep++;
                            break;
                        case (int)SegmentStage.RestfulSleep:
                            SegmentSummaryTable.RestlessSleep.timesToRestfulSleep++;
                            break;
                    }
                    break;
                case (int)SegmentStage.RestfulSleep:
                    switch (nextStage)
                    {
                        case (int)SegmentStage.Awake:
                            SegmentSummaryTable.RestfulSleep.timesToAwake++;
                            break;
                        case (int)SegmentStage.Doze:
                            SegmentSummaryTable.RestfulSleep.timesToDoze++;
                            break;
                        case (int)SegmentStage.Snooze:
                            SegmentSummaryTable.RestfulSleep.timesToSnooze++;
                            break;
                        case (int)SegmentStage.RestlessSleep:
                            SegmentSummaryTable.RestfulSleep.timesToRestlessSleep++;
                            break;
                        case (int)SegmentStage.RestfulSleep:
                            SegmentSummaryTable.RestfulSleep.timesToRestfulSleep++;
                            break;
                    }
                    break;
            }
        }

        /*
        public static DateTime dateTimeFromIso8601(string time) //get int value of minutes from iso8601 duration string
        {
            return DateTime.Parse(time, null, System.Globalization.DateTimeStyles.RoundtripKind);
        }
        */

        public static int minFromDur(string duration) //get int value of minutes from iso8601 duration string
        {
            return (System.Xml.XmlConvert.ToTimeSpan(duration)).Hours * 60 + (System.Xml.XmlConvert.ToTimeSpan(duration)).Minutes;
        }

    }
}
