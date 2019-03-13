using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Android.Webkit;
using SleepItOff.Droid;

[assembly: Dependency(typeof(ClearCookies))]

namespace SleepItOff.Droid
{
    public class ClearCookies : IClearCookies
    {
        public void ClearAllCookies()
        {
            var cookieManager = CookieManager.Instance;
            cookieManager.RemoveAllCookie();
        }
    }
}
