using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;


namespace SignBuzz
{
    public class User
    {
        string id;
        string userId;
        string name;
        int stage;
        int prizes;
        string image;
    
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

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [JsonProperty(PropertyName = "stage")]
        public int Stage
        {
            get { return stage; }
            set { stage = value; }
        }
        [JsonProperty(PropertyName = "img")]
        public String Image
        {
            get { return image; }
            set { image = value; }
        }
        [JsonProperty(PropertyName = "prizes")]
        public int Prizes
        {
            get { return prizes; }
            set { prizes = value; }
        }

        [Version]
        public string Version { get; set; }
    }
}
