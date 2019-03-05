using System;

namespace SleepItOff
{
    public class LiveIdCredentials
    {
        public static string AccessToken { get; set; }

        public static long ExpiresIn { get; set; }

        public static string RefreshToken { get; set; }

        //Last_Device_Sync should be updated just before Last_Sleep_Segment updated
        public static DateTime Last_Device_Sync { get; set; }

        public static SleepSegment Last_Sleep_Segment { get; set; }

        public static DateTime SleepStart { get; set; }

        public static string firstName { get; set; }

        public static string lastName { get; set; }

        public static string middleName { get; set; }

        public static string gender { get; set; }

        public static int height { get; set; }

        public static int weight { get; set; }

        public static DateTime birthdate { get; set; }

        public static string userId { get; set; }

        public static int mean_sleep_efficience { get; set; }

        public static int mean_num_of_wakeups { get; set; }

        public static void ClearCreds()
        {
            AccessToken = "";
            ExpiresIn = 0;
            RefreshToken = "";
            Last_Sleep_Segment = null;           
            firstName = "";
            lastName = "";
            middleName = "";
            gender = "";
            height = 0;
            weight = 0;
            userId = "";
            mean_num_of_wakeups = 0;
            mean_sleep_efficience = 0;            
        }

    }
}
