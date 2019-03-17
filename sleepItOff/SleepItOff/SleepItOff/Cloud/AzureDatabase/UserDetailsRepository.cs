using System;
using SleepItOff.Entities;
using SleepItOff.Repositories;
using Newtonsoft.Json;

namespace SleepItOff.Cloud.AzureDatabase
{
	public class UserDetailsRepository : AbstractAzureRepository, IUserDetailsRepository
    {
        public UserDetailsRecord GetUserDetails(string userId)
        {
            var userIdParameter = new Parameter(UserIdKey, userId);

            var result = CallAzureDatabase("GetUserDetails", userIdParameter);
            if (result == null)
                return null;

            return JsonConvert.DeserializeObject<UserDetailsRecord>(result);
        }

		public string SaveUserDetails(string UserId, string FirstName, string LastName, string Gender,
                                    int Age, int Height, int Weight)
		{
            return SaveUserDetails(new UserDetailsRecord
            {
                userId = UserId,
                firstName = FirstName,
				lastName = LastName,
                gender = Gender,
                age = Age,
                height = Height,
                weight = Weight
        });
		}

        public string SaveUserDetails(UserDetailsRecord record)
        {
            var userIdParameter = new Parameter(UserIdKey, record.userId);
            var json = JsonConvert.SerializeObject(record);
            var dataParameter = new Parameter("data", json);

            return CallAzureDatabase("SaveUserDetails", userIdParameter, dataParameter);
        }


        public UserIdsModel GetUsersByGender(string _gender)
        {
            var genderParameter = new Parameter(GenderKey, _gender);

            var result = CallAzureDatabase("GetUsersByGender", genderParameter);
            if (result == null)
                return null;

            return JsonConvert.DeserializeObject<UserIdsModel>(result);
        }

        public UserIdsModel GetUsersByAgeRange(int _min, int _max)
        {
            var minAgeParameter = new Parameter(minAgeKey, _min.ToString());
            var maxAgeParameter = new Parameter(maxAgeKey, _max.ToString());
            var parameters = new Parameter[2] { minAgeParameter, maxAgeParameter };

            var result = CallAzureDatabase("GetUsersByAgeRange", parameters);
            if (result == null)
                return null;

            return JsonConvert.DeserializeObject<UserIdsModel>(result);
        }

        public UserIdsModel GetUsersByGenderAndAgeRange(int _minAge, int _maxAge, string _gender)
        {
            var genderParameter = new Parameter(GenderKey, _gender);
            var minAgeParameter = new Parameter(minAgeKey, _minAge.ToString());
            var maxAgeParameter = new Parameter(maxAgeKey, _maxAge.ToString());
            var parameters = new Parameter[3] {genderParameter, minAgeParameter, maxAgeParameter };

            var result = CallAzureDatabase("GetUsersByGenderAndAgeRange", parameters);
            if (result == null)
                return null;

            return JsonConvert.DeserializeObject<UserIdsModel>(result);
        }

        public UserIdsModel GetUsersByGenderHeightAndWeight(int _height, int _weight, string _gender)
        {
            var genderParameter = new Parameter(GenderKey, _gender);
            var heightParameter = new Parameter(HeightKey, _height.ToString());
            var weightParameter = new Parameter(WeightKey, _weight.ToString());
            var parameters = new Parameter[3] { genderParameter, heightParameter, weightParameter };

            var result = CallAzureDatabase("GetUsersByGenderHeightAndWeight", parameters);
            if (result == null)
                return null;

            return JsonConvert.DeserializeObject<UserIdsModel>(result);
        }

        public UserIdsModel GetUsersByGenderHeightWeightAndAgeRange(int _age, int _height, int _weight, string _gender)
        {
            var genderParameter = new Parameter(GenderKey, _gender);
            var ageParameter = new Parameter(AgeKey, _age.ToString());
            var heightParameter = new Parameter(HeightKey, _height.ToString());
            var weightParameter = new Parameter(WeightKey, _weight.ToString());
            var parameters = new Parameter[4] { genderParameter, ageParameter, heightParameter, weightParameter };

            var result = CallAzureDatabase("GetUsersByGenderHeightWeightAndAgeRange", parameters);
            if (result == null)
                return null;

            return JsonConvert.DeserializeObject<UserIdsModel>(result);
        }
    }
}