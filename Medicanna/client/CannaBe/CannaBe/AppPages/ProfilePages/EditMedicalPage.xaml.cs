using CannaBe.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CannaBe.AppPages.ProfilePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditMedicalPage : Page
    {
        public EditMedicalPage()
        {
            this.InitializeComponent();
            PagesUtilities.AddBackButtonHandler();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var req = GlobalContext.CurrentUser.Data.MedicalNeeds;

            if (req != null)
            {
                try
                {
                    PagesUtilities.SetAllCheckBoxesTags(EditMedicalGrid,
                                     MedicalEnumMethods.FromEnumToIntList(req));
                }
                catch (Exception exc)
                {
                    AppDebug.Exception(exc, "EditMedicalEffectsPage.OnNavigatedTo");

                }
            }
        }

        private void BackToProfile(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProfilePage));
        }

        private void GoToEditPositive(object sender, TappedRoutedEventArgs e)
        {
            if (GlobalContext.RegisterContext == null)
            {
                GlobalContext.RegisterContext = new RegisterRequest();
            }

            GlobalContext.RegisterContext.Username = GlobalContext.CurrentUser.Data.Username;
            GlobalContext.RegisterContext.Password = "0";
            GlobalContext.RegisterContext.DOB = GlobalContext.CurrentUser.Data.DOB;
            GlobalContext.RegisterContext.Gender = GlobalContext.CurrentUser.Data.Gender;
            GlobalContext.RegisterContext.Country = GlobalContext.CurrentUser.Data.Country;
            GlobalContext.RegisterContext.City = GlobalContext.CurrentUser.Data.City;

            PagesUtilities.GetAllCheckBoxesTags(EditMedicalGrid,
                                                 out List<int> intList);

            GlobalContext.RegisterContext.IntListMedicalNeeds = intList;

            Frame.Navigate(typeof(EditPositivePage), GlobalContext.RegisterContext);
        }
    }
}
