using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Band.Sensors;

namespace UVapp
{
    static class ClientRecommendations
    {
        public static string getEnumUVRecommendation(UVIndexLevel uvi)
        {
            string res = "";

            /* This doesn't work as a switch case, I think because 
             * UVIndexLevel doesn't act as a C# enum, but as a Java binding.
             */

            if (uvi == UVIndexLevel.None)
                res = "No UV detected";
            else if (uvi == UVIndexLevel.Low)
                res = "UV is low, if you burn easily make sure you wear long sleeves and apply sunscreen preferably 30spf \n dont forget you sunglasses ;)";
            else if (uvi == UVIndexLevel.Medium)
                res = "UV is medium , try covering yourself as much as you can or stay in the shadow at noon , and don't forget to wear sunglasses, apply sunscreen 30spf and wear a hat";
            else if (uvi == UVIndexLevel.High)
                res = "High UV , must protect your skin and eyes , try staying indoors or in the shadow as much as possible , wear long clothes and keep your sunglasses on and apply 30spf sunscreen or more every two hours!";
            else if (uvi == UVIndexLevel.VeryHigh)
                res = "Very high UV , you need special protection for your eyes and skin , keep your sun exposure to minimum , wear long clothes, a hat and sunglasses , apply high sunscreen every two hours ";

            return res;
        }


        public static string getIntUVRecommendation(int uv)
        {
            string res = "";

            switch (uv)
            {
                case 0:
                    res = getEnumUVRecommendation(UVIndexLevel.None);
                    break;
                case 1:
                case 2:
                    res = getEnumUVRecommendation(UVIndexLevel.Low);
                    break;
                case 3:
                case 4:
                case 5:
                    res = getEnumUVRecommendation(UVIndexLevel.Medium);
                    break;
                case 6:
                case 7:
                    res = getEnumUVRecommendation(UVIndexLevel.High);
                    break;
                default:
                    if (uv >= 8)
                        res = getEnumUVRecommendation(UVIndexLevel.VeryHigh);
                    break;

                    /*
                 default:
                     if (uv >= 11) res = "Extreme UV, protect your eyes and skin by all means possible, keep your sun exposure to minimum, wear long clothes, a hat and sunglasses, apply high sunscreen every two hours";
                     break;
                     */
            }

            //uvButton.Text = uv.ToString();

            return res;
        }
    }
}