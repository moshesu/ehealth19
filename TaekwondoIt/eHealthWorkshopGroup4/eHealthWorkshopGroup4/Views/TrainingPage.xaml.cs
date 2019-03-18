/*
    Copyright (c) Microsoft Corporation All rights reserved.  
 
    MIT License: 
 
    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
    documentation files (the  "Software"), to deal in the Software without restriction, including without limitation
    the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
    and to permit persons to whom the Software is furnished to do so, subject to the following conditions: 
 
    The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software. 
 
    THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
    TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Band.Portable;
using Microsoft.Band.Portable.Sensors;
using Lottie.Forms;


namespace eHealthWorkshopGroup4.Views
{
    enum BandError: int
    {
        NO_BANDS =-1,CANT_MEASURE =-2, SUCCESS = 0
    }
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainingPage : ContentPage
    {
        //the actual band client
        //boolean describing the status of the current training
        private bool isTraining;
        private int readHR;
        private BandError error;
        // Button text
        Microsoft.Band.Portable.Sensors.BandHeartRateSensor HRsensor;
        //Property for showing training analisys
        public bool IsTraining
        {
            get
            {
                return isTraining;
            }
            set
            {
                isTraining = value;
                OnPropertyChanged();
            }
        }
        //Property for reading the HR
        public int ReadHR
        {
            get { return readHR; }
            set {
                readHR = value;
                //operation succeeded
                if (readHR >= 0)
                    error = BandError.SUCCESS;
                //an error occourred
                else {
                    //TODO best effort to make the proprty change (change caused by changind the value of error) effective.
                    ErrorOccurred = true;
                    error = (BandError)ReadHR;
                    //TODO switch that QualityLabel.Text with popup
                    //TODO message is not shown.
                    if (error == BandError.NO_BANDS)
                        ErrorAlert("There is no band connected to your phone. make sure to connect one and try again.");
                    else
                        ErrorAlert("Taekwondo It cannot measure from the band. please make sure to approve measurements from the band.");
                    IsTraining = false;
                }
                OnPropertyChanged();
            }
        }

        

        public async void ErrorAlert(string message)
        {
            await DisplayAlert("App message", message, "OK");
        }
        private HeartRateQuality quality;
        public HeartRateQuality Quality { get { return quality; } private set { quality = value; OnPropertyChanged(); } }

        //TODO when true should raise an error window with ErrorMessage <- DONE
        public bool ErrorOccurred
        {
            get
            {
                return error != BandError.SUCCESS;
            }
            set
            {
                if (value)
                {
                    OnPropertyChanged();
                }
            }

        }
        
        public TrainingPage()
        {
            InitializeComponent();
            this.IsTraining = false;
            HRLabel.FadeTo(0);
            QualityLabel.FadeTo(0);
            PoomsaeImg.FadeTo(0);
            HRsensor = null;
        }
        public async Task ConnectToBand()
        {

            var bandClientManager = BandClientManager.Instance;
            // query the service for paired devices
            var pairedBands = await bandClientManager.GetPairedBandsAsync();
            //TODO verify this is the correct condition
            if(pairedBands == null || !pairedBands.Any())
            {
                ReadHR = (int) BandError.NO_BANDS;
                return;
            }
            // connect to the first device
            var bandInfo = pairedBands.FirstOrDefault();
            if (bandInfo == null)
            {
                ReadHR = (int)BandError.NO_BANDS;
                return;
            }
            var band = await bandClientManager.ConnectAsync(bandInfo);
            var sensorManager = band.SensorManager;
            // get the heart rate sensor
            HRsensor = sensorManager.HeartRate;
            //TODO fetch more sensors?
            // add a handler
            HRsensor.ReadingChanged += (o, args) => {
                //TODO check need for quality
                Task.Run(() =>
                {
                    ReadHR = args.SensorReading.HeartRate;
                    Quality = args.SensorReading.Quality;
                });
                
            };
            // ask for user's consent
            if (HRsensor.UserConsented == UserConsent.Unspecified)
            {
                bool granted = await HRsensor.RequestUserConsent();
            }
            // gotten user consent
            if (HRsensor.UserConsented == UserConsent.Granted)
            {
                await HRsensor.StartReadingsAsync(BandSensorSampleRate.Ms16);

            }
            else
            {
                // user declined -error measure -1
                ReadHR = (int) BandError.CANT_MEASURE;
            }
        }
        public async void ChangeTrainingStatus(object sender, EventArgs e)
        {
            //TODO check if working

            IsTraining ^= true;
            if (IsTraining)
            {
                await ConnectToBand();// Can change IsTraining value;
                if (IsTraining)
                {
                    animationView.Play();
                    await Task.WhenAll(
                    animationView.ScaleTo(0, 400, Easing.BounceIn),
                    animationView.FadeTo(0, 700, Easing.SinInOut)
                    );

                    animationView.WidthRequest = 100;
                    animationView.HeightRequest = 100;
                    animationView.TranslationX = 0;

                    await Task.WhenAll(
                    animationView.ScaleTo(1, 0, Easing.BounceIn),
                    unFadeAll()
                    );
                }

            }
            else
            {

                //await HRsensor.StopReadingsAsync();
                
                await Task.WhenAll(
                    animationView.ScaleTo(0, 400, Easing.BounceIn),
                    FadeAll()
                    );

                animationView.WidthRequest = 300;
                animationView.HeightRequest = 300;
                animationView.TranslationX = 30;

                //IsTraining = false;

                await Task.WhenAll(

                animationView.ScaleTo(1, 400, Easing.BounceIn),
                animationView.FadeTo(1, 700, Easing.SinInOut)
                );
                animationView.Pause();

            }
        }

        private async Task FadeAll()
        {
            await Task.Run(() =>
                Task.WhenAll(
                    animationView.FadeTo(0, 700, Easing.SinInOut),
                    HRLabel.FadeTo(0, 700, Easing.SinInOut),
                    QualityLabel.FadeTo(0, 700, Easing.SinInOut),
                    PoomsaeImg.FadeTo(0, 700, Easing.SinInOut)
            ));

        }

        private async Task unFadeAll()
        {
            await Task.Run(() =>
                Task.WhenAll(
                    animationView.FadeTo(1, 700, Easing.SinInOut),
                    HRLabel.FadeTo(1, 700, Easing.SinInOut),
                    QualityLabel.FadeTo(1, 700, Easing.SinInOut),
                    PoomsaeImg.FadeTo(1, 700, Easing.SinInOut)
            ));
        }

    }
}