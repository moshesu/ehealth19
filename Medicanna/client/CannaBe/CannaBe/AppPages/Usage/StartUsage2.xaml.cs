using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using CannaBe;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using System.Diagnostics;

namespace CannaBe.AppPages.Usage
{
    public sealed partial class StartUsage2 : Page
    {
        DispatcherTimer Timer = new DispatcherTimer();
        bool isPaired = false;

        public StartUsage2()
        {
            this.InitializeComponent();
            PagesUtilities.AddBackButtonHandler();
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 1, 0); //so time would be updated
        }

        private void Timer_Tick(object sender, object e)
        {
            StartTime.Text = "Start Time: " + DateTime.Now.ToString("dd.MM.yy HH:mm");
        }

        private void OnPageLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (UsageContext.ChosenStrain != null)
            {
                StrainChosenText.Text = UsageContext.ChosenStrain.Name;
                Timer.Start();
                Timer_Tick(sender, e); //current time, before tick
            }
            else
            {
                StrainChosenText.Text = "Error: No strain chosen!";
            }
        }

        private void GoBack(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(StartUsage));
        }

        private void UseBandChanged(object sender, RoutedEventArgs e)
        {
            var b = sender as CheckBox;

            if (b.IsChecked.Value)
            { // Connect band
                PairBandButton.IsEnabled = true;
                if (isPaired && GlobalContext.Band.IsConnected())
                {
                    ContinueButton.IsEnabled = true;
                }
                else
                {
                    ContinueButton.IsEnabled = false;
                }
            }
            else
            { // No band
                PairBandButton.IsEnabled = false;
                ContinueButton.IsEnabled = true;
            }
        }


        private async void PairButtonClicked(object sender, RoutedEventArgs e)
        {
            StartAction();
            do
            { // Pair band
                ContinueButton.IsEnabled = false;

                var isSupported = await BandContext.GetBluetoothIsEnabledAsync();

                if (!isSupported)
                { // Bluetooth error
                    EndAction();

                    await new MessageDialog("Please enable BlueTooth and pair phone with Band", "Error!").ShowAsync();
                    break;
                }

                GlobalContext.Band = new BandContext();
                try
                { // Connect to band
                    isPaired = await GlobalContext.Band.PairBand().TimeoutAfter(new TimeSpan(0, 0, 15));
                    EndAction();

                    if (isPaired)
                    { // Connection successfull
                        PairBandButton.Content = "Paired Successfully!";
                        PairBandButton.Width = double.NaN;
                        PairBandButton.Foreground = new SolidColorBrush(Colors.Black);
                        PairBandButton.IsEnabled = false;
                    }
                    else
                    {
                        await new MessageDialog("Did not find band\nPlease try again", "Pairing Failed!").ShowAsync();
                    }
                }
                catch (TimeoutException)
                {
                    EndAction();
                    await new MessageDialog("Pairing timed out after 15 seconds", "Pairing Failed!").ShowAsync();
                }
            } while (false);
        }

        private void StartAction()
        {
            progressRing.IsActive = true;
            ContinueButton.IsEnabled = false;
        }

        private void EndAction()
        {
            progressRing.IsActive = false;
            ContinueButton.IsEnabled = true;
        }

        private async void StartSession(object sender, RoutedEventArgs e)
        {
            StartAction();

            bool useBand = UseBand.IsChecked.Value && isPaired && GlobalContext.Band.IsConnected();

            // Enter usage details
            UsageContext.Usage = new UsageData(UsageContext.ChosenStrain, DateTime.Now, useBand);

            if (!useBand)
            {
                Frame.Navigate(typeof(ActiveSession));
            }
            else // Use band, start acquiring heart rate
            {
                Acquiring.Visibility = Visibility.Visible;
                bool res = false;
                try
                { // Get heart rate
                    res = await Task.Run(() =>
                    GlobalContext.Band.StartHeartRate(UsageContext.Usage.HeartRateChangedAsync));
                }
                catch (Exception x)
                {
                    AppDebug.Exception(x, "StartSession => StartHeartRate");
                }

                Acquiring.Visibility = Visibility.Collapsed;

                EndAction();
                if (res)
                { // Continue to active session with correct parameters
                    ContinueButton.Content = "Success!";
                    PagesUtilities.SleepSeconds(1);
                    Frame.Navigate(typeof(ActiveSession));
                }
                else
                {
                    await new MessageDialog("Try reconnecting the band, re-pairing and wear the band", "Failed!").ShowAsync();
                    return;
                }

            }
        }
    }
}
;