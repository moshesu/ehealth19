using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Microsoft.Band.Sensors;   // For UVI

namespace UVapp
{

    class WeatherUV
    {
        // This class retrieves uv from a weather API

        private static readonly string weatherApiUri = "https://api.openuv.io/api/v1/uv";
        private static readonly string weatherApiAccessToken = "9cc480118cfbb8a00b6c7880b9394204";

        /*  // The previously used API
        struct uvApiResponse
        {
            public double lat;
            public double lon;
            public string date_iso;
            public long date;
            public double value;
        }
        */


        /* The class corresponds to the JSON structure
         * All strings are ISO 8601 times in UTC, e.g. "2019-01-31T12:15:34.451Z"
         */
        public class UvApiResponse
        {
            public UvApiResponseResult result;

            public class UvApiResponseResult
            {
                public double uv;
                public string uv_time;
                public double uv_max;
                public string uv_max_time;
                public double ozone;
                public string ozone_time;
                public SafeExposureTimes safe_exposure_times;
                public class SafeExposureTimes
                {
                    public int st1;
                    public int st2;
                    public int st3;
                    public int st4;
                    public int st5;
                    public int st6;
                }

                public SunInfo sun_info;

                public class SunInfo
                {
                    public SunTimes sun_times;

                    public class SunTimes
                    {
                        public string solarNoon;
                        public string nadir;
                        public string sunrise;
                        public string sunset;
                        public string sunriseEnd;
                        public string sunriseStart;
                        public string dawn;
                        public string dusk;
                        public string nauticalDawn;
                        public string nauticalDusk;
                        public string nightEnd;
                        public string night;
                        public string goldenHourEnd;
                        public string goldenHour;
                    }

                    public SunPosition sun_position;

                    public class SunPosition
                    {
                        double azimuth;
                        double altitude;
                    }
                }
            } 
        }
        
        

        public static async Task<double> GetWeatherUvAsync(HttpClient httpClient, double locationLatitude, double locationLongitude)
        {
            Uri requestUri = new Uri(weatherApiUri + $"?&lat={locationLatitude}&lng={locationLongitude}");
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            requestMessage.Headers.Add("x-access-token", weatherApiAccessToken);
            HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var deserialized = JsonConvert.DeserializeObject<UvApiResponse>(content);
                return deserialized.result.uv;
            }
            else
            {
                return -1;
            }
        }

        /**
         * Compares band and weather UV and returns a good compromise
         */
        public static double CompareBandAndWeatherUV(UVIndexLevel bandUvEnum, double weatherUV)
        {
            if (weatherUV == -1)
                return UVvalues.UvEnumToIntUpper(bandUvEnum);

            if (UVvalues.UvIntToEnum((int)weatherUV) == bandUvEnum)
            {
                return weatherUV;
            }
            else if (UVvalues.UvIntToEnum((int)weatherUV).Ordinal() < bandUvEnum.Ordinal()) // Weather value < band measured range
            {
                return UVvalues.UvEnumToIntLower(bandUvEnum);
            }
            else // Weather value > band measured range
            {
                return UVvalues.UvEnumToIntUpper(bandUvEnum);
            }
        }
    }
}