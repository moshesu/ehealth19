using System;

namespace SleepItOff.SleepItOffDBFunction.Database
{
	public abstract class UserModel
	{
		public int userId { get; set; }
	}

    public class User : UserModel
    {
        public string userName { get; set; }
        public string password { get; set; }
    }

    public class UserDetail : UserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }

    }
}