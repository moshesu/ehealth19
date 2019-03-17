using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CannaBe.Enums
{
    enum PositivePreferencesEnum
    {
        AROUSED = 1,
        GIGGLY = 2,
        FOCUSED = 3,
        SLEEPY = 4,
        TINGLY = 5,
        UPLIFTED = 6,
        TALKATIVE = 7,
        ENERGETIC = 8,
        CREATIVE = 9,
        HAPPY = 10,
        EUPHORIC = 11,
        HUNGRY = 12,
        RELAXED = 13
    }

    static class PositivePreferencesEnumMethods
    {
        public static List<string> FromIntToStringList(List<int> intList)
        {
            List<string> strList = new List<string>(intList.Count);

            foreach (int i in intList)
            {
                strList.Add(((PositivePreferencesEnum)i).ToString());
            }

            return strList;
        }

        public static List<PositivePreferencesEnum> FromIntList(List<int> intList)
        {
            List<PositivePreferencesEnum> enumList = new List<PositivePreferencesEnum>(intList.Count);

            foreach (int i in intList)
            {
                enumList.Add((PositivePreferencesEnum)i);
            }

            return enumList;
        }

        public static List<PositivePreferencesEnum> FromStringList(List<string> strList)
        {
            List<PositivePreferencesEnum> enumList = new List<PositivePreferencesEnum>(strList.Count);

            foreach (string s in strList)
            {
                try
                {
                    Enum.TryParse(s, out PositivePreferencesEnum val);
                    enumList.Add(val);
                }
                catch
                {
                    AppDebug.Line($"Failed FromStringList:\nValue '{s}' unknown");
                }

            }

            return enumList;
        }

        public static List<int> FromEnumToIntList(List<PositivePreferencesEnum> list)
        {
            List<int> res = new List<int>();

            foreach (PositivePreferencesEnum var in list)
            {
                res.Add((int)var);
            }
            return res;
        }

        public static int BitmapFromStringList(List<string> strList)
        {
            //AppDebug.Line("PositivePreferencesEnum isStrNull = " + (strList == null).ToString());

            List<PositivePreferencesEnum> enumList = new List<PositivePreferencesEnum>(strList.Count);
            //AppDebug.Line("Created List<PositivePreferencesEnum>");

            int bitmap = 0;

            foreach (string s in strList)
            {
                try
                {
                    //AppDebug.Line($"Trying to parse '{s}'");
                    Enum.TryParse(s.ToUpper().Replace(' ', '_'), out PositivePreferencesEnum val);

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
