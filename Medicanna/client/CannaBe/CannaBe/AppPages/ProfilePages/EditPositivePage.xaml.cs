using CannaBe.Enums;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace CannaBe.AppPages.ProfilePages
{
    public sealed partial class EditPositivePage : Page
    {
        public EditPositivePage()
        {
            this.InitializeComponent();
            PagesUtilities.AddBackButtonHandler((object sender, Windows.UI.Core.BackRequestedEventArgs e) =>
            {
                BackToEditMedical(null, null);
            });
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var req = GlobalContext.CurrentUser.Data.PositivePreferences;

            if (req != null)
            {
                try
                {
                    PagesUtilities.SetAllCheckBoxesTags(EditPositiveEffectsGrid,
                                     PositivePreferencesEnumMethods.FromEnumToIntList(req));
                }
                catch (Exception exc)
                {
                    AppDebug.Exception(exc, "EditPositiveEffectsPage.OnNavigatedTo");

                }
            }
        }


        private void BackToEditMedical(object sender, TappedRoutedEventArgs e)
        {
            PagesUtilities.GetAllCheckBoxesTags(EditPositiveEffectsGrid,
                           out List<int> intList);

            GlobalContext.RegisterContext.IntPositivePreferences = intList;

            Frame.Navigate(typeof(EditMedicalPage));
        }

        private void GoToEditNegative(object sender, TappedRoutedEventArgs e)
        {
            PagesUtilities.GetAllCheckBoxesTags(EditPositiveEffectsGrid,
                                                 out List<int> intList);

            GlobalContext.RegisterContext.IntPositivePreferences = intList;

            Frame.Navigate(typeof(EditNegativePage));
        }
    }
}
