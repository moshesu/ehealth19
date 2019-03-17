using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace CannaBe
{
    class UserData
    {
        public LoginResponse Data { get; }
        public List<UsageData> UsageSessions;

        public UserData(LoginResponse data)
        {
            Data = data;
            UsageSessions = new List<UsageData>();
        }

        public async Task<bool> RemoveUsage(UsageData usage)
        {
            bool status = false;
            try
            {
                UsageSessions.Remove(usage);

                var res = await HttpManager.Manager.Delete(Constants.MakeUrl($"usage/{Data.UserID}/{usage.UsageId}"));

                if (res == null)
                {
                    throw new Exception("Delete response is null");
                }

                if(res.IsSuccessStatusCode)
                {
                    status = true;
                }
                else
                {
                    AppDebug.Line("Error while removing usage");
                }
            }
            catch (Exception x)
            {
                AppDebug.Exception(x, "RemoveUsage");
            }

            return status;
        }
    }
}
