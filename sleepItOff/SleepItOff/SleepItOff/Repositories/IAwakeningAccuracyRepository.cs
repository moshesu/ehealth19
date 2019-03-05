using SleepItOff.Entities;
using System;

namespace SleepItOff.Repositories
{
	public interface IAwakeningAccuracyRepository
	{
        UserAwakeningAccuracyRecord GetUserAwakeningAccuracy(string userId);
		string SaveUserAwakeningAccuracy(string userId, int sleepType, DateTime definedAlaramTime, DateTime actualAlaramTime, TimeSpan timeDifference);
        string SaveUserAwakeningAccuracy(UserAwakeningAccuracyRecord record);
	}
}
