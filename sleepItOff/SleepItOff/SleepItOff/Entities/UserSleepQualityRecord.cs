namespace SleepItOff.Entities
{
	public class UserSleepQualityRecord
	{
		public string userId { get; set; }
        public int averageWakeUps { get; set; }
        public int averageSleepEfficiency { get; set; }

        public UserSleepQualityRecord() { }

		public UserSleepQualityRecord(string UserId, int AverageWakeUps, int AverageSleepEfficiency)
		{
            userId = UserId;
            averageWakeUps = AverageWakeUps;
            averageSleepEfficiency = AverageSleepEfficiency;
        }
	}
}
