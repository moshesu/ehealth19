using Newtonsoft.Json;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace CannaBe.AppPages.InformationPages
{
    public sealed partial class EffectsSearchResults : Page
    {
        public EffectsSearchResults()
        {
            this.InitializeComponent();
            this.FixPageSize();
            PagesUtilities.AddBackButtonHandler();
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SearchByEffects(GlobalContext.searchResult);
        }

        private void SearchByEffects(string req) // Search strains by effects chosen
        {
            SuggestedStrains strains = JsonConvert.DeserializeObject<SuggestedStrains>(req);
            if ( (strains.SuggestedStrainList.Count == 0) || (strains.Status != 0) ) // No result
            {
                Status.Text = "No strains found - Please narrow search parameters.";
            }
            if (strains.Status == 0) // Only exact matches
            {
                found.Text = $"Found {strains.SuggestedStrainList.Count} matching strains:";
                foreach (Strain s in strains.SuggestedStrainList)
                {
                    strainListGui.Items.Add(s);
                }
            }
        }
        private void StrainSelected(object sender, ItemClickEventArgs e) // Get information about strain
        {
            ListView lst = sender as ListView;
            Strain s = e.ClickedItem as Strain;
            Frame.Navigate(typeof(StrainSearchResults), s);
        }

        private void BackToSearchPage(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(StrainInformationPage));
        }

        private void GoToDashboard(object sender, TappedRoutedEventArgs e)
        {
            GlobalContext.searchResult = null;
            Frame.Navigate(typeof(DashboardPage));
        }
    }
}
