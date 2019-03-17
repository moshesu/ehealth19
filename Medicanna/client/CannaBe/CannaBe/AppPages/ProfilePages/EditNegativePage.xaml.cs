using CannaBe.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace CannaBe.AppPages.ProfilePages
{
    public sealed partial class EditNegativePage : Page
    {
        public EditNegativePage()
        {
            this.InitializeComponent();
            PagesUtilities.AddBackButtonHandler((object sender, Windows.UI.Core.BackRequestedEventArgs e) => 
            {
                BackToPositive(null, null);
            });

        }

        public void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            PagesUtilities.DontFocusOnAnythingOnLoaded(sender, e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var req = GlobalContext.CurrentUser.Data.NegativePreferences;

            if (req != null)
            {
                try
                {
                    PagesUtilities.SetAllCheckBoxesTags(EditNegativeEffectsGrid,
                                     NegativePreferencesEnumMethods.FromEnumToIntList(req));
                }
                catch (Exception exc)
                {
                    AppDebug.Exception(exc, "EditPositiveEffectsPage.OnNavigatedTo");

                }
            }
        }


        private void BackToPositive(object sender, TappedRoutedEventArgs e)
        {
            PagesUtilities.GetAllCheckBoxesTags(EditNegativeEffectsGrid, out List<int> intList);
            GlobalContext.RegisterContext.IntNegativePreferences = intList;

            Frame.Navigate(typeof(EditPositivePage));
        }

        private async void EditRegister(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage res = null;
            var user_id = GlobalContext.CurrentUser.Data.UserID;


            try
            {
                progressRing.IsActive = true;

                PagesUtilities.GetAllCheckBoxesTags(EditNegativeEffectsGrid,
                out List<int> intList);

                GlobalContext.RegisterContext.IntNegativePreferences = intList;
                res = await HttpManager.Manager.Post(Constants.MakeUrl($"edit/{user_id}"), GlobalContext.RegisterContext);


                if (res != null)
                {
                    if (res.IsSuccessStatusCode)
                    {
                        Status.Text = "Edit profile Successful!";
                        PagesUtilities.SleepSeconds(0.5);
                        Frame.Navigate(typeof(DashboardPage), res);
                    }
                    else
                    {
                        Status.Text = "Register failed! Status = " + res.StatusCode;
                    }
                }
                else
                {
                    Status.Text = "Register failed!\nPost operation failed";
                }
            }
            catch (Exception exc)
            {
                AppDebug.Exception(exc, "Register");
            }
            finally
            {
                progressRing.IsActive = false;
            }
        }
    }
}
