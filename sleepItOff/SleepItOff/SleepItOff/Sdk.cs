
using Microsoft.Band.Portable;
using Microsoft.Band;
using Microsoft.Band.Sensors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace SleepItOff
{
    class Sdk : INotifyPropertyChanged
    {
        private BandClient bandClient;
        private BandDeviceInfo band;
        private BandClientManager bandClientManager;
        public event PropertyChangedEventHandler PropertyChanged;
        List<double> gyroSamples = new List<double>();
        public int moveDetected;

        public Sdk()
        {
            bandClientManager = BandClientManager.Instance;
        }
        public async Task<IEnumerable<BandDeviceInfo>> getBands()
        {
            var bands = await bandClientManager.GetPairedBandsAsync();            

            return bands;
        }

        public async Task<String> ConnectToBand(BandDeviceInfo b)
        {
            band = b;
            bandClient = await bandClientManager.ConnectAsync(band);
            return String.Format("connected to {0} !", band.Name);
        }

        public async Task StartReadingGyroscope()
        {            
            gyroSamples.Clear();
            if (bandClient != null)
            {
                
                bandClient.SensorManager.Gyroscope.ReadingChanged += Gyroscope_ReadingChanged;
                await Task.Delay(TimeSpan.FromSeconds(0.1));
                await bandClient.SensorManager.Gyroscope.StartReadingsAsync();

                await Task.Delay(TimeSpan.FromSeconds(1.7));
                await StopReadingGyroscope();
                await Task.Delay(TimeSpan.FromSeconds(0.1));

            }

        }

        private async void Gyroscope_ReadingChanged(object sender, Microsoft.Band.Portable.Sensors.BandSensorReadingEventArgs<Microsoft.Band.Portable.Sensors.BandGyroscopeReading> e)
        {
            await Task.Run(() =>
            {
                gyroSamples.Add(e.SensorReading.AngularVelocityX);
                gyroSamples.Add(e.SensorReading.AngularVelocityY);
                gyroSamples.Add(e.SensorReading.AngularVelocityZ);
            });
        }
        public async Task StopReadingGyroscope()
        {
            await bandClient.SensorManager.Gyroscope.StopReadingsAsync();
            await Task.Delay(TimeSpan.FromSeconds(0.1));
            bandClient.SensorManager.Gyroscope.ReadingChanged -= Gyroscope_ReadingChanged;
            moveDetected += getMoves();
        }
        public int getMoves()
        {
            if (gyroSamples.Count > 8)
            {
                gyroSamples.RemoveRange(0, 3);//bug of gyro - first item is fake            
                if (gyroSamples.Max() > 5)
                {
                    gyroSamples.Sort();
                    gyroSamples.RemoveRange(0, 5); //bug of gyro - sometimes throws high value for 1 ms
                    gyroSamples.RemoveRange(gyroSamples.Count-5, 5); //negative values
                    if (gyroSamples.Max() > 5) return 1;
                    else return 0;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
