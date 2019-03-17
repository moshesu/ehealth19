using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SignBuzz.Compete
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Submit : ContentPage
	{
        int res;
        
		public Submit (int res)
		{
			InitializeComponent ();
            this.res = res;
            score.Text = res.ToString();
		}
        private async void submit_handler(object sender, EventArgs e)
        {
            try
            {
                double lati;
                double longi;
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    lati = location.Latitude;
                    longi = location.Longitude;
                    List<User> users = await MainUserManager.DefaultManager.CurrentUserTable
                   .Where(user => user.UserId == App.userId)
                   .ToListAsync();
                    await MainUserManager.DefaultManager.SaveUserCompeteAsync(new User_compete { UserId = App.userId, Name = users[0].Name , Lati = lati, Longi = longi, Res = res });
                    submit_btn.IsVisible = false;
                    finish.IsVisible = true;

                    await DisplayAlert("Great!", "Youre score had been saved!" , "OK");
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");
                      
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

        }

    }
}