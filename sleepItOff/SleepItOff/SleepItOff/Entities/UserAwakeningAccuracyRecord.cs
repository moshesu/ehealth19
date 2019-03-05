using System;

namespace SleepItOff.Entities
{
	public class UserAwakeningAccuracyRecord
	{
		public string userId { get; set; }
		public int sleepType { get; set; }
        public DateTime definedAlarmTime { get; set; }
        public DateTime actualAlarmTime { get; set; }
        public TimeSpan timeDifference { get; set; }

        public UserAwakeningAccuracyRecord() { }

		public UserAwakeningAccuracyRecord(string UserId, int SleepType, DateTime DefinedAlarmTime, 
                                            DateTime ActualAlarmTime, TimeSpan TimeDifference)
		{
            userId = UserId;
            sleepType = SleepType;
            definedAlarmTime = DefinedAlarmTime;
            actualAlarmTime = ActualAlarmTime;
            timeDifference = TimeDifference;
        }
	}
}
