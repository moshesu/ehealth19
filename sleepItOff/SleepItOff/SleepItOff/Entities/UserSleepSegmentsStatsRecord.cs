using System;

namespace SleepItOff.Entities
{
	public class UserSleepSegmentsStatsRecord
    {
        public string userId { get; set; }
        public DateTime lastUpdated { get; set; }

        public int awakeCountTimes { get; set; }
        public int awakeTotalDuration { get; set; }
        public int awakeToAwakeCount { get; set; }
        public int awakeToSnoozeCount { get; set; }
        public int awakeToDozeCount { get; set; }
        public int awakeToRestlessSleepCount { get; set; }
        public int awakeToRestfulSleepCount { get; set; }
        public int awakeToREMCount { get; set; }

        public int snoozeCountTimes { get; set; }
        public int snoozeTotalDuration { get; set; }
        public int snoozeToAwakeCount { get; set; }
        public int snoozeToSnoozeCount { get; set; }
        public int snoozeToDozeCount { get; set; }
        public int snoozeToRestlessSleepCount { get; set; }
        public int snoozeToRestfulSleepCount { get; set; }
        public int snoozeToREMCount { get; set; }

        public int dozeCountTimes { get; set; }
        public int dozeTotalDuration { get; set; }
        public int dozeToAwakeCount { get; set; }
        public int dozeToSnoozeCount { get; set; }
        public int dozeToDozeCount { get; set; }
        public int dozeToRestlessSleepCount { get; set; }
        public int dozeToRestfulSleepCount { get; set; }
        public int dozeToREMCount { get; set; }

        public int restlessSleepCountTimes { get; set; }
        public int restlessSleepTotalDuration { get; set; }
        public int restlessSleepToAwakeCount { get; set; }
        public int restlessSleepToSnoozeCount { get; set; }
        public int restlessSleepToDozeCount { get; set; }
        public int restlessSleepToRestlessSleepCount { get; set; }
        public int restlessSleepToRestfulSleepCount { get; set; }
        public int restlessSleepToREMCount { get; set; }

        public int restfulSleepCountTimes { get; set; }
        public int restfulSleepTotalDuration { get; set; }
        public int restfulSleepToAwakeCount { get; set; }
        public int restfulSleepToSnoozeCount { get; set; }
        public int restfulSleepToDozeCount { get; set; }
        public int restfulSleepToRestlessSleepCount { get; set; }
        public int restfulSleepToRestfulSleepCount { get; set; }
        public int restfulSleepToREMCount { get; set; }

        public int REMSleepCountTimes { get; set; }
        public int REMSleepTotalDuration { get; set; }
        public int REMSleepToAwakeCount { get; set; }
        public int REMSleepToSnoozeCount { get; set; }
        public int REMSleepToDozeCount { get; set; }
        public int REMSleepToRestlessSleepCount { get; set; }
        public int REMSleepToRestfulSleepCount { get; set; }
        public int REMSleepToREMCount { get; set; }

        public UserSleepSegmentsStatsRecord() { }

		public UserSleepSegmentsStatsRecord( string UserId, DateTime LastUpdated, 
                                             int AwakeCountTimes, int AwakeTotalDuration, int AwakeToAwakeCount, 
                                             int AwakeToSnoozeCount,int AwakeToDozeCount, int AwakeToRestlessSleepCount, 
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
            userId = UserId;            
            lastUpdated = LastUpdated;
            awakeCountTimes = AwakeCountTimes;
            awakeTotalDuration = AwakeTotalDuration;
            awakeToAwakeCount = AwakeToAwakeCount;
            awakeToSnoozeCount = AwakeToSnoozeCount;
            awakeToDozeCount = AwakeToDozeCount;
            awakeToRestlessSleepCount = AwakeToRestlessSleepCount;
            awakeToRestfulSleepCount = AwakeToRestfulSleepCount;
            awakeToREMCount = AwakeToREMCount;

            snoozeCountTimes = SnoozeCountTimes;
            snoozeTotalDuration = SnoozeTotalDuration;
            snoozeToAwakeCount = SnoozeToAwakeCount;
            snoozeToSnoozeCount = SnoozeToSnoozeCount;
            snoozeToDozeCount = SnoozeToDozeCount;
            snoozeToRestlessSleepCount = SnoozeToRestlessSleepCount;
            snoozeToRestfulSleepCount = SnoozeToRestfulSleepCount;
            snoozeToREMCount = SnoozeToREMCount;

            dozeCountTimes = DozeCountTimes;
            dozeTotalDuration = DozeTotalDuration;
            dozeToAwakeCount = DozeToAwakeCount;
            dozeToSnoozeCount = DozeToSnoozeCount;
            dozeToDozeCount = DozeToDozeCount;
            dozeToRestlessSleepCount = DozeToRestlessSleepCount;
            dozeToRestfulSleepCount = DozeToRestfulSleepCount;
            dozeToREMCount = DozeToREMCount;

            restlessSleepCountTimes = RestlessSleepCountTimes;
            restlessSleepTotalDuration = RestlessSleepTotalDuration;
            restlessSleepToAwakeCount = RestlessSleepToAwakeCount;
            restlessSleepToSnoozeCount = RestlessSleepToSnoozeCount;
            restlessSleepToDozeCount = RestlessSleepToDozeCount;
            restlessSleepToRestlessSleepCount = RestlessSleepToRestlessSleepCount;
            restlessSleepToRestfulSleepCount = RestlessSleepToRestfulSleepCount;
            restlessSleepToREMCount = RestlessSleepToREMCount;

            restfulSleepCountTimes = RestfulSleepCountTimes;
            restfulSleepTotalDuration = RestfulSleepTotalDuration;
            restfulSleepToAwakeCount = RestfulSleepToAwakeCount;
            restfulSleepToSnoozeCount = RestfulSleepToSnoozeCount;
            restfulSleepToDozeCount = RestfulSleepToDozeCount;
            restfulSleepToRestlessSleepCount = RestfulSleepToRestlessSleepCount;
            restfulSleepToRestfulSleepCount = RestfulSleepToRestfulSleepCount;
            restfulSleepToREMCount = RestfulSleepToREMCount;

            REMSleepCountTimes = rEMSleepCountTimes;
            REMSleepTotalDuration = rEMSleepTotalDuration;
            REMSleepToAwakeCount = rEMSleepToAwakeCount;
            REMSleepToSnoozeCount = rEMSleepToSnoozeCount;
            REMSleepToDozeCount = rEMSleepToDozeCount;
            REMSleepToRestlessSleepCount = rEMSleepToRestlessSleepCount;
            REMSleepToRestfulSleepCount = rEMSleepToRestfulSleepCount;
            REMSleepToREMCount = rEMSleepToREMCount;            
        }
	}
}
