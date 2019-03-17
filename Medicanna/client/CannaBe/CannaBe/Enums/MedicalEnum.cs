using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CannaBe.Enums
{
    enum MedicalEnum
    { // Medical preferences and related questions
        [EnumDescriptions("Are your seizures less frequent?", "Rate the intensity of your seizures:")]
        SEIZURES = 1,
        [EnumDescriptions("Do your muscles fell less stiff?", "Rate your muscle pain:")]
        MUSCLE_SPASMS = 2,
        [EnumDescriptions("Do your muscles fell less stiff?", "Rate your muscle stiffnes:")]
        SPASTICITY = 3,
        [EnumDescriptions("Is there a relief in your inflammation effect?", "Rate the intensity of your inflammation:")]
        INFLAMMATION = 4,
        [EnumDescriptions("Is there an improvement in your eye pressure?", "Rate the intensity of your eye pressure:")]
        EYE_PRESSURE = 5,
        [EnumDescriptions("Do you have fewer headaches?", "Rate how strong your headaches are:")]
        HEADACHES = 6,
        [EnumDescriptions("Do you feel more energetic?", "Rate how tired you regularly feel:")]
        FATIGUE = 7,
        [EnumDescriptions("Are you feeling less nauseous?", "Rate the intensity of your nausea:")]
        NAUSEA = 8,
        [EnumDescriptions("Is your appetite larger?", "Rate how bad is your appetite:")]
        LACK_OF_APPETITE = 9,
        [EnumDescriptions("Do you have less cramps?", "Rate the intensity of your cramps:")]
        CRAMPS = 10,
        [EnumDescriptions("Do you feel more relaxed?", "Rate how stressful you regularly feel:")]
        STRESS = 11,
        [EnumDescriptions("Is your pain improving?", "Rate your pain:")]
        PAIN = 12,
        [EnumDescriptions("Do you feel less depressed?", "Rate how intense is your depression regularly:")]
        DEPRESSION = 13,
        [EnumDescriptions("Do you feel improvement in your sleep quality?", "Rate how bad your sleep is:")]
        INSOMNIA = 14,
        [EnumDescriptions("Do you have fewer headaches?", "Rate the frequency of your headaches:")]
        HEADACHE = 15
    }

    static class MedicalEnumMethods
    { // Generate list of preferences from int list
        public static List<string> FromIntToStringList(List<int> intList)
        {
            List<string> strList = new List<string>(intList.Count);

            foreach (int i in intList)
            {
                strList.Add(((MedicalEnum)i).ToString());
            }

            return strList;
        }

        public static List<MedicalEnum> FromIntList(List<int> intList)
        { // Generate enum list from int list
            List<MedicalEnum> enumList = new List<MedicalEnum>(intList.Count);

            foreach (int i in intList)
            {
                MedicalEnum m = ((MedicalEnum)i);
                enumList.Add(m);
                AppDebug.Line(m.GetType().GetMember(m.ToString()));//.First().GetCustomAttributes<TAttribute>();
            }

            return enumList;
        }

        public static List<MedicalEnum> FromStringList(List<string> strList)
        { // Generate enum list from string list
            AppDebug.Line("MedicalEnum isStrNull = " + (strList == null).ToString());

            List<MedicalEnum> enumList = new List<MedicalEnum>(strList.Count);
            AppDebug.Line("Created List<MedicalEnum>");

            foreach (string s in strList)
            {
                try
                { // Try to get number from string
                    AppDebug.Line($"Trying to parse '{s}'");
                    Enum.TryParse(s, out MedicalEnum val);
                    enumList.Add(val);
                }
                catch
                {
                    AppDebug.Line($"Failed FromStringList:\nValue '{s}' unknown");
                }

            }

            return enumList;
        }

        public static List<int> FromEnumToIntList(List<MedicalEnum> list)
        { // Generate int list from enum list
            List<int> res = new List<int>();

            foreach (MedicalEnum var in list)
            {
                res.Add((int)var);
            }
            return res;
        }


        public static int BitmapFromStringList(List<string> strList)
        { // Generate bitmap from string list
            //AppDebug.Line("MedicalEnum isStrNull = " + (strList == null).ToString());

            List<MedicalEnum> enumList = new List<MedicalEnum>(strList.Count);
            //AppDebug.Line("Created List<MedicalEnum>");

            int bitmap = 0;
        
            foreach (string s in strList)
            {
                try
                {
                    //AppDebug.Line($"Trying to parse '{s}'");
                    Enum.TryParse(s.ToUpper().Replace(' ','_'), out MedicalEnum val);

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
