using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
namespace SignBuzz
{
    public class User_game3
    {
        string id;
        string userId;
        int stage;
        int ex1_g3;
        int ex2_g3;
        int ex3_g3;
        int ex4_g3;
        int ex5_g3;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "userId")]
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        [JsonProperty(PropertyName = "stage")]
        public int Stage
        {
            get { return stage; }
            set { stage = value; }
        }

        [JsonProperty(PropertyName = "ex1_g3")]
        public int Ex1_g3
        {
            get { return ex1_g3; }
            set { ex1_g3 = value; }
        }
        [JsonProperty(PropertyName = "ex2_g3")]
        public int Ex2_g3
        {
            get { return ex2_g3; }
            set { ex2_g3 = value; }
        }
        [JsonProperty(PropertyName = "ex3_g3")]
        public int Ex3_g3
        {
            get { return ex3_g3; }
            set { ex3_g3 = value; }
        }
        [JsonProperty(PropertyName = "ex4_g3")]
        public int Ex4_g3
        {
            get { return ex4_g3; }
            set { ex4_g3 = value; }
        }
        [JsonProperty(PropertyName = "ex5_g3")]
        public int Ex5_g3
        {
            get { return ex5_g3; }
            set { ex5_g3 = value; }
        }

        [Version]
        public string Version { get; set; }
    }
}

