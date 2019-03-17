using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Radios;

namespace CannaBe
{
    class BandContext
    {
        HeartRateModel _hearRateModel;
        ContactModel _contactModel;

        public BandContext()
        {
        }

        public static async Task<bool> GetBluetoothIsSupportedAsync()
        {
            var radios = await Radio.GetRadiosAsync();
            return radios.FirstOrDefault(radio => radio.Kind == RadioKind.Bluetooth) != null;
        }

        public static async Task<bool> GetBluetoothIsEnabledAsync()
        {
            var radios = await Radio.GetRadiosAsync();
            var bluetoothRadio = radios.FirstOrDefault(radio => radio.Kind == RadioKind.Bluetooth);
            return bluetoothRadio != null && bluetoothRadio.State == RadioState.On;
        }


        public bool IsConnected()
        {
            return BandModel.IsConnected;
        }

        public async Task<bool> PairBand()
        {
            bool ret = false;

            do
            {
                await BandModel.InitAsync();

                if (!BandModel.IsConnected)
                {
                    AppDebug.Line("Band NOT CONNECTED");
                    break;
                }

                AppDebug.Line("Band CONNECTED");

                _hearRateModel = new HeartRateModel();
                _contactModel = new ContactModel();

                ret = true;

            } while (false);

            return ret;
        }

        public async Task<bool> StartHeartRate(HeartRateModel.ChangedHandler handler)
        {
            bool ret = false;
            try
            {
                AppDebug.Line("Initializing HeartRate...");

                // Heart Rate
                await _hearRateModel.InitAsync();
                if(handler != null)
                {
                    _hearRateModel.Changed += handler;
                }
                ret = await _hearRateModel.Start();
            }
            catch (Exception x)
            {
                AppDebug.Exception(x, "InitHeartRate");
            }

            return ret;
        }

        public void StopHeartRate()
        {
            try
            {
                AppDebug.Line("Finalizing HeartRate...");

                // Heart Rate
                _hearRateModel.Stop();
            }
            catch (Exception x)
            {
                AppDebug.Exception(x, "InitHeartRate");
            }
        }

        private async void StartContact(ContactModel.ChangedHandler handler)
        {
            await _contactModel.InitAsync();
            _contactModel.Changed += handler;
            _contactModel.Start();
        }
    }
}
