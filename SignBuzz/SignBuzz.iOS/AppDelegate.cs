using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace SignBuzz.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IAuthenticate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            ImageCircleRenderer.Init();
            App.Init(this);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
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
                    user = await MainUserManager.DefaultManager.CurrentClient.LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController,
                        MobileServiceAuthenticationProvider.Google, "signbuzz");
                    if (user != null)
                    {
                        App.user = user;
                        message = string.Format("you are now signed-in as {0}.",
                            user.UserId);
                        success = true;
                    }
                }
                else
                {
                    // Sign in with Facebook login using a server-managed flow.
                    user = await MainUserManager.DefaultManager.CurrentClient.LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController,
                        MobileServiceAuthenticationProvider.Facebook, "signbuzz");
                    if (user != null)
                    {
                        App.user = user;
                        message = string.Format("you are now signed-in as {0}.",
                            user.UserId);
                        success = true;
                    }
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            UIAlertView avAlert = new UIAlertView("Sign-in result", message, null, "OK", null);
            avAlert.Show();

            return success;
        }
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return MainUserManager.DefaultManager.CurrentClient.ResumeWithURL(url);
        }
    }
}
