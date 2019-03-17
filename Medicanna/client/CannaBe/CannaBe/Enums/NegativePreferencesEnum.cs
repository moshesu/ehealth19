using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CannaBe.Enums
{
    enum NegativePreferencesEnum
    {
        DIZZY = 1,
        DRY_MOUTH = 2,
        PARANOID = 3,
        DRY_EYES = 4,
        ANXIOUS = 5
    }

    static class NegativePreferencesEnumMethods
    {
        public static List<string> FromIntToStringList(List<int> intList)
        {
            List<string> strList = new List<string>(intList.Count);

            foreach (int i in intList)
            {
                strList.Add(((NegativePreferencesEnum)i).ToString());
            }

            return strList;
        }


        public static List<NegativePreferencesEnum> FromIntList(List<int> intList)
        {
            List<NegativePreferencesEnum> enumList = new List<NegativePreferencesEnum>(intList.Count);

            foreach (int i in intList)
            {
                enumList.Add((NegativePreferencesEnum)i);
            }

            return enumList;
        }

        public static List<NegativePreferencesEnum> FromStringList(List<string> strList)
        {
            List<NegativePreferencesEnum> enumList = new List<NegativePreferencesEnum>(strList.Count);

            foreach (string s in strList)
            {
                try
                {
                    Enum.TryParse(s, out NegativePreferencesEnum val);
                    enumList.Add(val);
                }
                catch
                {
                    AppDebug.Line($"Failed FromStringList:\nValue '{s}' unknown");
                }

            }

            return enumList;
        }

        public static List<int> FromEnumToIntList(List<NegativePreferencesEnum> list)
        {
            List<int> res = new List<int>();

            foreach (NegativePreferencesEnum var in list)
            {
                res.Add((int)var);
            }
            return res;
        }

        public static int BitmapFromStringList(List<string> strList)
        {
            //AppDebug.Line("NegativePreferencesEnum isStrNull = " + (strList == null).ToString());

            List<NegativePreferencesEnum> enumList = new List<NegativePreferencesEnum>(strList.Count);
            //AppDebug.Line("Created List<NegativePreferencesEnum>");

            int bitmap = 0;

            foreach (string s in strList)
            {
                try
                {
                    //AppDebug.Line($"Trying to parse '{s}'");
                    Enum.TryParse(s.ToUpper().Replace(' ', '_'), out NegativePreferencesEnum val);

                    bitmap |= 1 << (((int)val) - 1);
                }
                catch
                {
                    AppDebug.Line($"Failed FromStringList:\nValue '{s}' unknown");
                }

            }

            return bitmap;
        }

    }
}
