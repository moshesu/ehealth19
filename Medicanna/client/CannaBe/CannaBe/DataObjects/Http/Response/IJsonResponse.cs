using System.Net.Http;

namespace CannaBe
{
    abstract class IJsonResponse
    {
        public static IJsonResponse CreateFromHttpResponse(HttpResponseMessage msg)
        {
            return HttpManager.ParseJson<IJsonResponse>(msg);
        }

        public static IJsonResponse CreateFromHttpResponse(object msg)
        {
            return HttpManager.ParseJson<IJsonResponse>(msg as HttpResponseMessage);
        }
    }
}
