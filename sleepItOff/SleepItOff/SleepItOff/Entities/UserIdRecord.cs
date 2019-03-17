namespace SleepItOff.Entities
{
	public class UserIdRecord
	{
		public string userId { get; set; }

		public UserIdRecord() { }

		public UserIdRecord(string UserId)
		{
            userId = UserId;
		}
	}
}
