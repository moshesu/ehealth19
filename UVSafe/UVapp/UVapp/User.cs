using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Newtonsoft.Json;

namespace UVapp
{
    //TBD: 7 days of the week exposete , Last day recorded, SkinType, maxUv
    /*
    class DailyData
    {
        public int TimeExposed { get; set; } //counting minutes'
        public double accumulatedUv;
        public double maxUv { get; set; }
        public DateTime date;
    }

    class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        SkinType skinType;

        DailyData[] recentDaysData;
    }
    */

    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long TimeExposed { get; set; } //counting minutes
        public double accumulatedUV { get; set; }   // In minutes*UVI
        public string Date { get; set; }
        public int skinType { get; set; }
        public int maxUv { get; set; }

        public static User deserializeJson(string userJson)
        {
            return JsonConvert.DeserializeObject<User>(userJson);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public User(string userName, string password, SkinType skinType)
        {
            this.UserName = userName;
            this.Password = password;
            this.Date = "";
            this.Id = userName + "-" + password;
            this.skinType = (int)skinType;
            this.accumulatedUV = 0;
            this.TimeExposed = 0;
            this.maxUv = 0;
        }
        public User(string userName, string password)       // TBD: just for now, for debugging purposes
        {
            this.UserName = userName;
            this.Password = password;
            this.Date = "";
            this.Id = userName + "-" + password;
            this.skinType = 0;
            this.accumulatedUV = 0;
            this.maxUv = 0;
        }

        public User()
        {
            // Default constructor for JSON
        }

        public static string getTodayDateString()
        {
            string[] dateArray = DateTime.Now.ToString().Split(' ')[0].Split('/');
            string dateStr = "";
            for(int i = 0; i < dateArray.Length - 1; i++)
            {
                dateStr = dateStr + dateArray[i] + ".";
            }
            dateStr = dateStr + dateArray[dateArray.Length - 1];
            return dateStr;
        }

    }
}
