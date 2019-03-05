using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using SleepItOff.iOS;
using Xamarin.Forms;
[assembly: Dependency(typeof(ClearCookies))]

namespace SleepItOff.iOS
{
    public class ClearCookies : IClearCookies
    {
        public void ClearAllCookies()
        {
            NSHttpCookieStorage CookieStorage = NSHttpCookieStorage.SharedStorage;
            foreach (var cookie in CookieStorage.Cookies)
                CookieStorage.DeleteCookie(cookie);
        }
    }
}
