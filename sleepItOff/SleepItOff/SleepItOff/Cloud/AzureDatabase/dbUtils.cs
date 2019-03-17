using SleepItOff.Entities;
using System;
using System.Collections.Generic;
using static SleepItOff.Cloud.AzureDatabase.SleepQualityRepository;

namespace SleepItOff.Cloud.AzureDatabase
{
    class dbUtils
    {
        /*
        * after signing in to the application, check if the user already in the db and add him if not
        * in this point we have the mshealth profile information of the user 
        * from there we can retrieve the userId
        */
        public static bool assertUserInDB(string UserId)
        {
            var _manageUsersRepository = new ManageUseresRepository();

            try
            {
                var userRecord = _manageUsersRepository.GetUser(UserId);
                if (userRecord == null)
                    return false;
                else
                    return true;                
            }
            catch (Exception e)
            {
                throw new AzureApiBadResponseCodeExcpetion(e);
            }
        }

        public static void addUserToDB(UserRecord user, UserDetailsRecord userDetails)
        {
            addUserToManageUsersTable(user);
            addUserToUserDetailsTable(userDetails);
        }

        public static void addUserToManageUsersTable(UserRecord user)
        {
            try
            {
                var _manageUsersRepository = new ManageUseresRepository();
                var userRecord = _manageUsersRepository.SaveUser(user.userId, DateTime.Now);
                if (userRecord == null)
                {
                    throw new Exception("Error inserting user to ManageUsers table");
                }
                return;
            }
            catch (Exception e)
            {
                throw new AzureApiBadResponseCodeExcpetion(e);
            }
        }

        public static void addUserToUserDetailsTable(UserDetailsRecord userDetails)
        {
            try
            {
                var _userDetailsRepository = new UserDetailsRepository();
                var userRecord = _userDetailsRepository.SaveUserDetails(userDetails.userId, 
                                                                        userDetails.firstName, 
                                                                        userDetails.lastName, 
                                                                        userDetails.gender,
                                                                        userDetails.age, 
                                                                        userDetails.height, 
                                                                        userDetails.weight);
                if (userRecord == null)
                {
                    throw new Exception("Error inserting user details to UserDetails table");
                }
                return;
            }
            catch (Exception e)
            {
                throw new AzureApiBadResponseCodeExcpetion(e);
            }
        }

        public static void updateUserSleepQualityDetails(UserSleepQualityRecord record)
        {
            try
            {
                var _userSleepQualityRepository = new SleepQualityRepository();
                var sleepQualityRecord = _userSleepQualityRepository.UpdateUserSleepQuality(record.userId,
                                                                        record.averageWakeUps,
                                                                        record.averageSleepEfficiency);
                if (sleepQualityRecord == null)
                {
                    throw new Exception("Error updating user sleep quality records in SleepQuality table");
                }
                return;
            }
            catch (Exception e)
            {
                throw new AzureApiBadResponseCodeExcpetion(e);
            }
        }

        
        public static sleepQuality doesTheUserSleepWellByAgeAndGender(string _userId, int _age, string _gender)
        {
            var repo = new SleepQualityRepository();
            (sleepEfficiencyQuality, countWakeUpsQuality) quality = repo.checkUserSleepQualityForHisGenderAndAge(_userId, _age, _gender);
            if ((quality.Item1 == sleepEfficiencyQuality.Good && quality.Item2 != countWakeUpsQuality.Bad) ||
                (quality.Item1 != sleepEfficiencyQuality.Bad && quality.Item2 == countWakeUpsQuality.Good))
            {
                return sleepQuality.Good;
            }
            else if (quality.Item1 == sleepEfficiencyQuality.Medium && quality.Item2 == countWakeUpsQuality.Medium)
            {
                return sleepQuality.Medium;
            }
            return sleepQuality.Bad;
        }


        public static sleepQuality doesTheUserSleepWellByAge(string _userId, int _age)
        {
            var repo = new SleepQualityRepository();
            (sleepEfficiencyQuality, countWakeUpsQuality) quality = repo.checkUserSleepQualityForHisAge(_userId, _age);
            if((quality.Item1 == sleepEfficiencyQuality.Good && quality.Item2 != countWakeUpsQuality.Bad) ||
                (quality.Item1 != sleepEfficiencyQuality.Bad && quality.Item2 == countWakeUpsQuality.Good))
            {
                return sleepQuality.Good;
            }
            else if (quality.Item1 == sleepEfficiencyQuality.Medium && quality.Item2 == countWakeUpsQuality.Medium)
            {
                return sleepQuality.Medium;
            }
            return sleepQuality.Bad;
        }

