using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CannaBe
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            this.FixPageSize();
            PagesUtilities.AddBackButtonHandler();
        }

        private void BoxGotFocus(object sender, RoutedEventArgs e)
        {
            //TextBox textBoxSender = sender as TextBox;

            //textBoxSender.SelectAll();
        }

        private void BoxLostFocus(object sender, RoutedEventArgs e)
        {
            //TextBox textBoxSender = sender as TextBox;

            //textBoxSender.Select(0, 0);
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }

        private async void PostLogin(object sender, RoutedEventArgs e)
        {
            AppDebug.Line("PostLogin");
            HttpResponseMessage res = null;
            progressRing.IsActive = true;

            try
            { // Create login request
                var req = new LoginRequest(Username.Text, Password.Password);

                res = await Task.Run(async () => await HttpManager.Manager.Post(Constants.MakeUrl("login"), req));

                if (res != null)
                { // Request succeeded
                    var content = res.GetContent();

                    switch (res.StatusCode)
                    { // Login successfull
                        case HttpStatusCode.OK:
                            AppDebug.Line("Login success!");
                            PagesUtilities.SleepSeconds(1);
                            progressRing.IsActive = false;
                            Frame.Navigate(typeof(DashboardPage), res);
                            break;
                    // Login failed
                        case HttpStatusCode.BadRequest:
                            Status.Text = "Login failed!\n" + content["message"];
                            break;

                        default:
                            Status.Text = "Error!\n" + content["message"];
                            break;
                    }
                }
                else
                {
                    Status.Text = "Login failed!\nPost operation failed";
                }

            }
            catch (Exception exc)
            {
                Status.Text = "Exception during login";

                AppDebug.Exception(exc, "PostLogin");
            }
            finally
            {
                progressRing.IsActive = false;
            }
        }

        private void BackToMain(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void Page_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        { // enter key
            if (e.Key == VirtualKey.Enter)
            {
                if (Username.Text.Length > 0 && Password.Password.Length > 0)
                { // Post login request with enter key
                    PostLogin(sender, e);
                }
            }
        }
    }
}