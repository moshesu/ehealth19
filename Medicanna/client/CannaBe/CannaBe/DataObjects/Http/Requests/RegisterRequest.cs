using Newtonsoft.Json;
using System.Collections.Generic;

namespace CannaBe
{
    class RegisterRequest : LoginRequest
    {
        [JsonProperty("dob")]
        public string DOB { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("medical")]
        public int BitmapMedicalNeeds { get; set; }
        //public List<string> MedicalNeeds { get; set; }

        [JsonProperty("positive")]
        public int BitmapPositivePreferences { get; set; }
        //public List<string> PositivePreferences{ get; set; }

        [JsonProperty("negative")]
        public int BitmapNegativePreferences { get; set; }
        //public List<string> NegativePreferences { get; set; }

        private List<int> intListMedicalNeeds;
        [JsonIgnore]
        public List<int> IntListMedicalNeeds
        {
            get
            {
                return intListMedicalNeeds;
            }
            set
            {
                intListMedicalNeeds = value;
                BitmapMedicalNeeds = value.FromIntListToBitmap();
            }
        }

        private List<int> intPositivePreferences;

        [JsonIgnore]
        public List<int> IntPositivePreferences
        {
            get
            {
                return intPositivePreferences;
            }
            set
            {
                intPositivePreferences = value;
                BitmapPositivePreferences = value.FromIntListToBitmap();
            }
        }


        private List<int> intNegativePreferences;

        [JsonIgnore]
        public List<int> IntNegativePreferences
        {
            get
            {
                return intNegativePreferences;
            }
            set
            {
                intNegativePreferences = value;
                BitmapNegativePreferences = value.FromIntListToBitmap();
            }
        }

        public RegisterRequest() { }

        public RegisterRequest(string username, string password, string dob, string gender, string country, string city, string email) 
            : base(username, password)
        {
            DOB = dob;
            Gender = gender;
            Country = country;
            City = city;
            Email = email;
        }
    }
}
