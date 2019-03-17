using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace CannaBe
{
    class UsageData : ViewModel
    {
        public delegate void HeartRateUpdateHandler(double avg, int min, int max);
        public Strain UsageStrain { get; private set; }
        private DateTime startTime;
        public Dictionary<string, string> usageFeedback;
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            private set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }
        public string StartTimeString
        {
            get
            {
                return StartTime.ToString("MMMM dd, yyyy HH:mm:ss");
            }
        }

        public DateTime EndTime { get; private set; }

        public string EndTimeString
        {
            get
            {
                return EndTime.ToString("MMMM dd, yyyy HH:mm:ss");
            }
        }

        private TimeSpan duration;
        public TimeSpan Duration
        {
            get
            {
                return duration;
            }
            private set
            {
                duration = value;
                OnPropertyChanged("Duration");
            }
        }
        public string DurationString
        { // Duration of usage
            get
            {
                StringBuilder str = new StringBuilder("");
                if (Duration.Hours > 0)
                {
                    str.Append($"{Duration.Hours} hours, ");
                }
                if (Duration.Minutes > 0)
                {
                    str.Append($"{Duration.Minutes} mins, ");
                }
                if (Duration.TotalSeconds >= 1)
                {
                    str.Append($"{Duration.Seconds} secs");
                }
                else
                {
                    str.Append($"{Duration.TotalMilliseconds} ms");
                }

                return str.ToString();
            }
        }

        private DispatcherTimer timer;

        public HeartRateUpdateHandler Handler = null;

        private bool usedBandData = false;
        public bool UseBandData
        {
            get
            {
                return usedBandData;
            }
            set
            {
                usedBandData = value;
                OnPropertyChanged("UseBandData");
            }
        }

        public Visibility ShowBandData
        {
            get
            {
                return usedBandData ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public string UsedBandString
        {
            get
            {
                return UseBandData ? "Yes" : "No";
            }
        }

        public UsageData(Strain usageStrain, DateTime startTime, bool _useBandData)
        {
            UsageStrain = usageStrain;
            StartTime = startTime;
            UseBandData = _useBandData;
            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            timer.Start();
        }

        private UsageData() { }

        public static implicit operator UsageData(UsageUpdateRequest res)
        { // Get total data for usage
            UsageData u;
            try
            {
                u = new UsageData
                {
                    UsageStrain = new Strain(res.StrainName, res.StrainId),
                    StartTime = DateTimeOffset.FromUnixTimeMilliseconds(res.unixStartTime).DateTime.ToLocalTime(),
                    EndTime = DateTimeOffset.FromUnixTimeMilliseconds(res.unixEndTime).DateTime.ToLocalTime(),
                    MedicalRank = res.MedicalRank,
                    PositiveRank = res.PositiveRank,
                    OverallRank = res.OverallRank,
                    HeartRateAverage = res.HeartbeatAvg,
                    HeartRateMax = res.HeartbeatHigh,
                    HeartRateMin = res.HeartbeatLow,
                    UseBandData = res.HeartbeatAvg > 0 ? true : false,
                    UsageId = res.UsageId
                };
                u.Duration = u.EndTime.Subtract(u.StartTime);
                u.usageFeedback = JsonConvert.DeserializeObject<Dictionary<string, string>>(res.QuestionsJson);
            }
            catch(Exception e)
            {
                AppDebug.Exception(e, "implicit operator UsageData(UsageUpdateRequest res)");
                return null;
            }
            return u;
        }

        public void AddTimerFunction(EventHandler<object> handler)
        {
            timer.Tick += handler;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is UsageData other))
            {
                return false;
            }

            return StartTime.Equals(other.StartTime) && EndTime.Equals(other.EndTime);
        }

        public override int GetHashCode()
        {
            return StartTime.GetHashCode();
        }

        public void EndUsage()
        { // End usage and save
            EndTime = DateTime.Now;
            timer.Stop();
            Duration = EndTime.Subtract(StartTime);
            HeartRateAverage = Math.Round(HeartRateAverage);
            GlobalContext.UpdateUsagesContextIfEmptyAsync();
            GlobalContext.CurrentUser.UsageSessions.Add(this);
        }

        private long HeartRateReadings { get; set; } = 0;
        public double HeartRateAverage { get; private set; } = 0;
        public int HeartRateMin { get; private set; } = int.MaxValue;
        public int HeartRateMax { get; private set; } = 0;

        public void HeartRateChangedAsync(int rate, double accuracy)
        { // Compute heart rate - min, max and average
            if (accuracy == 1)
            {
                HeartRateReadings++;

                if (HeartRateReadings < 5)
                    return;

                //from: math.stackexchange.com/questions/106700/incremental-averageing
                HeartRateAverage += (rate - HeartRateAverage) / (HeartRateReadings-4);

                HeartRateMin = Math.Min(HeartRateMin, rate);
                HeartRateMax = Math.Max(HeartRateMax, rate);

                //AppDebug.Line($"HeartRate: cur<{rate}> min<{HeartRateMin}> avg<{Math.Round(HeartRateAverage, 0)}> max<{HeartRateMax}>");

                if (HeartRateReadings % 5 == 0)
                {
                    AppDebug.Line($"HeartRate: cur<{rate}> min<{HeartRateMin}> avg<{Math.Round(HeartRateAverage, 0)}> max<{HeartRateMax}>");
                    try
                    {
                        //Run code on main thread for UI change, preventing exception
                        CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            try
                            {
                                Handler?.Invoke(HeartRateAverage, HeartRateMin, HeartRateMax);
                            }
                            catch (Exception x)
                            {
                                AppDebug.Exception(x, "HeartRateChangedAsync => Handler?.Invoke");
                            }
                        }).AsTask().GetAwaiter().GetResult();
                    }
                    catch (Exception e)
                    {
                        AppDebug.Exception(e, "HeartRateChanged");
                    }
                }
            }
        }

        public double MedicalRank { get; set; } = 0;
        public double PositiveRank { get; set; } = 0;
        public double OverallRank { get; set; } = 0;

        public string UsageId { get; set; } = "";
    }
}
