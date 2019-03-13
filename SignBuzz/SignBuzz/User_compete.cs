using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace SignBuzz
{
    public class User_compete
     {
        string id;
        string userId;
        string name;
        double lati;
        double longi;
        int res;

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
        [JsonProperty(PropertyName = "lati")]
        public double Lati
        {
            get { return lati; }
            set { lati = value; }
        }

        [JsonProperty(PropertyName = "longi")]
        public double Longi
        {
            get { return longi; }
            set { longi = value; }
        }
        [JsonProperty(PropertyName = "res")]
        public int Res
        {
            get { return res; }
            set { res = value; }
        }
        
       

        [Version]
        public string Version { get; set; }
    }
}

