using SleepItOff.Entities;

namespace SleepItOff.Repositories
{
	public interface IUserDetailsRepository
	{
		UserDetailsRecord GetUserDetails(string userId);
		string SaveUserDetails(string userId, string firstName, string lastName, string gender, int age, int height, int weight);
        string SaveUserDetails(UserDetailsRecord record);
	}
}
