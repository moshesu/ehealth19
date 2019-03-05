using System;
using SleepItOff.Entities;
using SleepItOff.Repositories;
using Newtonsoft.Json;
using System.Text;

namespace SleepItOff.Cloud.AzureDatabase
{
	public class ManageUseresRepository : AbstractAzureRepository, IManageUsersRepository
	{

        public UserRecord GetUser(string userId)
        {
            var userIdParameter = new Parameter(UserIdKey, userId);

            var result = CallAzureDatabase("GetUser", userIdParameter);
            if (result == null)
                return null;

            return JsonConvert.DeserializeObject<UserRecord>(result);
        }

		public string SaveUser(string UserId, DateTime LastUpdated)
		{
            return SaveUser(new UserRecord
            {
                userId = UserId,
                lastUpdated = LastUpdated
            });
		}

        public string SaveUser(UserRecord record)
        {
            var userIdParameter = new Parameter(UserIdKey, record.userId);
            var json = JsonConvert.SerializeObject(record);
            var dataParameter = new Parameter("data", json);

            return CallAzureDatabase("SaveUser", userIdParameter, dataParameter);
        }
	}
}