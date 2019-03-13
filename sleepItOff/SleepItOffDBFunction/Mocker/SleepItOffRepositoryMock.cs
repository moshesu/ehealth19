using System;
using System.Collections.Generic;
using System.Linq;
using SleepItOff.SleepItOffDBFunction.Database;

namespace SleepItOff.SleepItOffDBFunction.Mocker
{
	public class SleepItOffRepositoryMock
    {
        private readonly char[] _genders = new[] { 'M', 'F' };

        private readonly string[] _userNames = new[]
			{"Omer", "Grisha", "Eyal", "Dummy", "Eitan"};

        private readonly string[] _passwords = new[] { "123", "111", "123321"};

        private int _targetUserId;

        private static readonly Random _rand = new Random();
        
        public IList<UserModel> GetUsers()
		{
			return GetUsers();
		}

		public IList<UserModel> GetUsers(int UserId)
		{
			_targetUserId = UserId;

			var users = Enumerable.Range(1, 3).Select(GenerateUser).ToList();

            users[0].userId = _targetUserId;
			return users;
		}

		private UserModel GenerateUser(int _UserId)
		{
            var rand = new Random();
            int _userNamesIndex = rand.Next(0, _userNames.Length);
            rand = new Random();
            int _passwordsIndex = rand.Next(0, _passwords.Length);

            return new User
            {
				userId = _UserId,
                userName = _userNames[_userNamesIndex],
                password = _passwords[_passwordsIndex]
            };
		}
    }
}