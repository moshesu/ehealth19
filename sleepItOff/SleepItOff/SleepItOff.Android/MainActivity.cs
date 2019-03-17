using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using System.Threading.Tasks;


namespace SleepItOff.Droid
{
    [Activity(Label = "SleepItOff", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        PowerManager.WakeLock wL;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            var context = this.ApplicationContext;
            PowerManager powerManager = (PowerManager)context.GetSystemService("power");
             wL = powerManager.NewWakeLock(WakeLockFlags.Partial, "whatever");
            wL.Acquire();
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        protected override void OnDestroy()
        {
            wL.Release();
            base.OnDestroy();
        }


        // @Omer
        // instead of this, created interface IBluetoothService in SleepItOff Croos platform directory
        // and then created implementing class BluetoothServiceAndroid in SleepItOff.Android
        // the code is here for now if we will need it, if not it can be erased

        /*
        public async void SyncBandWithCloud(View view)
        {
            int bt = EnableBT(view);
            if (bt > 0) // if we actually turned on the bluetooth
            {
                await Task.Delay(60000);
            }
            else // if bluetooth was already turned on
            {
                DisableBT(view);
                bt = EnableBT(view);
                await Task.Delay(60000); // wait for 60 seconds
            }
            DisableBT(view);
            bt = EnableBT(view); // enable bluetooth for the second time
            await Task.Delay(60000); // wait for 60 seconds
            DisableBT(view); // disable bluetooth at the end
        }

        public int EnableBT(View view)
        {
            BluetoothAdapter mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            if(!mBluetoothAdapter.IsEnabled)           
            {
                mBluetoothAdapter.Enable();
                return 1;
            }
            return 0;
        }

        public void DisableBT(View view)
        {
            BluetoothAdapter mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            if (mBluetoothAdapter.IsEnabled)
            {
                mBluetoothAdapter.Disable();
            }
        }
        */

    }
}