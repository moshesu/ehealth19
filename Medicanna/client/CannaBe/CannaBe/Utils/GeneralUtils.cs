using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Radios;

namespace CannaBe
{
    static class GeneralUtils
    {
        //from here: stackoverflow.com/a/22078975
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    return await task;  // Very important in order to propagate exceptions
                }
                else
                {
                    throw new TimeoutException();
                }
            }
        }

        public static async Task EnableBluetoothAsync()
        {
            try
            {
                var access = await Radio.RequestAccessAsync();
                if (access != RadioAccessStatus.Allowed)
                {
                    return;
                }
                BluetoothAdapter adapter = await BluetoothAdapter.GetDefaultAsync();
                if (null != adapter)
                {
                    var btRadio = await adapter.GetRadioAsync();

                    await btRadio.SetStateAsync(RadioState.On);
                }
            }
            catch(Exception c)
            {
                AppDebug.Exception(c, "EnableBluetoothAsync");
            }
        }

        public static List<UsageData> ToUsageList(this UsageUpdateRequest[] list)
        {
            List<UsageData> lst = new List<UsageData>(list.Length);

            foreach (var v in list)
            {
                lst.Add(v);
            }

            return lst;
        }

        public static Dictionary<string, string> GetContent(this HttpResponseMessage res)
        {
            return HttpManager.ParseJson<Dictionary<string, string>>(res);
        }
    }
}
