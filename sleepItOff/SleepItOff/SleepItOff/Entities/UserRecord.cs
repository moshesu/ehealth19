using System;

namespace SleepItOff.Entities
{
	public class UserRecord
	{
		public string userId { get; set; }
        public DateTime lastUpdated { get; set; }
        /*public string userName { get; set; }
        public string password { get; set; }*/

        public UserRecord() { }

		public UserRecord(string UserId, DateTime LastUpdated)
		{
            userId = UserId;
            lastUpdated = LastUpdated;
		}
	}
}
