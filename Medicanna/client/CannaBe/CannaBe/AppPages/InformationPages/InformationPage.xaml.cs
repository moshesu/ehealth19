using CannaBe.AppPages.InformationPages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CannaBe.AppPages
{
    public sealed partial class InformationPage : Page
    {
        public InformationPage()
        {
            this.InitializeComponent();
            this.FixPageSize();
            PagesUtilities.AddBackButtonHandler();
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
                textBoxSender.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Black);
                textBoxSender.Text = "Enter " + textBoxSender.Name;

            }
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }

        private void GoToDashboard(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DashboardPage));
        }

        private void GoToStrainInformation(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(StrainInformationPage));
        }

        private void GoToOrganizationInformation(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(OrganizationInformationPage));
        }

        private void GoToMedicalInformation(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MedicalInformationPage));
        }
    }
}
