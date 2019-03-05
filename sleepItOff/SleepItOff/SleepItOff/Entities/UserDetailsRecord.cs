namespace SleepItOff.Entities
{
	public class UserDetailsRecord
	{
		public string userId { get; set; }
		public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public int age { get; set; }
        public int height { get; set; }
        public int weight { get; set; }

        public UserDetailsRecord() { }

		public UserDetailsRecord(string UserId, string FirstName, string LastName, string Gender,
                                    int Age, int Height, int Weight)
		{
            userId = UserId;
            firstName = FirstName ?? "NULL";
            lastName = LastName ?? "NULL";
            gender = Gender ?? "NULL";
            age = Age;
            height = Height;
            weight = Weight;
        }
    }
}
