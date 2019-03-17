using System.Net.Http;

namespace CannaBe
{
    abstract class Request
    {
        public static implicit operator HttpContent(Request req)
        { // Http request
            var json = HttpManager.CreateJson(req);
            try
            {
                AppDebug.Line("Created JSON:");
                AppDebug.Line(json.ReadAsStringAsync().Result);
            }
            catch
            {
                AppDebug.Line("Exception while printing json");
            }


            return json;
        }
    }
}
