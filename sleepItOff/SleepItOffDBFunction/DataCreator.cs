using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace SleepItOff.SleepItOffDBFunction
{
	public static class DataCreator
	{
		[FunctionName("DataCreator")]
		public static HttpResponseMessage Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestMessage req, ILogger log)
		{
			//ComparisonUserCreator.CreateUsers();
			return req.CreateResponse(HttpStatusCode.OK);
		}
	}
}