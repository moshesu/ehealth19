using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using System.Data.SqlClient;
using Microsoft.Azure.WebJobs.Extensions.Http;
using SleepItOff.SleepItOffDBFunction.Database;
using Microsoft.Extensions.Logging;

namespace SleepItOff.SleepItOffDBFunction
{
	public static class SleepItOffDatabase
	{
		[FunctionName("SleepItOffDatabase")]
		public static HttpResponseMessage Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestMessage req, ILogger log)
		{
			string action = AbstractRequest.GetParameter(req, "action");

			switch (action)
			{
				/* Single requests */

				case "SaveUser":
					return new ManageUsers().Save(req, log);

				case "GetUser":
					return new ManageUsers().Get(req, log);

				case "RemoveUser":
					return new ManageUsers().Remove(req, log);

                case "SaveUserDetails":
                    return new UserDetails().Save(req, log);

                case "GetUserDetails":
                    return new UserDetails().Get(req, log);

                case "GetUsersByGender":
                    return new UserDetails().GetUserIdsByGender(req, log);

                case "GetUsersByAgeRange":
                    return new UserDetails().GetUserIdsByAgeRange(req, log);

                case "GetUsersByGenderAndAgeRange":
                    return new UserDetails().GetUserIdsByGenderAndAgeRange(req, log);

                case "GetUsersByGenderHeightAndWeight":
                    return new UserDetails().GetUserIdsByGenderHeightAndWeight(req, log);

                case "GetUsersByGenderHeightWeightAndAgeRange":
                    return new UserDetails().GetUserIdsByGenderHeightWeightAndAgeRange(req, log);

                case "RemoveUserDetails":
                    return new UserDetails().Remove(req, log);

                case "GetUserAwakeningAccuracy":
                    return new AwakeningAccuracy().Get(req, log);

                case "SaveUserAwakeningAccuracy":
					return new AwakeningAccuracy().Save(req, log);

                case "GetUserSleepSegmentsStats":
                    return new SleepSegmentsStats().Get(req, log);

                case "SaveUserSleepSegmentsStats":
                    return new SleepSegmentsStats().Save(req, log);

                case "UpdateUserSleepSegmentsStats":
                    return new SleepSegmentsStats().Update(req, log);

                case "GetAverageSleepSegmentsStatsFromUsers":
                    return new SleepSegmentsStats().GetAverageSleepSegmentsStatsFromUsers(req, log);

                case "GetUserSleepQuality":
                    return new SleepQuality().Get(req, log);

                case "SaveUserSleepQuality":
                    return new SleepQuality().Save(req, log);

                case "UpdateUserSleepQuality":
                    return new SleepQuality().Update(req, log);

                case "GetIsUserInTopTenUsersBySleepEfficiency":
                    return new SleepQuality().GetIsUserInTopTenUsersBySleepEfficiency(req, log);

                case "GetIsUserInTopFiveUsersBySleepEfficiency":
                    return new SleepQuality().GetIsUserInTopFiveUsersBySleepEfficiency(req, log);

                case "GetIsUserInTopTenUsersByWakeUps":
                    return new SleepQuality().GetIsUserInTopTenUsersByWakeUps(req, log);

                case "GetIsUserInTopFiveUsersByWakeUps":
                    return new SleepQuality().GetIsUserInTopFiveUsersByWakeUps(req, log);

                case "GetAverageWakeUpsForUsers":
                    return new SleepQuality().GetAverageWakeUpsForUsers(req, log);

                case "GetAverageSleepEfficiencyForUsers":
                    return new SleepQuality().GetAverageSleepEfficiencyForUsers(req, log);

                default:
					return req.CreateResponse(HttpStatusCode.BadRequest, "Unknown action");
			}
		}

	}
}