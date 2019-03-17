using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace CannaBe
{
    static class GlobalContext
    {
        public static UserData CurrentUser { get; set; } = null;
        public static RegisterRequest RegisterContext { get; set; } = null;
        public static BandContext Band { get; set; } = null;
        public static int searchType = 0;
        public static string searchResult { get; set; } = null;

        public static void AddUserToContext(NavigationEventArgs e)
        { // Add user to context after login
            try
            {
                CurrentUser = new UserData(LoginResponse.CreateFromHttpResponse(e.Parameter));
            }
            catch (System.Exception exc)
            {
                AppDebug.Exception(exc, "AddUserToContext");
                CurrentUser = null;
            }
        }

        public static void UpdateUsagesContextIfEmptyAsync()
        { // Update user usages in current context
            if (CurrentUser.UsageSessions.Count == 0)
            {
                AppDebug.Line("Updating usage history from server for user " + CurrentUser.Data.Username);
                var usages = Task.Run(() => GetUsagesFromServer()).GetAwaiter().GetResult();
                if (usages != null)
                { // Usages exist
                    CurrentUser.UsageSessions = usages;
                    var names = $"[{string.Join(", ", from u in usages select $"{u.UsageStrain.Name}-{u.StartTime.ToString("dd.MM.yy-HH:mm")}")}]";
                    AppDebug.Line($"Got {usages.Count} usages: {names}");

                }
            }
        }

        private static List<UsageData> GetUsagesFromServer()
        { // Get usages for user from DB
            try
            {
                var res = HttpManager.Manager.Get(Constants.MakeUrl("usage/" + GlobalContext.CurrentUser.Data.UserID));
                var str = res.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<UsageUpdateRequest[]>(str).ToUsageList();
            }
            catch (Exception x)
            {
                AppDebug.Exception(x, "GetUsagesFromServer");

                return null;
            }
        }
    }
}
