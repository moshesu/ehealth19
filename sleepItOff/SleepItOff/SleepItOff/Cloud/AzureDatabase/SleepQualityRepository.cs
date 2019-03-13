using System;
using SleepItOff.Entities;
using SleepItOff.Repositories;
using Newtonsoft.Json;
using System.Text;

namespace SleepItOff.Cloud.AzureDatabase
{
	public class SleepQualityRepository : AbstractAzureRepository, ISleepQualityRepository
    {
        public UserSleepQualityRecord GetUserSleepQuality(string userId)
        {
            var userIdParameter = new Parameter(UserIdKey, userId);

            var result = CallAzureDatabase("GetUserSleepQuality", userIdParameter);
            if (result == null)
                return null;

            return JsonConvert.DeserializeObject<UserSleepQualityRecord>(result);
        }

		public string SaveUserSleepQuality(string UserId, int AverageWakeUps, int AverageSleepEfficiency)
		{
            return SaveUserSleepQuality(new UserSleepQualityRecord
            {
                userId = UserId,
                averageWakeUps = AverageWakeUps,
                averageSleepEfficiency = AverageSleepEfficiency
            });
		}

        public string SaveUserSleepQuality(UserSleepQualityRecord record)
        {
            var userIdParameter = new Parameter(UserIdKey, record.userId);

            var json = JsonConvert.SerializeObject(record);
            var dataParameter = new Parameter("data", json);

            return CallAzureDatabase("SaveUserSleepQuality", userIdParameter, dataParameter);
        }

        public string UpdateUserSleepQuality(string UserId, int AverageWakeUps, int AverageSleepEfficiency)
        {
            return UpdateUserSleepQuality(new UserSleepQualityRecord
            {
                userId = UserId,
                averageWakeUps = AverageWakeUps,
                averageSleepEfficiency = AverageSleepEfficiency
            });
        }

        public string UpdateUserSleepQuality(UserSleepQualityRecord record)
        {
            var userIdParameter = new Parameter(UserIdKey, record.userId);

            var json = JsonConvert.SerializeObject(record);
            var dataParameter = new Parameter("data", json);

            return CallAzureDatabase("UpdateUserSleepQuality", userIdParameter, dataParameter);
        }

        //TODO: add button that by clicking on it will return this result
        public (sleepEfficiencyQuality, countWakeUpsQuality) checkRelativeUserSleepQuality(string _userId)
        {
            var userIdParameter = new Parameter(UserIdKey, _userId);
            var inSleepEfficiencyTopTen = CallAzureDatabase("GetIsUserInTopTenUsersBySleepEfficiency", userIdParameter);
            var inSleepEfficiencyTopFive = CallAzureDatabase("GetIsUserInTopFiveUsersBySleepEfficiency", userIdParameter);
            var inWakeUpsTopTen = CallAzureDatabase("GetIsUserInTopTenUsersByWakeUps", userIdParameter);
            var inWakeUpsTopFive = CallAzureDatabase("GetIsUserInTopFiveUsersByWakeUps", userIdParameter);
            sleepEfficiencyQuality sleepEfficiencyResult;
            countWakeUpsQuality wakeUpsResult;
            
            if (inSleepEfficiencyTopFive == null)
            {
                if(inSleepEfficiencyTopTen == null) sleepEfficiencyResult = sleepEfficiencyQuality.Bad;
                else sleepEfficiencyResult = sleepEfficiencyQuality.Medium;
            }
            else sleepEfficiencyResult = sleepEfficiencyQuality.Good;

            if (inWakeUpsTopFive == null)
            {
                if (inWakeUpsTopTen == null) wakeUpsResult = countWakeUpsQuality.Good;
                else wakeUpsResult = countWakeUpsQuality.Medium;
            }
            else wakeUpsResult = countWakeUpsQuality.Bad;

            return (sleepEfficiencyResult, wakeUpsResult);
        }

