using CannaBe.AppPages.Usage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CannaBe.AppPages.RecomendationPages
{
    public sealed partial class MyRecomendations : Page
    {
        enum SortType
        {
            MATCH,
            RANK,
            COUNT
        };

        private SortType sortType = SortType.MATCH;
        private SuggestedStrains strains;
        private SuggestedStrains matchSortedStrains = null;
        private SuggestedStrains rankSortedStrains = null;
        private SuggestedStrains countSortedStrains = null;

        public MyRecomendations()
        {
            InitializeComponent();
            PagesUtilities.AddBackButtonHandler();
        }

        private async void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            progressRing.IsActive = true;
            UsageContext.ChosenStrain = null;
            if (GlobalContext.CurrentUser != null)
            { // Get recommended strains for user from server
                Message.Text = "Searching for matching strains based on your profile...";

                var user_id = GlobalContext.CurrentUser.Data.UserID;
                var url = Constants.MakeUrl($"strains/recommended/{user_id}/"); // Build url for user

                try
                { // Send request to server
                    var res = await HttpManager.Manager.Get(url);

                    if (res == null)
                    {
                        progressRing.IsActive = false;
                        return;
                    }
                    // Successful request
                    PagesUtilities.SleepSeconds(0.2);

                    // Recommended strain list
                    strains = await Task.Run(() => HttpManager.ParseJson<SuggestedStrains>(res)); // Parsing JSON

                    if (strains.SuggestedStrainList.Count == 0)
                    {
                        ErrorNoStrainFound.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        switch (strains.Status)
                        { // Status for match type
                            case 0: // Full match
                                Message.Text = $"Showing {strains.SuggestedStrainList.Count} exactly matched strains:";
                                break;

                            case 1: // Partial match - medical match but positive dont
                                Message.Text = $"No exact matches found!\nTry updating your positive preferences.\nShowing {strains.SuggestedStrainList.Count} partially matching strains:";
                                break;

                            case 2: // Partial match - medical and positive don't match
                                Message.Text = $"No exact matches found!\nTry updating your positive and medical preferences.\nShowing {strains.SuggestedStrainList.Count} partially matching strains:";
                                break;
                        }
                        Scroller.Height = Stack.ActualHeight - Message.ActualHeight - 20;

                        Random rnd = new Random();

                        foreach (var strain in strains.SuggestedStrainList)
                        { // **Random numbers**
                            strain.Rank = rnd.Next(1, 100);
                            strain.NumberOfUsages = rnd.Next(0, 1000);
                        }

                        if (strains.Status != 0)
                        { // Calculate partal match rate
                            foreach (var strain in strains.SuggestedStrainList)
                            {
                                strain.MatchingPercent = strain / GlobalContext.CurrentUser;
                            }

                            strains.SuggestedStrainList.Sort(Strain.MatchComparison);
                            matchSortedStrains = new SuggestedStrains(strains.Status, new List<Strain>(strains.SuggestedStrainList)); ;
                        }

                        var names = $"[{string.Join(", ", from u in strains.SuggestedStrainList select $"{u.Name}")}]";
                        AppDebug.Line($"Status={strains.Status} Got {strains.SuggestedStrainList.Count} strains: {names}");

                        FillStrainList(strains);

                        foreach (var child in ButtonsGrid.Children)
                        { // Display buttons to choose strains
                            if (child.GetType() == typeof(Viewbox))
                            {
                                var b = (child as Viewbox).Child as RadioButton;
                                if (!((string)b.Tag == "match" && strains.Status == 0))
                                {
                                    b.IsEnabled = true;
                                }
                            }
                        }
                        ButtonsGrid.Opacity = 1;
                        //StrainList.Height = Scroller.ActualHeight;
                    }
                }
                catch (Exception x)
                {
                    AppDebug.Exception(x, "OnPageLoaded");
                    await new MessageDialog("Error while getting suggestions from the server", "Error").ShowAsync();
                }

                progressRing.IsActive = false;
            }

        }

        private void FillStrainList(SuggestedStrains localStrainList)
        {
            if (StrainList.Children.Count > 0)
            {
                StrainList.Children.Clear();
            }

            int i = 1;
            foreach (var strain in localStrainList.SuggestedStrainList)
            {
                string addition = null;

                switch (sortType)
                {
                    case SortType.MATCH:
                        addition = localStrainList.Status != 0 ? string.Format(" ({0:0}% match)", strain.MatchingPercent) : "";
                        break;

                    case SortType.RANK:
                        addition = string.Format(" (rank: {0:0})", strain.Rank);
                        break;

                    case SortType.COUNT:
                        addition = $" (count: {strain.NumberOfUsages})";
                        break;
                }
                var r = new RadioButton()
                {
                    Foreground = new SolidColorBrush(Windows.UI.Colors.Black),
                    FontSize = 15,
                    VerticalContentAlignment = VerticalAlignment.Top,
                    FontWeight = FontWeights.Bold,
                    Content = $"{i++}. {strain.Name}{addition}",
                    DataContext = strain
                };
                r.IsChecked = false;
                r.Checked += OnChecked;
                StrainList.Children.Add(r);
            }
        }

        private void OnChecked(object sender, RoutedEventArgs e)
        {
            //ContinueButton.IsEnabled = true;
            UsageContext.ChosenStrain = (sender as RadioButton).DataContext as Strain;
        }

        private void GoToDashboard(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DashboardPage));
        }

        private void ContinueHandler(object sender, RoutedEventArgs e)
        {
            if (UsageContext.ChosenStrain != null)
            {
                Frame.Navigate(typeof(StartUsage2));
            }
        }

        private void RadioChecked(object sender, RoutedEventArgs e)
        { // Radio buttons for sorting list
            var b = sender as RadioButton;
            switch (b.Tag)
            {
                case "match": // Sort by match percentage
                    sortType = SortType.MATCH;
                    RadioCount.IsChecked = false;
                    RadioRank.IsChecked = false;
                    FillStrainList(matchSortedStrains);
                    break;

                case "rank": // Sort by rank
                    sortType = SortType.RANK;
                    RadioCount.IsChecked = false;
                    RadioMatch.IsChecked = false;

                    if (rankSortedStrains == null)
                    { // Perform sort
                        strains.SuggestedStrainList.Sort(Strain.RankComparison);
                        rankSortedStrains = new SuggestedStrains(strains.Status, new List<Strain>(strains.SuggestedStrainList));
                    }
                    FillStrainList(rankSortedStrains);
                    break;

                case "count": // Sort by number of usages
                    sortType = SortType.COUNT;
                    RadioRank.IsChecked = false;
                    RadioMatch.IsChecked = false;
                    if (countSortedStrains == null)
                    { // Perform sort
                        strains.SuggestedStrainList.Sort(Strain.CountComparison);
                        countSortedStrains = new SuggestedStrains(strains.Status, new List<Strain>(strains.SuggestedStrainList));
                    }
                    FillStrainList(countSortedStrains);
                    break;
            }
        }
    }
}
