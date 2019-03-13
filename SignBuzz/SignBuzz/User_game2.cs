using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

using System.Collections.Generic;
using System.Text;

namespace SignBuzz
{
    public class User_game2
    {
        string id;
        string userId;
        int stage;
        int ex1_g2;
        int ex2_g2;
        int ex3_g2;
        int ex4_g2;
        int ex5_g2;

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
        
        [JsonProperty(PropertyName = "ex1_g2")]
        public int Ex1_g2
        {
            get { return ex1_g2; }
            set { ex1_g2 = value; }
        }
        [JsonProperty(PropertyName = "ex2_g2")]
        public int Ex2_g2
        {
            get { return ex2_g2; }
            set { ex2_g2 = value; }
        }
        [JsonProperty(PropertyName = "ex3_g2")]
        public int Ex3_g2
        {
            get { return ex3_g2; }
            set { ex3_g2 = value; }
        }
        [JsonProperty(PropertyName = "ex4_g2")]
        public int Ex4_g2
        {
            get { return ex4_g2; }
            set { ex4_g2 = value; }
        }
        [JsonProperty(PropertyName = "ex5_g2")]
        public int Ex5_g2
        {
            get { return ex5_g2; }
            set { ex5_g2 = value; }
        }

        [Version]
        public string Version { get; set; }
    }
}


