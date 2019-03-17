using CannaBe.AppPages;
using CannaBe.AppPages.PostTreatmentPages;
using CannaBe.AppPages.RecomendationPages;
using CannaBe.AppPages.Usage;
using System;
using Windows.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace CannaBe
{
    public sealed partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            this.InitializeComponent();
            this.FixPageSize();
            PagesUtilities.AddBackButtonHandler((object sender, Windows.UI.Core.BackRequestedEventArgs e) =>
            {
                e.Handled = true;
                LogoutHandler(null, null);
            });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter == null)
                return;

            GlobalContext.AddUserToContext(e); // Add logined user to current context
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
            if(GlobalContext.CurrentUser == null)
            {
                UsageHistoryButton.IsEnabled = false;
                MyProfileButton.IsEnabled = false;
                Welcome.Text = "Debug Session, no user";
            }
            else
            { // Login successfull
                Welcome.Text = $"Welcome, {GlobalContext.CurrentUser.Data.Username}!";
                AppDebug.Line($"Wrote welocme text: [{Welcome.Text}]");
            }
        }

        private void GoToInformationPage(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(InformationPage));
        }

        private void GoToPostTreatment(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(PostTreatment));
        }

        private void LogoutHandler(object sender, TappedRoutedEventArgs e)
        { // Logout - delete local data

            GlobalContext.CurrentUser = null;
            GlobalContext.RegisterContext = null;
            GlobalContext.Band = null;
            UsageContext.ChosenStrain = null;
            UsageContext.DisplayUsage = null;
            UsageContext.Usage = null;
            Frame.Navigate(typeof(MainPage));
        }

        private void GoToStartUsage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StartUsage));
        }

        private void GoToUsageHistory(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UsageHistory));
        }

        private void GoToRecommendations(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MyRecomendations)); ;
        }

        private void GoToProfile(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProfilePage)); ;
        }

        private void GoToEmailPage(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ExportToEmail)); ;
        }
    }
}
