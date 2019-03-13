using SleepItOff.Entities;
using System;

namespace SleepItOff.Repositories
{
	public interface IManageUsersRepository
	{
		UserRecord GetUser(string userId);
		string SaveUser(string userId, DateTime lastUpdated);
		string SaveUser(UserRecord record);
	}
}
