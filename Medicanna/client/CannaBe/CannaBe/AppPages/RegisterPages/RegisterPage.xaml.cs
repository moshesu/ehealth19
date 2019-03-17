using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CannaBe
{
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            this.InitializeComponent();
            this.FixPageSize();
            PagesUtilities.AddBackButtonHandler((object sender, Windows.UI.Core.BackRequestedEventArgs e) =>
            {
                BackToHome(null, null);
            });
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);

            var req = GlobalContext.RegisterContext;

            if (req != null)
            {
                try
                { // Load previously entered details, or empty if none exist
                    Username.Text = req.Username ?? "";
                    Password.Password = "";
                    Country.Text = req.Country ?? "";
                    City.Text = req.City ?? "";
                    Email.Text = req.Email ?? "";

                    if (req.Gender != null)
                    {
                        if (req.Gender == "Male")
                            Gender.SelectedIndex = 1;
                        else if (req.Gender == "Female")
                            Gender.SelectedIndex = 2;
                        else
                            Gender.SelectedIndex = 0;
                    }
                }
                catch (Exception exc)
                {
                    AppDebug.Exception(exc, "RegisterPage.OnPageLoaded");
                }
            }
        }

        private void ContinueMedicalRegister(object sender, RoutedEventArgs e)
        {
            int flag = 0;
            try
            { // Check valid gender
                if (!Gender.SelectedValue.ToString().Equals(null))
                {

                }
            }
            catch
            {
                flag = 1;
            }

            // Check if properties are valid
            if (string.IsNullOrEmpty(Username.Text))
            {
                Status.Text = "Please enter a valid username";
            }
            else if (string.IsNullOrEmpty(Password.Password))
            {
                Status.Text = "Please enter a valid password";
            }
            else if (flag == 1)
            {
                Status.Text = "Please enter a valid gender";
            }
            else if (string.IsNullOrEmpty(Country.Text))
            {
                Status.Text = "Please enter a valid country";
            }
            else if (string.IsNullOrEmpty(City.Text))
            {
                Status.Text = "Please enter a valid city";
            }
            else if (string.IsNullOrEmpty(Email.Text))
            {
                Status.Text = "Please enter a valid email";
            }
            else if (!Email.Text.IsValidEmail())
            {
                Status.Text = "Please enter a valid email";
            }
            else
            { // All properties are valid, build request to continue
                if (GlobalContext.RegisterContext == null)
                {
                    GlobalContext.RegisterContext = new RegisterRequest();
                }

                GlobalContext.RegisterContext.Username = Username.Text;
                GlobalContext.RegisterContext.Password = Password.Password;
                GlobalContext.RegisterContext.DOB = DOB.Date.Day + "/" + DOB.Date.Month + "/" + DOB.Date.Year;
                GlobalContext.RegisterContext.Gender = Gender.SelectedValue.ToString();
                GlobalContext.RegisterContext.Country = Country.Text;
                GlobalContext.RegisterContext.City = City.Text;
                GlobalContext.RegisterContext.Email = Email.Text;

                Frame.Navigate(typeof(RegisterMedicalPage), GlobalContext.RegisterContext);
            }
        }

        private void BackToHome(object sender, TappedRoutedEventArgs e)
        { // Delete registration request and return to dashboard
            GlobalContext.RegisterContext = null;
            Frame.Navigate(typeof(MainPage));
        }
        private void Page_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        { // enter key
            if (e.Key == VirtualKey.Enter)
            {
                ContinueMedicalRegister(sender, e);
            }
        }
    }
}