        public (sleepEfficiencyQuality, countWakeUpsQuality) checkUserSleepQualityForHisAge(string _userId, int _userAge)
        {
            UserSleepQualityRecord userSleepQuality = new SleepQualityRepository().GetUserSleepQuality(_userId);

            var minAgeParameter = new Parameter(minAgeKey, (_userAge - 5).ToString());
            var maxAgeParameter = new Parameter(maxAgeKey, (_userAge + 5).ToString());
            var parameters = new Parameter[2] { minAgeParameter, maxAgeParameter };
            var usersInHisAgeRange = CallAzureDatabase("GetUsersByAgeRange", parameters);

            UserIdsModel _users = JsonConvert.DeserializeObject<UserIdsModel>(usersInHisAgeRange);
            StringBuilder sb = new StringBuilder();
            foreach(string s in _users.userIds)
            {
                sb.Append(s + ",");
            }
            sb = sb.Remove(sb.Length - 1,1); //remove the last comma
            string users = sb.ToString();

            var usersParam = new Parameter("users", users);

            var _usersAverageWakeUps = CallAzureDatabase("GetAverageWakeUpsForUsers", usersParam);
            double usersAverageWakeUps = Convert.ToDouble(_usersAverageWakeUps);

            var _usersAverageSleepEfficiency = CallAzureDatabase("GetAverageSleepEfficiencyForUsers", usersParam);
            double usersAverageSleepEfficiency = Convert.ToDouble(_usersAverageSleepEfficiency);

            // set wakeUps quality for the user
            countWakeUpsQuality wakeUpsResult;
            if (userSleepQuality.averageWakeUps >= usersAverageWakeUps) wakeUpsResult = countWakeUpsQuality.Good;
            else
            {
                if(userSleepQuality.averageWakeUps < usersAverageWakeUps - 2) wakeUpsResult = countWakeUpsQuality.Bad;
                else wakeUpsResult = countWakeUpsQuality.Medium;
            }

            // set sleepEfficiency quality for the user
            sleepEfficiencyQuality sleepEfficiencyResult;
            if (userSleepQuality.averageSleepEfficiency >= usersAverageSleepEfficiency) sleepEfficiencyResult = sleepEfficiencyQuality.Good;
            else
            {
                if (userSleepQuality.averageSleepEfficiency < usersAverageSleepEfficiency - 4) sleepEfficiencyResult = sleepEfficiencyQuality.Bad;
                else sleepEfficiencyResult = sleepEfficiencyQuality.Medium;
            }

            return (sleepEfficiencyResult, wakeUpsResult);
        }

        public (sleepEfficiencyQuality, countWakeUpsQuality) checkUserSleepQualityForHisGender(string _userId, string _userGender)
        {
            UserSleepQualityRecord userSleepQuality = new SleepQualityRepository().GetUserSleepQuality(_userId);

            var genderParameter = new Parameter(GenderKey, _userGender);
            var usersInHisGender = CallAzureDatabase("GetUsersByGender", genderParameter);

            UserIdsModel _users = JsonConvert.DeserializeObject<UserIdsModel>(usersInHisGender);
            StringBuilder sb = new StringBuilder();
            foreach (string s in _users.userIds)
            {
                sb.Append(s + ",");
            }
            sb = sb.Remove(sb.Length - 1, 1); //remove the last comma
            string users = sb.ToString();

            var usersParam = new Parameter("users", users);

            var _usersAverageWakeUps = CallAzureDatabase("GetAverageWakeUpsForUsers", usersParam);
            double usersAverageWakeUps = Convert.ToDouble(_usersAverageWakeUps);

            var _usersAverageSleepEfficiency = CallAzureDatabase("GetAverageSleepEfficiencyForUsers", usersParam);
            double usersAverageSleepEfficiency = Convert.ToDouble(_usersAverageSleepEfficiency);

            // set wakeUps quality for the user
            countWakeUpsQuality wakeUpsResult;
            if (userSleepQuality.averageWakeUps >= usersAverageWakeUps) wakeUpsResult = countWakeUpsQuality.Good;
            else
            {
                if (userSleepQuality.averageWakeUps < usersAverageWakeUps - 2) wakeUpsResult = countWakeUpsQuality.Bad;
                else wakeUpsResult = countWakeUpsQuality.Medium;
            }

            // set sleepEfficiency quality for the user
            sleepEfficiencyQuality sleepEfficiencyResult;
            if (userSleepQuality.averageSleepEfficiency >= usersAverageSleepEfficiency) sleepEfficiencyResult = sleepEfficiencyQuality.Good;
            else
            {
                if (userSleepQuality.averageSleepEfficiency < usersAverageSleepEfficiency - 4) sleepEfficiencyResult = sleepEfficiencyQuality.Bad;
                else sleepEfficiencyResult = sleepEfficiencyQuality.Medium;
            }

            return (sleepEfficiencyResult, wakeUpsResult);
        }

