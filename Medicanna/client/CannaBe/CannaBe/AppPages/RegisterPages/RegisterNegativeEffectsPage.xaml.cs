using CannaBe.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace CannaBe
{
    public sealed partial class RegisterNegativeEffectsPage : Page
    {
        public RegisterNegativeEffectsPage()
        {
            this.InitializeComponent();
            this.FixPageSize();
            PagesUtilities.AddBackButtonHandler((object sender, Windows.UI.Core.BackRequestedEventArgs e) =>
            {
                BackToPositiveEffectsRegister(null, null);
            });
        }
        private void BoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBoxSender = sender as TextBox;

            if (textBoxSender.Text == ("Enter " + textBoxSender.Name))
            {
                textBoxSender.Text = "";
                textBoxSender.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Black);
            }
        }

        private void BoxLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBoxSender = sender as TextBox;

            if (textBoxSender.Text.Length == 0)
            {
                textBoxSender.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.White);
                textBoxSender.Text = "Enter " + textBoxSender.Name;

            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var req = GlobalContext.RegisterContext;

            if (req != null)
            {
                try
                { // If checkboxes were chosen before, update them again
                    PagesUtilities.SetAllCheckBoxesTags(RegisterNegativeEffectsGrid,
                                     req.IntNegativePreferences);
                }
                catch (Exception exc)
                {
                    AppDebug.Exception(exc, "RegisterNegativeEffectsPage.OnNavigatedTo");

                }
            }
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }

        private void BackToPositiveEffectsRegister(object sender, TappedRoutedEventArgs e)
        { // Save changes to checkboxes before navigating back
            PagesUtilities.GetAllCheckBoxesTags(RegisterNegativeEffectsGrid,
                    out List<int> intList);
            GlobalContext.RegisterContext.IntNegativePreferences = intList;

            Frame.Navigate(typeof(RegisterPositiveEffectsPage));
        }

        private async void Register(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage res = null;

            try
            { // Build register request
                progressRing.IsActive = true;

                // Get negative checkboxes
                PagesUtilities.GetAllCheckBoxesTags(RegisterNegativeEffectsGrid,
                out List<int> intList);

                GlobalContext.RegisterContext.IntNegativePreferences = intList;

                // Send request with register information
                res = await HttpManager.Manager.Post(Constants.MakeUrl("register"), GlobalContext.RegisterContext);


                if (res != null)
                { // Request succeeded
                    var content = res.GetContent();

                    switch (res.StatusCode)
                    { // Register succeeded
                        case HttpStatusCode.Created:
                        case HttpStatusCode.OK:
                            Status.Text = "Register Successful!";
                            PagesUtilities.SleepSeconds(1);
                            Frame.Navigate(typeof(DashboardPage), res);
                            break;
                        // Register failed
                        case HttpStatusCode.BadRequest:
                            Status.Text = "Register failed!\n" + content["message"];
                            break;

                        default:
                            Status.Text = "Error!\n" + content["message"];
                            break;
                    }
                }
                else
                {
                    Status.Text = "Register failed!\nPost operation failed";
                }
            }
            catch (Exception exc)
            {
                AppDebug.Exception(exc, "Register");
            }
            finally
            {
                progressRing.IsActive = false;
            }
        }
    }
}
