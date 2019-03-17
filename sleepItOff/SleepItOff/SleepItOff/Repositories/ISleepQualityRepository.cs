using SleepItOff.Entities;
using System;

namespace SleepItOff.Repositories
{
	public interface ISleepQualityRepository
	{
        UserSleepQualityRecord GetUserSleepQuality(string userId);
		string SaveUserSleepQuality(string userId, int averageWakeUps, int averageSleepEfficiency);
        string SaveUserSleepQuality(UserSleepQualityRecord record);
        string UpdateUserSleepQuality(string UserId, int AverageWakeUps, int AverageSleepEfficiency);
        string UpdateUserSleepQuality(UserSleepQualityRecord record);
    }
}