        public static sleepQuality doesTheUserSleepWellByGender(string _userId, string _gender)
        {
            var repo = new SleepQualityRepository();
            (sleepEfficiencyQuality, countWakeUpsQuality) quality = repo.checkUserSleepQualityForHisGender(_userId, _gender);
            if ((quality.Item1 == sleepEfficiencyQuality.Good && quality.Item2 != countWakeUpsQuality.Bad) ||
                (quality.Item1 != sleepEfficiencyQuality.Bad && quality.Item2 == countWakeUpsQuality.Good))
            {
                return sleepQuality.Good;
            }
            else if (quality.Item1 == sleepEfficiencyQuality.Medium && quality.Item2 == countWakeUpsQuality.Medium)
            {
                return sleepQuality.Medium;
            }
            return sleepQuality.Bad;
        }

        //possible that null will be returned from this function if no a like users found
        public static List<int> defineUserSegmentsTransitionsProbabilitiesOption1(string _userId, int _age, string _gender, float userDataWeight)
        {
            SleepSegmentsStatsRepository repo = new SleepSegmentsStatsRepository();
            List<int> userStats = repo.GetUserSleepSegmentsStatsList(_userId);
            List<int> aLikeUsersAverages = repo.getSleepSegmentsStatsAveragesFromALikeUsersOption1(_userId, _age, _gender);
            if (aLikeUsersAverages == null) { return null; }

            List<int> res = new List<int>();
            float aLikeUsersDataWeight = 1 - userDataWeight;
            for(int i=0; i<userStats.Count; i++)
            {
                res.Add((int)(userDataWeight*userStats[i]) + (int)(aLikeUsersDataWeight*aLikeUsersAverages[i]));
            }
            return res;
        }

        //possible that null will be returned from this function if no a like users found
        public static List<int> defineUserSegmentsTransitionsProbabilitiesOption2(string _userId, int _height, int _weight, string _gender, float userDataWeight)
        {
            SleepSegmentsStatsRepository repo = new SleepSegmentsStatsRepository();
            List<int> userStats = repo.GetUserSleepSegmentsStatsList(_userId);
            List<int> aLikeUsersAverages = repo.getSleepSegmentsStatsAveragesFromALikeUsersOption2(_userId, _height, _weight, _gender);
            if(aLikeUsersAverages == null) { return null; }

            List<int> res = new List<int>();
            float aLikeUsersDataWeight = 1 - userDataWeight;
            for (int i = 0; i < userStats.Count; i++)
            {
                res.Add((int)(userDataWeight * userStats[i]) + (int)(aLikeUsersDataWeight * aLikeUsersAverages[i]));
            }
            return res;
        }

        //possible that null will be returned from this function if no a like users found
        public static List<int> defineUserSegmentsTransitionsProbabilitiesCombined(string _userId, int _age, int _height, int _weight, string _gender, float userDataWeight)
        {
            SleepSegmentsStatsRepository repo = new SleepSegmentsStatsRepository();
            List<int> userStats = repo.GetUserSleepSegmentsStatsList(_userId);
            List<int> aLikeUsersAverages = repo.getSleepSegmentsStatsAveragesFromALikeUsersCombined(_userId, _age, _height, _weight, _gender);
            if (aLikeUsersAverages == null) { return null; }

            List<int> res = new List<int>();
            float aLikeUsersDataWeight = 1 - userDataWeight;
            for (int i = 0; i < userStats.Count; i++)
            {
                res.Add((int)(userDataWeight * userStats[i]) + (int)(aLikeUsersDataWeight * aLikeUsersAverages[i]));
            }
            return res;
        }

        //possible that null will be returned from this function if no a like users found
        public static List<int> getSleepSegmentsStatsAverages(string _userId, int _age, string _gender)
        {
            SleepSegmentsStatsRepository repo = new SleepSegmentsStatsRepository();
            return repo.getSleepSegmentsStatsAveragesFromALikeUsersOption1(_userId, _age, _gender);
        }


        /* utils */

        public enum sleepQuality
        {
            Bad = 0,
            Medium = 1,
            Good = 2
        }

    }
}
