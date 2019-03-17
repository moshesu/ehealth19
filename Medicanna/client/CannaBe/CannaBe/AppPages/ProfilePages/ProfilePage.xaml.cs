using CannaBe.AppPages.ProfilePages;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;

namespace CannaBe.AppPages
{
    public sealed partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
            PagesUtilities.AddBackButtonHandler();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            var profile = GlobalContext.CurrentUser.Data;

            username.Text = profile.Username;
            dob.Text = profile.DOB; //format of dd/mm/yyyy
            age.Text = profile.DobDate.ToAge().ToString();
            place.Text = $"{profile.City}, {profile.Country}";

            FillTextList(profile.MedicalNeeds, ref MedicalNeeds);
            FillTextList(profile.PositivePreferences, ref Positive);
            FillTextList(profile.NegativePreferences, ref Negative);
        }

        private void FillTextList<T>(List<T> relevantList, ref Run t) where T : Enum
        {
            StringBuilder sb = new StringBuilder("");
            var med = relevantList.ToArray();
            for (int i = 0; i < med.Length; i++)
            {
                sb.Append(med[i].Name());

                if (i < med.Length - 1)
                {
                    sb.Append(", ");
                }
            }
            t.Text = sb.ToString();
        }

        private void GoToDashboard(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DashboardPage));
        }

        private void GoToEditPage(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(EditMedicalPage));
        }
    }
}
