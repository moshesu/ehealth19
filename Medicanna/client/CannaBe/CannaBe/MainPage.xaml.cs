using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CannaBe
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.FixPageSize();
        }

        private void GoToLoginPage(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private void GoToRegisterPage(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage));
        }

        private void GoToDashboardPage(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DashboardPage));
        }

        private void LocalHostDebugChanged(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (LocalHostDebug.IsChecked == true)
                Constants.IsLocalHost = true;
            else
                Constants.IsLocalHost = false;
        }

        private void OnPageLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            LocalHostDebug.IsChecked = Constants.IsLocalHost;
            Application.Current.Suspending += AppExitHandler;
        }

        private void AppExitHandler(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            AppDebug.Line("Exiting app!");
            deferral.Complete();
        }

        private void Page_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            e.Handled = true;
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                GoToLoginPage(null, null);
            }
        }

        private void ExitApp(object sender, TappedRoutedEventArgs e)
        {
            AppDebug.Line("Exiting app!");
            Application.Current.Exit();
        }
    }
}
