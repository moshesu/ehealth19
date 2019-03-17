using Microsoft.Band;
using System;
using System.Threading.Tasks;

namespace CannaBe
{
    public class BandModel : ViewModel
    {
        public static IBandInfo SelectedBand { get; set; }
        public static IBandClient BandClient { get; set; }

        public static bool IsConnected
        {
            get
            {
                return BandClient != null;
            }
        }

        public static async Task FindDevicesAsync()
        {
            AppDebug.Line("In FindDevicesAsync");
            var bands = await BandClientManager.Instance.GetBandsAsync();
            if (bands != null && bands.Length > 0)
            {
                SelectedBand = bands[0]; // take the first band
                AppDebug.Line($"Found Band [{SelectedBand.Name}]");
            }
            else
            {
                AppDebug.Line("Did not find Band");

            }
        }

        public static async Task InitAsync()
        {
            AppDebug.Line("In InitAsync");
            try
            {
                if (IsConnected)
                    return;

                await FindDevicesAsync();
                if (SelectedBand != null)
                {
                    AppDebug.Line("connecting to band");

                    BandClient = await BandClientManager.Instance.ConnectAsync(SelectedBand);

                    AppDebug.Line("connected to band");

                }
            }
            catch (Exception x)
            {
                AppDebug.Exception(x, "InitAsync");
            }
        }
    }
}
