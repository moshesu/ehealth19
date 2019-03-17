using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Bluetooth;
using SleepItOff.Droid.Services;
using SleepItOff;
using System.Threading.Tasks;
using Android.Widget;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(BluetoothServiceAndroid))]
namespace SleepItOff.Droid.Services
{
    class BluetoothServiceAndroid : IBluetoothService
    {

        public BluetoothServiceAndroid() { }

        public IBluetoothService CreateBluetoothService()
        {
            return new BluetoothServiceAndroid();
        }

        public int EnableBluetooth()
        {
            BluetoothAdapter mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            if (!mBluetoothAdapter.IsEnabled)
            {
                mBluetoothAdapter.Enable();
                return 1;
            }
            return 0;
        }

        public int DisableBluetooth()
        {
            BluetoothAdapter mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            if (mBluetoothAdapter.IsEnabled)
            {
                mBluetoothAdapter.Disable();
            }
            return 1;
        }
    }
}