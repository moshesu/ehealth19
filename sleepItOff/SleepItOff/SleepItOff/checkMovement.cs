using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Band.Portable;
using Xamarin.Forms;
using System.Collections;
using System.Windows.Input;

namespace SleepItOff
{
    class checkMovement : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static BandDeviceInfo band;
        private static Sdk bandService;
        public static DateTime asleepTime;
        private bool isToggleAccelerometer;
        public bool IsToggleAccelerometer
        {
            get { return isToggleAccelerometer; }
            set
            {
                if (isToggleAccelerometer != value)
                {
                    isToggleAccelerometer = value;
                    OnPropertyChanged();
                    //ToggleAccelerometer();
                }
            }
        }
        private double readAccelerometerx;
        public double ReadAccelerometerx
        {
            get { return readAccelerometerx; }
            set
            {
                readAccelerometerx = value;
                OnPropertyChanged();
            }
        }
        private double readAccelerometery;
        public double ReadAccelerometery
        {
            get { return readAccelerometery; }
            set
            {
                readAccelerometery = value;
                OnPropertyChanged();
            }
        }
        private double readAccelerometerz;
        public double ReadAccelerometerz
        {
            get { return readAccelerometerz; }
            set
            {
                readAccelerometerz = value;
                OnPropertyChanged();
            }
        }
        public checkMovement()
        {
        //    isToggleAccelerometer = true;
        //    bandService = new Sdk();
        //    getBands();
        }
            private static async void getBands()
        {
            var bands = await bandService.getBands();
            band = bands.FirstOrDefault();
            if (band == null)
            {
                Console.WriteLine("failed");
                return;
            }
            Console.WriteLine(band.Name);

        }

        private static async void ConnectToBand()
        {
            if (band != null)
            {
                var result = await bandService.ConnectToBand(band);
                //bandService.PropertyChanged += BandService_PropertyChanged;
            }


        }

        private async void BandService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await Task.Run(() =>
            {
                //this.ReadAccelerometerx = bandService.CurrentAccelerometerx;
                //this.ReadAccelerometery = bandService.CurrentAccelerometery;
                //this.ReadAccelerometerz = bandService.CurrentAccelerometerz;
           
            });
        }

        public Command Connect
        {
            get
            {
                return new Command(() =>
                {
                    ConnectToBand();
                });
            }
        }

        public Command StopAllSensors
        {
            get
            {
                return new Command(() =>
                {
 //                   this.IsToggleAccelerometer = false; isToggleAccelerometer = false; ToggleAccelerometer();                
                });
            }
        }
        public Command getAccelerometer
        {
            get
            {
                return new Command(async () =>
                {
                    await CheckSleepness(true);//todo - get datetime result
                });
            }
        }
        public static async Task<int> CheckSleepness(bool first_time)
        {
            bandService = new Sdk();
            getBands();
            ConnectToBand();

            await Task.Delay(TimeSpan.FromSeconds(1));//make sure that the band is connected

            double AS = 0;
            List<int> motion_in_minutes = new List<int>();  
            while (first_time || (!first_time && motion_in_minutes.Count<7))
            {
                await CountMotionsInMinute();
                motion_in_minutes.Add(bandService.moveDetected);
                if (motion_in_minutes.Count > 7) motion_in_minutes.RemoveAt(0);
                if (motion_in_minutes.Count == 7)
                {
                    AS = 35*(0.0069*motion_in_minutes[0]//from research, with adaptation
                        +0.0112*motion_in_minutes[1]
                        +0.0118 * motion_in_minutes[2]
                        + 0.0158 * motion_in_minutes[3]
                        + 0.0472 * motion_in_minutes[4]
                        + 0.03 * motion_in_minutes[5]
                        + 0.0106 * motion_in_minutes[6]);
                    if (AS < 1) break;
                }
            }
            asleepTime = DateTime.Now.AddMinutes(-2);
            if (first_time)
            {
                return 1; // asleep for a first time
            }
            else
            {
                if (AS < 1) return 1; // still sleeping
                else return 0; //awake
            }

        }
        private static async Task CountMotionsInMinute()
        {
            bandService.moveDetected = 0;
            for (int i = 0; i < 30; i++)
            {
               await bandService.StartReadingGyroscope();                   
            }

        }

        public ICommand ClickCommand2 => new Command<string>((url) =>
        {
            Device.OpenUri(new System.Uri(url));
        });

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
