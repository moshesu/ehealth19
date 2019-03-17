using Newtonsoft.Json;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace CannaBe.AppPages.InformationPages
{
    public sealed partial class StrainSearchResults : Page
    {
        Dictionary<string, string> res;
        public StrainSearchResults()
        {
            this.InitializeComponent();
            this.FixPageSize();
            PagesUtilities.AddBackButtonHandler((object sender, Windows.UI.Core.BackRequestedEventArgs e) =>
            {
                BackToSearchPage(null, null);
            });
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (GlobalContext.searchType == 1) // Searched by strain
            {
                var req = (string)e.Parameter;
                searchByStrain(req);
            }
            else if (GlobalContext.searchType == 2) // Navigated from effect search
            {
                var req = (Strain)e.Parameter;
                searchFromEffects(req);
            }
            else
            {
                AppDebug.Line("Invalid navigation!");
            }
        }

        private void searchFromEffects(Strain req) // Display data if arrived from effect search
        {
            strain.Text = req.Name + ":";
            desc.Text = req.Description ?? "No description available for this strain";
            if (req.NumberOfUsages != 0)
            { // Strain has been used before
                score.Text = "Overall rank by users: " + req.Rank;
                numberofusages.Text = "Ranked by: " + req.NumberOfUsages + " users";
            }
            else score.Text = "This strain has not been ranked yet!";
        }

        private void searchByStrain(string req) // Display data if arrived from name search
        {
            string name = "", description = "", rank = "", status = "", usagenumstr = "";
            int usagenum = 0;
            res = JsonConvert.DeserializeObject<Dictionary<string, string>>(req);
            try
            { // Build strain parameters from respond
                res.TryGetValue("name", out name);
                res.TryGetValue("description", out description);
                res.TryGetValue("rank", out rank);
                res.TryGetValue("number_of_usages", out usagenumstr);
                int.TryParse(usagenumstr, out usagenum);

                strain.Text = name + ":";
                desc.Text = description;
                if (usagenum != 0)
                { // Strain has been used before
                    score.Text = "Overall rank by users: " + rank;
                    numberofusages.Text = "Ranked by: " + usagenum + " users";
                }
                else
                {
                    score.Text = "This strain has not been ranked yet!";
                }
            }
            catch
            { // Invalid strain name
                res.TryGetValue("status", out status);
                if (int.Parse(status) == 400) Status.Text = "Not a valid strain name - Please try again.";
            }
        }

        private void BackToSearchPage(object sender, TappedRoutedEventArgs e)
        { // Back to page based on which search was navigated from
            if (GlobalContext.searchType == 1) Frame.Navigate(typeof(StrainInformationPage));
            else if (GlobalContext.searchType == 2) Frame.Navigate(typeof(EffectsSearchResults));
        }

        private void GoToDashboard(object sender, TappedRoutedEventArgs e)
        {
            GlobalContext.searchResult = null; // Delete search result
            Frame.Navigate(typeof(DashboardPage));
        }
    }
}
