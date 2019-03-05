using System;
using SleepItOff.Entities;
using SleepItOff.Repositories;
using Newtonsoft.Json;

namespace SleepItOff.Cloud.AzureDatabase
{
	public class AwakeningAccuracyRepository : AbstractAzureRepository, IAwakeningAccuracyRepository
    {
        public UserAwakeningAccuracyRecord GetUserAwakeningAccuracy(string userId)
        {
            var userIdParameter = new Parameter(UserIdKey, userId);

            var result = CallAzureDatabase("GetUserAwakeningAccuracy", userIdParameter);
            if (result == null)
                return null;

            return JsonConvert.DeserializeObject<UserAwakeningAccuracyRecord>(result);
        }

		public string SaveUserAwakeningAccuracy(string UserId, int SleepType, DateTime DefinedAlarmTime, 
                                                DateTime ActualAlarmTime, TimeSpan TimeDifference)
		{
            return SaveUserAwakeningAccuracy(new UserAwakeningAccuracyRecord
            {
                userId = UserId,
                sleepType = SleepType,
				definedAlarmTime = DefinedAlarmTime,
                actualAlarmTime = ActualAlarmTime,
                timeDifference = TimeDifference
            });
		}

        public string SaveUserAwakeningAccuracy(UserAwakeningAccuracyRecord record)
        {
            var userIdParameter = new Parameter(UserIdKey, record.userId);

            var json = JsonConvert.SerializeObject(record);
            var dataParameter = new Parameter("data", json);

            return CallAzureDatabase("SaveUserAwakeningAccuracy", userIdParameter, dataParameter);
        }
	}
}