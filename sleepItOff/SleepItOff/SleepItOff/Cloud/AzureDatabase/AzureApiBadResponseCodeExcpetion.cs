using System;
using System.Net;

namespace SleepItOff.Cloud.AzureDatabase
{
	public class AzureApiBadResponseCodeExcpetion : Exception
	{
		public AzureApiBadResponseCodeExcpetion(HttpStatusCode statusCode, string content) :
			base($"Bad Azure API response - {statusCode} - {content}")
		{
		}

		public AzureApiBadResponseCodeExcpetion(Exception e) :
			base("Exception in Azure API call", e)
		{
		}
	}
}