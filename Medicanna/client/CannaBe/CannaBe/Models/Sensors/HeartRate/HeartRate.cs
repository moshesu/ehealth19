using Microsoft.Band;
using Microsoft.Band.Sensors;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace CannaBe
{

    public class HeartRateModel : ViewModel
    {
        public delegate void ChangedHandler(int heartRate, double quality);
        public event ChangedHandler Changed;
        public static int peak = 0;
        public static double sum = 0;
        public static int count = 0;

        public async Task InitAsync()
        {
            AppDebug.Line("HeartRateModel Starting...");

            if (BandModel.IsConnected)
            {
                AppDebug.Line("HeartRateModel Is connected...");

                var consent = UserConsent.Declined;
                try
                {
                    consent = BandModel.BandClient.SensorManager.HeartRate.GetCurrentUserConsent();
                }
                catch (Exception x)
                {
                    AppDebug.Exception(x, "InitAsync");
                }

                if (consent != UserConsent.Granted)
                {
                    AppDebug.Line("HeartRateModel requesting user access");
                    try
                    {
                        AppDebug.Line("HeartRateModel inside CoreDispatcher");

                        var r = await BandModel.BandClient.SensorManager.HeartRate.RequestUserConsentAsync();

                        AppDebug.Line($"HeartRateModel RequestUserConsentAsync returned {r}");

                    }
                    catch (Exception x)
                    {
                        AppDebug.Exception(x, "HeartRate.RequestUserConsentAsync");
                    }
                    AppDebug.Line("HeartRateModel user access success");
                }

                BandModel.BandClient.SensorManager.HeartRate.ReadingChanged += HeartRate_ReadingChanged;
                AppDebug.Line("HeartRateModel InitAsync");
            }
        }

        public async Task<bool> Start()
        {
            bool ret = false;
            try
            {
                AppDebug.Line("HeartRateModel Start");

                if (BandModel.IsConnected)
                {
                    AppDebug.Line("HeartRateModel reading..");

                    ret = await BandModel.BandClient.SensorManager.HeartRate.StartReadingsAsync();

                    AppDebug.Line("HeartRateModel read..");
                }
            }
            catch (Exception x)
            {
                AppDebug.Exception(x, "HeartRateModel.Start");
            }

            return ret;
        }

        public void Stop()
        {
            try
            {
                if (BandModel.IsConnected)
                {
                    BandModel.BandClient.SensorManager.HeartRate.StopReadingsAsync().GetAwaiter().GetResult();
                }
            }
            catch (Exception x)
            {
                AppDebug.Exception(x, "HeartRateModel.Stop");
            }
        }

        private void HeartRate_ReadingChanged(object sender, BandSensorReadingEventArgs<IBandHeartRateReading> e)
        {
            HeartRateSensorReading reading = new HeartRateSensorReading
            {
                HeartRate = e.SensorReading.HeartRate,
                Quality = e.SensorReading.Quality
            };
            Changed?.Invoke(reading.Value, reading.Accuracy);
        }
    }
}
