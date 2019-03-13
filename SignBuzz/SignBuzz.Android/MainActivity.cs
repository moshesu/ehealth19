using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
namespace SignBuzz.Droid
{
    [Activity(Label = "SignBuzz", Icon = "@mipmap/icon", Theme = "@style/splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthenticate
    {
        // Define a authenticated user.
        private MobileServiceUser user;

        public async Task<bool> Authenticate(bool google)
        {
            var success = false;
            var message = string.Empty;
            try
            {
                if (google)
                {
                    // Sign in with Facebook login using a server-managed flow.
                    user = await MainUserManager.DefaultManager.CurrentClient.LoginAsync(this,
                        MobileServiceAuthenticationProvider.Google, "signbuzz");
                    if (user != null)
                    {
                        App.user = user;
                        App.userId = user.UserId;
                        message = "Great You logged in!";
                        success = true;
                    }
                }
                else
                {
                    // Sign in with Facebook login using a server-managed flow.
                    user = await MainUserManager.DefaultManager.CurrentClient.LoginAsync(this,
                        MobileServiceAuthenticationProvider.Facebook, "signbuzz");
                    if (user != null)
                    {
                        App.user = user;
                        App.userId = user.UserId;
                        message = "Great You logged in!";
                        success = true;
                    }
                }
                
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.Create().Show();

            return success;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.Window.RequestFeature(WindowFeatures.ActionBar);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.SetTheme(Resource.Style.MainTheme);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState); // add this line to your code, it may also be called: bundle
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            // Initialize the authenticator before loading the app.
            App.Init((IAuthenticate)this);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}