using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace eHealthWorkshopGroup4.Droid
{
    [Activity(Label = "eHealthWorkshopGroup4", Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
    class SplashActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startupwork = new Task(() => { Simulatestartup(); });
            startupwork.Start();
        }

        async void Simulatestartup()
        {
            await Task.Delay(500);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }

    }
}