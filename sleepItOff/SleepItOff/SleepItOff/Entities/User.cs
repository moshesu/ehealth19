using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;


namespace SleepItOff.Entities
{    

    class User
    {

        [JsonProperty(PropertyName = "UserId")]
        private string UserId { get; set; }

        [JsonProperty(PropertyName = "FirstName")]
        private string FirstName { get; set; }

        [JsonProperty(PropertyName = "LastName")]
        private string LastName { get; set; }

        [JsonProperty(PropertyName = "Gender")]
        private string Gender { get; set; }

        [JsonProperty(PropertyName = "DateOfBirth")]
        private int DateOfBirth { get; set; }

        [JsonProperty(PropertyName = "Height")]
        private int Height { get; set; }

        [JsonProperty(PropertyName = "Weight")]
        private int Weight { get; set; }

        [JsonProperty(PropertyName = "UserName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "Password")]
        private string Password { get; set; }

        public User() { } //default c'tor

        public User(string FirstName,
                         string LastName,
                         string Gender,
                         int DateOfBirth,
                         int Height,
                         int Weight,
                         string UserName,
                         string Password,
                         string UserId
                         )
        {
            this.SetFirstName(FirstName);
            this.SetLastName(LastName);
            this.SetGender(Gender);
            this.SetDateOfBirth(DateOfBirth);
            this.SetHeight(Height);
            this.SetWeight(Weight);
            this.UserName = UserName;
            this.Password = Password;
            this.UserId = UserId;
        }

        /*public void SetUserId()
        {
            Id = UUID.NameUUIDFromBytes(Encoding.ASCII.GetBytes(GetUserName().ToString())).ToString();
        }*/

        public void SetFirstName(string firstName)
        {
            this.FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            this.LastName = lastName;
        }

        public void SetGender(string gender)
        {
            this.Gender = gender;
        }

        public void SetDateOfBirth(int dateOfBirth)
        {
            this.DateOfBirth = dateOfBirth;
        }

        public void SetHeight(int height)
        {
            this.Height = height;
        }

        public void SetWeight(int weight)
        {
            this.Weight = weight;
        }

        public void SetUserName(string username)
        {
            this.UserName = username;
        }

        public void SetPassword(string password)
        {
            this.Password = password;
        }
        
        public string GetFirstName()
        {
            return FirstName;
        }

        public string GetLastName()
        {
            return LastName;
        }

        public string GetGender()
        {
            return Gender;
        }

        public int GetDateOfBirth()
        {
            return DateOfBirth;
        }

        public int GetHeight()
        {
            return Height;
        }

        public int HetWeight()
        {
            return Weight;
        }

        public string GetId()
        {
            return UserId;
        }

        public string GetUserName()
        {
            return UserName;
        }

        public string GetPassword()
        {
            return Password;
        }

    }

}