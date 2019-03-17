using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CannaBe.AppPages.InformationPages
{
    public sealed partial class OrganizationInformationPage : Page
    {
        public OrganizationInformationPage()
        {
            this.InitializeComponent();
            this.FixPageSize();
            PagesUtilities.AddBackButtonHandler();
        }
        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }

        private void BackToInformationPage(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(InformationPage));
        }

        private void GoToDashboard(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DashboardPage));
        }
    }
}
