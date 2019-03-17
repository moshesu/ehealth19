using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CannaBe.AppPages
{
    public sealed partial class ExportToEmail : Page
    {
        Regex emailValidator;

        public ExportToEmail()
        {
            this.InitializeComponent();
            emailValidator = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            PagesUtilities.AddBackButtonHandler();
        }

        private void GoToDashboard(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DashboardPage));
        }

        private void EmailChanged(object sender, KeyRoutedEventArgs e)
        { // Check email as it changes
            var t = sender as TextBox;
            var b = t.Text.IsValidEmail();
            //SendButton.IsEnabled = b;
            //InvalidMailTitle.Visibility = b ? Windows.UI.Xaml.Visibility.Collapsed : Windows.UI.Xaml.Visibility.Visible;
        }

        private async void SendEmailAsync(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage res = null;
            SendEmailRequest req;
            if (EmailAddressBox.Text.IsValidEmail()) // Check email validity
            {
                try
                {
                    progressRing.IsActive = true;
                    
                    // Send email request
                    req = new SendEmailRequest(EmailAddressBox.Text, FreeMessageBox.Text);
                    // Export usages
                    res = await HttpManager.Manager.Post(Constants.MakeUrl($"usage/export/{GlobalContext.CurrentUser.Data.UserID}"), req);

                    progressRing.IsActive = false;

                    if (res != null)
                    {
                        if (res.IsSuccessStatusCode)
                        { // Email sent successfully
                            await new MessageDialog($"An email was sent to {EmailAddressBox.Text} successfully", "Success").ShowAsync();
                        }
                        else
                        {
                            await new MessageDialog($"There was an error while sending the mail (status: {res.StatusCode}, message: \"{res.GetContent()["message"]}\"", "Error").ShowAsync();
                        }
                    }
                    else
                    {
                        await new MessageDialog($"There was an error with the server, response is empty.", "Error").ShowAsync();
                    }

                    PagesUtilities.SleepSeconds(1);
                    Frame.Navigate(typeof(DashboardPage));
                }
                catch (Exception x)
                {
                    AppDebug.Exception(x, "UsageUpdate");
                }
            }
            else
            { // Email address not valid
                await new MessageDialog("Invalid email address. Please try again").ShowAsync();
            }

        }
    }
}
