using CannaBe.AppPages.ProfilePages;
using CannaBe.Enums;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace CannaBe
{
    public sealed partial class RegisterMedicalPage : Page
    {
        public RegisterMedicalPage()
        {
            this.InitializeComponent();
            this.FixPageSize();
            PagesUtilities.AddBackButtonHandler((object sender, Windows.UI.Core.BackRequestedEventArgs e) =>
            {
                BackToRegister(null, null);
            });
        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var req = GlobalContext.RegisterContext;

            if (req != null)
            {
                try
                { // If checkboxes were chosen before, update them again
                    PagesUtilities.SetAllCheckBoxesTags(RegisterMedicalGrid,
                                     req.IntListMedicalNeeds);
                }
                catch(Exception exc)
                {
                    AppDebug.Exception(exc, "RegisterMedicalPage.OnNavigatedTo");
                }
            }
        }

        private void BackToRegister(object sender, TappedRoutedEventArgs e)
        { // Save changes to checkboxes before navigating back
            PagesUtilities.GetAllCheckBoxesTags(RegisterMedicalGrid,
                                     out List<int> intList);

            GlobalContext.RegisterContext.IntListMedicalNeeds = intList;

            Frame.Navigate(typeof(RegisterPage));
        }

        private void ContinuePositiveEffectsRegister(object sender, TappedRoutedEventArgs e)
        { // Save changes to checkboxes and continue
            PagesUtilities.GetAllCheckBoxesTags(RegisterMedicalGrid,
                                                 out List<int> intList);

            GlobalContext.RegisterContext.IntListMedicalNeeds = intList;

            Frame.Navigate(typeof(RegisterPositiveEffectsPage), GlobalContext.RegisterContext);
        }
    }
}
