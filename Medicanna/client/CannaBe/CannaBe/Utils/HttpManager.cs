using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using Windows.UI.Popups;

namespace CannaBe
{
    public sealed class HttpManager
    {
        private static HttpManager instance = null;
        private static HttpClient client = null;

        HttpManager()
        {
        }

        public static HttpManager Manager
        {
            get
            {
                if (instance == null)
                {
                    instance = new HttpManager();
                    client = new HttpClient
                    {
                        Timeout = new TimeSpan(0, 0, 10) //10 seconds
                    };
                }

                return instance;
            }
        }

        public static HttpContent CreateJson(object obj)
        {
            //from: https://stackoverflow.com/questions/23585919/send-json-via-post-in-c-sharp-and-receive-the-json-returned
            AppDebug.Line("Creating json");
            // Serialize our concrete class into a JSON String
            var stringPayload = JsonConvert.SerializeObject(obj, Formatting.Indented);
            AppDebug.Line("created json");

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            AppDebug.Line("created content");

            return httpContent;
        }

        public static T ParseJson<T>(HttpResponseMessage res)
        {
            return JsonConvert.DeserializeObject<T>(res.Content.ReadAsStringAsync().Result);
        }

        public async Task<HttpResponseMessage> Get(string URL)
        { // Get from server
            AppDebug.Line("In Get: " + URL);

            try
            {
                bool isInternetConnected = NetworkInterface.GetIsNetworkAvailable();
                if (!isInternetConnected)
                {
                    AppDebug.Line("Error - No Internet connection!");

                    await new MessageDialog("No internet connection!", "Error!").ShowAsync();
                    return null;
                }

                var response = await client.GetAsync(URL).ConfigureAwait(false);
                AppDebug.Line("finished get");

                var responseString = await response.Content.ReadAsStringAsync();

                if (responseString.Length < 2000)
                {
                    AppDebug.Line("response from get: [" + responseString + "]");
                }
                else
                {
                    AppDebug.Line("response from get (first 2000 chars): [" + responseString.Substring(0, 500) + "]");
                }
                return response;
            }
            catch (Exception e)
            {
                AppDebug.Exception(e, "Get");
                return null;
            }
        }

        public async Task<HttpResponseMessage> Post(string URL, HttpContent content)
        { // Post to server
            try
            {
                bool isInternetConnected = NetworkInterface.GetIsNetworkAvailable();
                if (!isInternetConnected)
                {
                    AppDebug.Line("Error - No Internet connection!");

                    await new MessageDialog("No internet connection!", "Error!").ShowAsync();
                    return null;
                }
                var response = await client.PostAsync(URL, content);
                var responseString = await response.Content.ReadAsStringAsync();

                AppDebug.Line("response from post: [" + responseString + "]");
                return response;
            }
            catch (Exception e)
            {
                AppDebug.Exception(e, "Post");
                return null;
            }

        }

        public async Task<HttpResponseMessage> Delete(string URL)
        {
            AppDebug.Line("In Delete: " + URL);

            try
            {
                bool isInternetConnected = NetworkInterface.GetIsNetworkAvailable();
                if (!isInternetConnected)
                {
                    AppDebug.Line("Error - No Internet connection!");

                    await new MessageDialog("No internet connection!", "Error!").ShowAsync();
                    return null;
                }

                var response = await client.DeleteAsync(URL).ConfigureAwait(false);
                AppDebug.Line("finished get");

                var responseString = await response.Content.ReadAsStringAsync();

                if (responseString.Length < 2000)
                {
                    AppDebug.Line("response from delete: [" + responseString + "]");
                }
                else
                {
                    AppDebug.Line("response from get (first 2000 chars): [" + responseString.Substring(0, 500) + "]");
                }
                return response;
            }
            catch (Exception e)
            {
                AppDebug.Exception(e, "Get");
                return null;
            }
        }

    }
}
