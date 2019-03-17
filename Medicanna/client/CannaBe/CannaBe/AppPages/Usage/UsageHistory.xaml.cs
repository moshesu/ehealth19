using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;


namespace CannaBe.AppPages.Usage
{
    public sealed partial class UsageHistory : Page
    {
        private UsageData selectedUsage;

        public UsageHistory()
        {
            this.InitializeComponent();
        }

        private async void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            progressRing.IsActive = true;

            do
            {
                if (GlobalContext.CurrentUser == null)
                {
                    AppDebug.Line("GlobalContext.CurrentUser == null");
                    break;
                }

                // Update usage history
                await Task.Run(() => GlobalContext.UpdateUsagesContextIfEmptyAsync());

                if (GlobalContext.CurrentUser.UsageSessions == null)
                { // No usages
                    AppDebug.Line("GlobalContext.CurrentUser.UsageSessions == null");
                    break;
                }
                if (GlobalContext.CurrentUser.UsageSessions.Count == 0)
                { // No usages
                    UsageListGui.Visibility = Visibility.Collapsed;
                    NoUsageButton.Visibility = Visibility.Visible;
                    AppDebug.Line("GlobalContext.CurrentUser.UsageSessions.Count == 0");
                    break;
                }

                foreach (var usage in GlobalContext.CurrentUser.UsageSessions)
                {
                    if (usage == null)
                    {
                        AppDebug.Line("usage == null");
                        continue;
                    }
                    // Add usage to usage list displayed
                    UsageListGui.Items.Add(usage);
                }
            } while (false);
            progressRing.IsActive = false;

        }



        private void GoToDashboard(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DashboardPage));
        }

        private void UsageSelected(object sender, ItemClickEventArgs e)
        { // Select usage to view details on
            ListView lst = sender as ListView;
            UsageData u = e.ClickedItem as UsageData;
            AppDebug.Line($"Selected usage on [{u.StartTimeString}]");
            UsageContext.DisplayUsage = u;
            Frame.Navigate(typeof(UsageDisplay));
        }

        private async void Remove_Click(object sender, RoutedEventArgs e)
        { // Remove usage from history
            AppDebug.Line($"Remove usage on [{selectedUsage.StartTimeString}]");
            try
            {
                var yesCommand = new UICommand("Remove", async cmd =>
                {
                    AppDebug.Line("removing...");
                    var b = await GlobalContext.CurrentUser.RemoveUsage(selectedUsage);
                    if (!b)
                    {
                        await new MessageDialog("There was an error while deleting the usage from the server.", "Error").ShowAsync();
                    }
                    else
                    {
                        await new MessageDialog("Usage removed successfuly.", "Success").ShowAsync();
                    }
                    Frame.Navigate(typeof(UsageHistory));
                });
                var noCommand = new UICommand("Cancel", cmd =>
                {
                    AppDebug.Line("Cancel remove");
                });
                var dialog = new MessageDialog("Are you sure you want to remove the usage from the history?", "Remove Usage")
                {
                    Options = MessageDialogOptions.None
                };
                dialog.Commands.Add(yesCommand);
                dialog.Commands.Add(noCommand);

                await dialog.ShowAsync();
            }
            catch (Exception x)
            {
                AppDebug.Exception(x, "Remove_Click");
            }

        }

        private void ListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            try
            { // Open right click menu
                ListView lst = sender as ListView;
                selectedUsage = ((FrameworkElement)e.OriginalSource).DataContext as UsageData;
                if (selectedUsage != null)
                {
                    UsageMenu.ShowAt(lst, e.GetPosition(lst));
                    AppDebug.Line($"Right click usage on [{selectedUsage.StartTimeString}]");
                }
            }
            catch (Exception x)
            {
                AppDebug.Exception(x, "ListView_RightTapped");
            }
        }

        private void AddNewUsage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StartUsage));
        }
    }
}
