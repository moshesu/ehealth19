using SleepItOff.Entities;
using System;

namespace SleepItOff.Repositories
{
	public interface ISleepSegmentsStatsRepository
	{
        UserSleepSegmentsStatsRecord GetUserSleepSegmentsStats(string userId);
		string SaveUserSleepSegmentsStats(string UserId, DateTime LastUpdated,
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
                                             int rEMSleepToRestfulSleepCount, int rEMSleepToREMCount);
        string SaveUserSleepSegmentsStats(UserSleepSegmentsStatsRecord record);
        string UpdateUserSleepSegmentsStats(string UserId, DateTime LastUpdated,
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
                                             int rEMSleepToRestfulSleepCount, int rEMSleepToREMCount);
        string UpdateUserSleepSegmentsStats(UserSleepSegmentsStatsRecord record);
    }
}
