using System;
using SleepItOff.Entities;
using SleepItOff.Repositories;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace SleepItOff.Cloud.AzureDatabase
{
	public class SleepSegmentsStatsRepository : AbstractAzureRepository, ISleepSegmentsStatsRepository
    {
        public UserSleepSegmentsStatsRecord GetUserSleepSegmentsStats(string userId)
        {
            var userIdParameter = new Parameter(UserIdKey, userId);

            var result = CallAzureDatabase("GetUserSleepSegmentsStats", userIdParameter);
            if (result == null)
                return null;

            return JsonConvert.DeserializeObject<UserSleepSegmentsStatsRecord>(result);
        }

        public List<int> GetUserSleepSegmentsStatsList(string userId)
        {
            var userIdParameter = new Parameter(UserIdKey, userId);

            var result = CallAzureDatabase("GetUserSleepSegmentsStats", userIdParameter);
            if (result == null)
                return null;

            UserSleepSegmentsStatsRecord record = JsonConvert.DeserializeObject<UserSleepSegmentsStatsRecord>(result);
            List<int> averages = new List<int>();
            averages.Add(record.awakeCountTimes); averages.Add(record.awakeTotalDuration); averages.Add(record.awakeToAwakeCount);
            averages.Add(record.awakeToSnoozeCount); averages.Add(record.awakeToDozeCount); averages.Add(record.awakeToRestlessSleepCount);
            averages.Add(record.awakeToRestfulSleepCount); averages.Add(record.awakeToREMCount);

            averages.Add(record.snoozeCountTimes); averages.Add(record.snoozeTotalDuration); averages.Add(record.snoozeToAwakeCount);
            averages.Add(record.snoozeToSnoozeCount); averages.Add(record.snoozeToDozeCount); averages.Add(record.snoozeToRestlessSleepCount);
            averages.Add(record.snoozeToRestfulSleepCount); averages.Add(record.snoozeToREMCount);

            averages.Add(record.dozeCountTimes); averages.Add(record.dozeTotalDuration); averages.Add(record.dozeToAwakeCount);
            averages.Add(record.dozeToSnoozeCount); averages.Add(record.dozeToDozeCount); averages.Add(record.dozeToRestlessSleepCount);
            averages.Add(record.dozeToRestfulSleepCount); averages.Add(record.dozeToREMCount);

            averages.Add(record.restlessSleepCountTimes); averages.Add(record.restlessSleepTotalDuration); averages.Add(record.restlessSleepToAwakeCount);
            averages.Add(record.restlessSleepToSnoozeCount); averages.Add(record.restlessSleepToDozeCount); averages.Add(record.restlessSleepToRestlessSleepCount);
            averages.Add(record.restlessSleepToRestfulSleepCount); averages.Add(record.restlessSleepToREMCount);

            averages.Add(record.restfulSleepCountTimes); averages.Add(record.restfulSleepTotalDuration); averages.Add(record.restfulSleepToAwakeCount);
            averages.Add(record.restfulSleepToSnoozeCount); averages.Add(record.restfulSleepToDozeCount); averages.Add(record.restfulSleepToRestlessSleepCount);
            averages.Add(record.restfulSleepToRestfulSleepCount); averages.Add(record.restfulSleepToREMCount);

            averages.Add(record.REMSleepCountTimes); averages.Add(record.REMSleepTotalDuration); averages.Add(record.REMSleepToAwakeCount);
            averages.Add(record.REMSleepToSnoozeCount); averages.Add(record.REMSleepToDozeCount); averages.Add(record.REMSleepToRestlessSleepCount);
            averages.Add(record.REMSleepToRestfulSleepCount); averages.Add(record.REMSleepToREMCount);

            return averages;
        }

        public string SaveUserSleepSegmentsStats(string UserId, DateTime LastUpdated,
                                             int AwakeCountTimes, int AwakeTotalDuration, int AwakeToAwakeCount,
                                             int AwakeToSnoozeCount, int AwakeToDozeCount, int AwakeToRestlessSleepCount,
                                             int AwakeToRestfulSleepCount, int AwakeToREMCount, int SnoozeCountTimes,
                                             int SnoozeTotalDuration, int SnoozeToAwakeCount, int SnoozeToSnoozeCount,
                                             int SnoozeToDozeCount, int SnoozeToRestlessSleepCount, int SnoozeToRestfulSleepCount,
                                             int SnoozeToREMCount, int DozeCountTimes, int DozeTotalDuration, int DozeToAwakeCount, int DozeToSnoozeCount,
                                             int DozeToDozeCount, int DozeToRestlessSleepCount, int DozeToRestfulSleepCount, int DozeToREMCount,
                                             int RestlessSleepCountTimes, int RestlessSleepTotalDuration, int RestlessSleepToAwakeCount, int RestlessSleepToSnoozeCount,
                                             int RestlessSleepToDozeCount, int RestlessSleepToRestlessSleepCount, int RestlessSleepToRestfulSleepCount,
                                             int RestlessSleepToREMCount, int RestfulSleepCountTimes, int RestfulSleepTotalDuration,
                                             int RestfulSleepToAwakeCount, int RestfulSleepToSnoozeCount, int RestfulSleepToDozeCount,
                                             int RestfulSleepToRestlessSleepCount, int RestfulSleepToRestfulSleepCount, int RestfulSleepToREMCount,
                                             int rEMSleepCountTimes, int rEMSleepTotalDuration, int rEMSleepToAwakeCount,
                                             int rEMSleepToSnoozeCount, int rEMSleepToDozeCount, int rEMSleepToRestlessSleepCount,
                                             int rEMSleepToRestfulSleepCount, int rEMSleepToREMCount)
		{
            return SaveUserSleepSegmentsStats(new UserSleepSegmentsStatsRecord
            {
                userId = UserId,
                lastUpdated = LastUpdated,
                awakeCountTimes = AwakeCountTimes,
                awakeTotalDuration = AwakeTotalDuration,
                awakeToAwakeCount = AwakeToAwakeCount,
                awakeToSnoozeCount = AwakeToSnoozeCount,
                awakeToDozeCount = AwakeToDozeCount,
                awakeToRestlessSleepCount = AwakeToRestlessSleepCount,
                awakeToRestfulSleepCount = AwakeToRestfulSleepCount,
                awakeToREMCount = AwakeToREMCount,

                snoozeCountTimes = SnoozeCountTimes,
                snoozeTotalDuration = SnoozeTotalDuration,
                snoozeToAwakeCount = SnoozeToAwakeCount,
                snoozeToSnoozeCount = SnoozeToSnoozeCount,
                snoozeToDozeCount = SnoozeToDozeCount,
                snoozeToRestlessSleepCount = SnoozeToRestlessSleepCount,
                snoozeToRestfulSleepCount = SnoozeToRestfulSleepCount,
                snoozeToREMCount = SnoozeToREMCount,

                dozeCountTimes = DozeCountTimes,
                dozeTotalDuration = DozeTotalDuration,
                dozeToAwakeCount = DozeToAwakeCount,
                dozeToSnoozeCount = DozeToSnoozeCount,
                dozeToDozeCount = DozeToDozeCount,
                dozeToRestlessSleepCount = DozeToRestlessSleepCount,
                dozeToRestfulSleepCount = DozeToRestfulSleepCount,
                dozeToREMCount = DozeToREMCount,

                restlessSleepCountTimes = RestlessSleepCountTimes,
                restlessSleepTotalDuration = RestlessSleepTotalDuration,
                restlessSleepToAwakeCount = RestlessSleepToAwakeCount,
                restlessSleepToSnoozeCount = RestlessSleepToSnoozeCount,
                restlessSleepToDozeCount = RestlessSleepToDozeCount,
                restlessSleepToRestlessSleepCount = RestlessSleepToRestlessSleepCount,
                restlessSleepToRestfulSleepCount = RestlessSleepToRestfulSleepCount,
                restlessSleepToREMCount = RestlessSleepToREMCount,

                restfulSleepCountTimes = RestfulSleepCountTimes,
                restfulSleepTotalDuration = RestfulSleepTotalDuration,
                restfulSleepToAwakeCount = RestfulSleepToAwakeCount,
                restfulSleepToSnoozeCount = RestfulSleepToSnoozeCount,
                restfulSleepToDozeCount = RestfulSleepToDozeCount,
                restfulSleepToRestlessSleepCount = RestfulSleepToRestlessSleepCount,
                restfulSleepToRestfulSleepCount = RestfulSleepToRestfulSleepCount,
                restfulSleepToREMCount = RestfulSleepToREMCount,

                REMSleepCountTimes = rEMSleepCountTimes,
                REMSleepTotalDuration = rEMSleepTotalDuration,
                REMSleepToAwakeCount = rEMSleepToAwakeCount,
                REMSleepToSnoozeCount = rEMSleepToSnoozeCount,
                REMSleepToDozeCount = rEMSleepToDozeCount,
                REMSleepToRestlessSleepCount = rEMSleepToRestlessSleepCount,
                REMSleepToRestfulSleepCount = rEMSleepToRestfulSleepCount,
                REMSleepToREMCount = rEMSleepToREMCount
            });
		}

        public string SaveUserSleepSegmentsStats(UserSleepSegmentsStatsRecord record)
        {
            var userIdParameter = new Parameter(UserIdKey, record.userId);

            var json = JsonConvert.SerializeObject(record);
            var dataParameter = new Parameter("data", json);

            return CallAzureDatabase("SaveUserSleepSegmentsStats", userIdParameter, dataParameter);
        }

        public string UpdateUserSleepSegmentsStats(string UserId, DateTime LastUpdated,
                                             int AwakeCountTimes, int AwakeTotalDuration, int AwakeToAwakeCount,
                                             int AwakeToSnoozeCount, int AwakeToDozeCount, int AwakeToRestlessSleepCount,
                                             int AwakeToRestfulSleepCount, int AwakeToREMCount, int SnoozeCountTimes,
                                             int SnoozeTotalDuration, int SnoozeToAwakeCount, int SnoozeToSnoozeCount,
                                             int SnoozeToDozeCount, int SnoozeToRestlessSleepCount, int SnoozeToRestfulSleepCount,
                                             int SnoozeToREMCount, int DozeCountTimes, int DozeTotalDuration, int DozeToAwakeCount, int DozeToSnoozeCount,
                                             int DozeToDozeCount, int DozeToRestlessSleepCount, int DozeToRestfulSleepCount, int DozeToREMCount,
                                             int RestlessSleepCountTimes, int RestlessSleepTotalDuration, int RestlessSleepToAwakeCount, int RestlessSleepToSnoozeCount,
                                             int RestlessSleepToDozeCount, int RestlessSleepToRestlessSleepCount, int RestlessSleepToRestfulSleepCount,
                                             int RestlessSleepToREMCount, int RestfulSleepCountTimes, int RestfulSleepTotalDuration,
                                             int RestfulSleepToAwakeCount, int RestfulSleepToSnoozeCount, int RestfulSleepToDozeCount,
                                             int RestfulSleepToRestlessSleepCount, int RestfulSleepToRestfulSleepCount, int RestfulSleepToREMCount,
                                             int rEMSleepCountTimes, int rEMSleepTotalDuration, int rEMSleepToAwakeCount,
                                             int rEMSleepToSnoozeCount, int rEMSleepToDozeCount, int rEMSleepToRestlessSleepCount,
                                             int rEMSleepToRestfulSleepCount, int rEMSleepToREMCount)
        {
            return UpdateUserSleepSegmentsStats(new UserSleepSegmentsStatsRecord
            {
                userId = UserId,
                lastUpdated = LastUpdated,
                awakeCountTimes = AwakeCountTimes,
                awakeTotalDuration = AwakeTotalDuration,
                awakeToAwakeCount = AwakeToAwakeCount,
                awakeToSnoozeCount = AwakeToSnoozeCount,
                awakeToDozeCount = AwakeToDozeCount,
                awakeToRestlessSleepCount = AwakeToRestlessSleepCount,
                awakeToRestfulSleepCount = AwakeToRestfulSleepCount,
                awakeToREMCount = AwakeToREMCount,

                snoozeCountTimes = SnoozeCountTimes,
                snoozeTotalDuration = SnoozeTotalDuration,
                snoozeToAwakeCount = SnoozeToAwakeCount,
                snoozeToSnoozeCount = SnoozeToSnoozeCount,
                snoozeToDozeCount = SnoozeToDozeCount,
                snoozeToRestlessSleepCount = SnoozeToRestlessSleepCount,
                snoozeToRestfulSleepCount = SnoozeToRestfulSleepCount,
                snoozeToREMCount = SnoozeToREMCount,

                dozeCountTimes = DozeCountTimes,
                dozeTotalDuration = DozeTotalDuration,
                dozeToAwakeCount = DozeToAwakeCount,
                dozeToSnoozeCount = DozeToSnoozeCount,
                dozeToDozeCount = DozeToDozeCount,
                dozeToRestlessSleepCount = DozeToRestlessSleepCount,
                dozeToRestfulSleepCount = DozeToRestfulSleepCount,
                dozeToREMCount = DozeToREMCount,

                restlessSleepCountTimes = RestlessSleepCountTimes,
                restlessSleepTotalDuration = RestlessSleepTotalDuration,
                restlessSleepToAwakeCount = RestlessSleepToAwakeCount,
                restlessSleepToSnoozeCount = RestlessSleepToSnoozeCount,
                restlessSleepToDozeCount = RestlessSleepToDozeCount,
                restlessSleepToRestlessSleepCount = RestlessSleepToRestlessSleepCount,
                restlessSleepToRestfulSleepCount = RestlessSleepToRestfulSleepCount,
                restlessSleepToREMCount = RestlessSleepToREMCount,

                restfulSleepCountTimes = RestfulSleepCountTimes,
                restfulSleepTotalDuration = RestfulSleepTotalDuration,
                restfulSleepToAwakeCount = RestfulSleepToAwakeCount,
                restfulSleepToSnoozeCount = RestfulSleepToSnoozeCount,
                restfulSleepToDozeCount = RestfulSleepToDozeCount,
                restfulSleepToRestlessSleepCount = RestfulSleepToRestlessSleepCount,
                restfulSleepToRestfulSleepCount = RestfulSleepToRestfulSleepCount,
                restfulSleepToREMCount = RestfulSleepToREMCount,

                REMSleepCountTimes = rEMSleepCountTimes,
                REMSleepTotalDuration = rEMSleepTotalDuration,
                REMSleepToAwakeCount = rEMSleepToAwakeCount,
                REMSleepToSnoozeCount = rEMSleepToSnoozeCount,
                REMSleepToDozeCount = rEMSleepToDozeCount,
                REMSleepToRestlessSleepCount = rEMSleepToRestlessSleepCount,
                REMSleepToRestfulSleepCount = rEMSleepToRestfulSleepCount,
                REMSleepToREMCount = rEMSleepToREMCount
            });
        }

        public string UpdateUserSleepSegmentsStats(UserSleepSegmentsStatsRecord record)
        {
            var userIdParameter = new Parameter(UserIdKey, record.userId);

            var json = JsonConvert.SerializeObject(record);
            var dataParameter = new Parameter("data", json);

            return CallAzureDatabase("UpdateUserSleepSegmentsStats", userIdParameter, dataParameter);
        }

        // returns null if no a like users
        public List<int> getSleepSegmentsStatsAveragesFromALikeUsersOption1(string _userId, int userAge, string userGender)
        {
            var minAgeParameter = new Parameter(minAgeKey, (userAge - 3).ToString());
            var maxAgeParameter = new Parameter(maxAgeKey, (userAge + 3).ToString());
            var genderParameter = new Parameter(GenderKey, userGender);
            var parameters = new Parameter[3] { genderParameter, minAgeParameter, maxAgeParameter };
            var usersInHisGenderAndAgeRange = CallAzureDatabase("GetUsersByGenderAndAgeRange", parameters);

            UserIdsModel _users = JsonConvert.DeserializeObject<UserIdsModel>(usersInHisGenderAndAgeRange);
            if (_users.userIds.Count == 0) { return null; }

            StringBuilder sb = new StringBuilder();
            foreach (string s in _users.userIds)
            {
                sb.Append(s + ",");
            }
            sb = sb.Remove(sb.Length - 1, 1); //remove the last comma
            string users = sb.ToString();

            var usersParam = new Parameter("users", users);

            var sleepSegmentsStatsAverages = CallAzureDatabase("GetAverageSleepSegmentsStatsFromUsers", usersParam);
            if(sleepSegmentsStatsAverages == null) { return null; }

            List<int> averages = JsonConvert.DeserializeObject<List<int>>(sleepSegmentsStatsAverages);
            return averages;
        }

        // returns null if no a like users
        public List<int> getSleepSegmentsStatsAveragesFromALikeUsersOption2(string _userId, int userHeight, int userWeight, string userGender)
        {
            var HeightParameter = new Parameter(HeightKey, userHeight.ToString());
            var WeightParameter = new Parameter(WeightKey, userWeight.ToString());
            var genderParameter = new Parameter(GenderKey, userGender);
            var parameters = new Parameter[3] { genderParameter, HeightParameter, WeightParameter };
            var usersInHisGenderHeightAndWeight = CallAzureDatabase("GetUsersByGenderHeightAndWeight", parameters);

            UserIdsModel _users = JsonConvert.DeserializeObject<UserIdsModel>(usersInHisGenderHeightAndWeight);
            if (_users.userIds.Count == 0) { return null; }

            StringBuilder sb = new StringBuilder();
            foreach (string s in _users.userIds)
            {
                sb.Append(s + ",");
            }
            sb = sb.Remove(sb.Length - 1, 1); //remove the last comma
            string users = sb.ToString();

            var usersParam = new Parameter("users", users);

            var sleepSegmentsStatsAverages = CallAzureDatabase("GetAverageSleepSegmentsStatsFromUsers", usersParam);
            if (sleepSegmentsStatsAverages == null) { return null; }

            List<int> averages = JsonConvert.DeserializeObject<List<int>>(sleepSegmentsStatsAverages);
            return averages;
        }

        // returns null if no a like users
        public List<int> getSleepSegmentsStatsAveragesFromALikeUsersCombined(string _userId, int userAge, int userHeight, int userWeight, string userGender)
        {
            UserDetailsRepository repo = new UserDetailsRepository();
            UserIdsModel r = repo.GetUsersByGenderHeightWeightAndAgeRange(userAge, userHeight, userWeight, userGender);
            if (r.userIds.Count == 0) { return null; }

            StringBuilder sbs = new StringBuilder();
            foreach (string s in r.userIds)
            {
                sbs.Append(s + ",");
            }
            sbs = sbs.Remove(sbs.Length - 1, 1); //remove the last comma
            string users = sbs.ToString();

            var usersParam = new Parameter("users", users);

            var sleepSegmentsStatsAverages = CallAzureDatabase("GetAverageSleepSegmentsStatsFromUsers", usersParam);
            if (sleepSegmentsStatsAverages == null) { return null; }

            List<int> averages = JsonConvert.DeserializeObject<List<int>>(sleepSegmentsStatsAverages);
            return averages;
        }
    }
}