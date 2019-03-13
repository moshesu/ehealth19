using System;
using System.Collections.Generic;
using System.Text;


namespace SleepItOff
{
    public interface IBluetoothService
    {
        IBluetoothService CreateBluetoothService();
        int EnableBluetooth();
        int DisableBluetooth();
    }
}
