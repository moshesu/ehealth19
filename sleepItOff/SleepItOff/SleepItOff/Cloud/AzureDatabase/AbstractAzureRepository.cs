using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Converters;

namespace SleepItOff.Cloud.AzureDatabase
{
    
	public abstract class AbstractAzureRepository
	{
        private const string Host = "https://sleepitoffdbfunction20181221043350.azurewebsites.net/api"; //Production

        private readonly string DatabaseUrl = $"{Host}/SleepItOffDatabase";
        private const string DatabaseCode = "53GO6koFQpujmWaHo3OBbop00Ls5C9p4SqY8Tf6TCVVJ340tkigQ4g==";

        protected const string UserIdKey = "userId";
        protected const string GenderKey = "gender";
        protected const string HeightKey = "height";
        protected const string WeightKey = "weight";
        protected const string AgeKey = "age";
        protected const string minAgeKey = "min";
        protected const string maxAgeKey = "max";
        protected const string DateFormat = "yyyy-MM-ddTHH\\:mm\\:ss";
		public IsoDateTimeConverter DateTimeConverter = new IsoDateTimeConverter {DateTimeFormat = DateFormat};

		public string CallAzureDatabase(string action, params Parameter[] externalParameters)
		{
			return CallAzureFunction(DatabaseUrl, action, DatabaseCode, externalParameters);
		}

		private string CallAzureFunction(string url, string action, string code, params Parameter[] externalParameters)
		{
			try
			{
				var ownParameters = new[]
				{
					new Parameter("code", code),
					new Parameter("action", action)
				};

				var parameters = ownParameters.Concat(externalParameters);
				var headers = parameters.Select(p => $"{p.Key}={p.Value}").ToArray();

				var ub = new UriBuilder(url)
				{
					Query = string.Join("&", headers)
				};

                using (var http = new HttpClient())
                //using (var resp = http.GetAsync(ub.Uri).AsTask().ConfigureAwait(false).GetAwaiter().GetResult())
                // TODO: figure out why AsTask() not working
                using (var resp = http.GetAsync(ub.Uri).ConfigureAwait(false).GetAwaiter().GetResult())
                {
                    if (resp.IsSuccessStatusCode)
                        return resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (resp.StatusCode == HttpStatusCode.NotFound)
                        return null;

                    throw new AzureApiBadResponseCodeExcpetion(resp.StatusCode, resp.Content.ToString());
                }
			}

			catch (Exception e)
			{
				throw new AzureApiBadResponseCodeExcpetion(e);
			}
		}
	}

	public class Parameter
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public Parameter(string key, string value)
		{
			Key = key;
			Value = value;
		}

	} 
}