        public (sleepEfficiencyQuality, countWakeUpsQuality) checkUserSleepQualityForHisGenderAndAge(string _userId, int _userAge, string _userGender)
        {
            UserSleepQualityRecord userSleepQuality = new SleepQualityRepository().GetUserSleepQuality(_userId);

            var genderParameter = new Parameter(GenderKey, _userGender);
            var minAgeParameter = new Parameter(minAgeKey, (_userAge - 5).ToString());
            var maxAgeParameter = new Parameter(maxAgeKey, (_userAge + 5).ToString());
            var parameters = new Parameter[3] { genderParameter, minAgeParameter, maxAgeParameter };
            var usersInHisGenderAndAgeRange = CallAzureDatabase("GetUsersByGenderAndAgeRange", parameters);

            UserIdsModel _users = JsonConvert.DeserializeObject<UserIdsModel>(usersInHisGenderAndAgeRange);
            StringBuilder sb = new StringBuilder();
            foreach (string s in _users.userIds)
            {
                sb.Append(s + ",");
            }
            sb = sb.Remove(sb.Length - 1, 1); //remove the last comma
            string users = sb.ToString();

            var usersParam = new Parameter("users", users);

            var _usersAverageWakeUps = CallAzureDatabase("GetAverageWakeUpsForUsers", usersParam);
            double usersAverageWakeUps = Convert.ToDouble(_usersAverageWakeUps);

            var _usersAverageSleepEfficiency = CallAzureDatabase("GetAverageSleepEfficiencyForUsers", usersParam);
            double usersAverageSleepEfficiency = Convert.ToDouble(_usersAverageSleepEfficiency);

            // set wakeUps quality for the user
            countWakeUpsQuality wakeUpsResult;
            if (userSleepQuality.averageWakeUps >= usersAverageWakeUps) wakeUpsResult = countWakeUpsQuality.Good;
            else
            {
                if (userSleepQuality.averageWakeUps < usersAverageWakeUps - 2) wakeUpsResult = countWakeUpsQuality.Bad;
                else wakeUpsResult = countWakeUpsQuality.Medium;
            }

            // set sleepEfficiency quality for the user
            sleepEfficiencyQuality sleepEfficiencyResult;
            if (userSleepQuality.averageSleepEfficiency >= usersAverageSleepEfficiency + 10) sleepEfficiencyResult = sleepEfficiencyQuality.Good;
            else
            {
                if (userSleepQuality.averageSleepEfficiency <= usersAverageSleepEfficiency - 10) sleepEfficiencyResult = sleepEfficiencyQuality.Bad;
                else sleepEfficiencyResult = sleepEfficiencyQuality.Medium;
            }

            return (sleepEfficiencyResult, wakeUpsResult);
        }

        public int calculateQualityLevel(int userSleepEfficiency, int userWakeupsCount)
        {
            if ((userSleepEfficiency == 2 && userWakeupsCount != 0) ||
                (userSleepEfficiency != 0 && userWakeupsCount == 2))
            {
                // good sleep quality relatively to other users with the same gender
                return 2;
            }
            else if ((userSleepEfficiency == 2 && userWakeupsCount == 0) ||
                (userSleepEfficiency == 0 && userWakeupsCount == 2) ||
                (userSleepEfficiency == 1 && userWakeupsCount == 1))
            {
                // medium sleep quality relatively to other users with the same gender
                return 1;
            }
            else
            {
                // bad sleep quality relatively to other users with the same gender
                return 0;
            }
        }

        public enum sleepEfficiencyQuality
        {
            Bad = 0,
            Medium = 1,
            Good = 2
        }

        public enum countWakeUpsQuality
        {
            Bad = 0,
            Medium = 1,
            Good = 2
        }
    }
}