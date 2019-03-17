
using System;
using System.Text.RegularExpressions;

namespace CannaBe
{
    static class Constants
    {
        public const string CannaBeUrl = "http://medicanna.westeurope.cloudapp.azure.com:8080/";
        public const string CannaBeUrlLocalHost = "http://localhost:8080/";

        public static bool IsLocalHost = false;

        private static readonly Regex emailValidator = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

        public static string MakeUrl(string addition)
        {
            if (!IsLocalHost)
                return CannaBeUrl + addition;
            else
                return CannaBeUrlLocalHost + addition;
        }

        public static bool IsValidEmail(this string email)
        {
            return emailValidator.IsMatch(email);
        }

        public static int ToAge(this DateTime dob)
        {
            try
            {
                //taken from here: stackoverflow.com/a/11942
                int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                int dobInt = int.Parse(dob.ToString("yyyyMMdd"));
                return (now - dobInt) / 10000;
            }
            catch(Exception x)
            {
                AppDebug.Exception(x, "GetAgeFromDob");
                return 0;
            }

        }
    }
}
