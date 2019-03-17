using CannaBe.AppPages.InformationPages;
using System;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CannaBe.AppPages
{
    public sealed partial class StrainInformationPage : Page
    {
        private readonly static string STRAIN_EXAMPLE = "e.g. 'Alaska'";

        public StrainInformationPage()
        {
            this.InitializeComponent();
            this.FixPageSize();
            PagesUtilities.AddBackButtonHandler();
        }

        private void BoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBoxSender = sender as TextBox;

            if (textBoxSender.Text == STRAIN_EXAMPLE)
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
                textBoxSender.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.White);
                textBoxSender.Text = STRAIN_EXAMPLE;

            }
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void BackToInformation(object sender, TappedRoutedEventArgs e)
        {
            GlobalContext.searchResult = null;
            Frame.Navigate(typeof(InformationPage));
        }

        private async void SearchStrain(object sender, RoutedEventArgs e)
        {
            // Get checked effects
            PagesUtilities.GetAllCheckBoxesTags(MedicalSearchGrid, out List<int> MedicalList);
            PagesUtilities.GetAllCheckBoxesTags(PositiveSearchGrid, out List<int> PositiveList);

            // Produce bitmap of effects
            int MedicalBitMap = StrainToInt.FromIntListToBitmap(MedicalList);
            int PositiveBitMap = StrainToInt.FromIntListToBitmap(PositiveList);
            var url = "";

            if ((MedicalList.Count == 0) && (PositiveList.Count == 0) && ((StrainName.Text == "") || (StrainName.Text == "e.g. 'Alaska'")))
            { // Nothing chosen
                Status.Text = "Invaild Search! Please enter search parameter";
            }
            else
            {
                Status.Text = "";

                if ((StrainName.Text != "") && (StrainName.Text != "e.g. 'Alaska'"))
                { // Search by strain name
                    url = Constants.MakeUrl("strain/name/" + StrainName.Text);
                    GlobalContext.searchType = 1;
                }
                else
                { // Search by effect
                    url = Constants.MakeUrl($"strain/effects?medical={MedicalBitMap}&positive={PositiveBitMap}");
                    GlobalContext.searchType = 2;
                }
                try
                { // Build request for information
                    var res = HttpManager.Manager.Get(url);

                    if (res == null)
                        return;

                    var str = await res.Result.Content.ReadAsStringAsync();
                    AppDebug.Line(str);
                    if (GlobalContext.searchType == 1) Frame.Navigate(typeof(StrainSearchResults), str); // Search by name
                    else if (GlobalContext.searchType == 2)
                    { // Search by effect
                        GlobalContext.searchResult = str;
                        Frame.Navigate(typeof(EffectsSearchResults));
                    }
                }
                catch (Exception ex)
                {
                    AppDebug.Exception(ex, "SearchStrain");
                    await new MessageDialog("Failed get: \n" + url, "Exception in Search Strain").ShowAsync();
                }
            }
        }

        private void Page_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                SearchStrain(sender, e);
            }
        }

        private void Scroller_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if ((sender as ScrollViewer).VerticalOffset == 0)
            {
                ShowMore.Visibility = Visibility.Visible;
            }
            else
            {
                ShowMore.Visibility = Visibility.Collapsed;
            }
        }
    }
}
