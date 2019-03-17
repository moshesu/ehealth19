using CannaBe.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CannaBe
{
    class Strain : ViewModel
    {
        private string name;

        [JsonProperty("name")]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [JsonProperty("id")]
        public int ID;

        [JsonProperty("race")]
        public string Race;

        [JsonProperty("description")]
        public string Description = "";

        [JsonProperty("rank")]
        public double Rank;

        [JsonProperty("number_of_usages")]
        public int NumberOfUsages;

        private int bitmapMedicalNeeds;

        [JsonProperty("medical")]
        public int BitmapMedicalNeeds
        {
            get
            {
                return bitmapMedicalNeeds;
            }
            set
            {
                bitmapMedicalNeeds = value;
                MedicalNeeds = value.FromBitmapToEnumList<MedicalEnum>();
            }
        }
        public List<MedicalEnum> MedicalNeeds { get; set; }

        private int bitmapPositivePreferences;

        [JsonProperty("positive")]
        public int BitmapPositivePreferences
        {
            get
            {
                return bitmapPositivePreferences;
            }
            set
            {
                bitmapPositivePreferences = value;
                PositivePreferences = value.FromBitmapToEnumList<PositivePreferencesEnum>();
            }
        }
        public List<PositivePreferencesEnum> PositivePreferences { get; set; }

        private int bitmapNegativePreferences;

        [JsonProperty("negative")]
        public int BitmapNegativePreferences
        {
            get
            {
                return bitmapNegativePreferences;
            }
            set
            {
                bitmapNegativePreferences = value;
                NegativePreferences = value.FromBitmapToEnumList<NegativePreferencesEnum>();
            }
        }

        public List<NegativePreferencesEnum> NegativePreferences { get; set; }



        [JsonConstructor]
        public Strain(string name, int iD, int medicalNeeds, string race,
            int positivePreferences,
            int negativePreferences)
        {
            Name = name;
            ID = iD;
            Race = race;
            BitmapMedicalNeeds = medicalNeeds;
            BitmapPositivePreferences = positivePreferences;
            BitmapNegativePreferences = negativePreferences;
        }

        public Strain(string name, int iD)
        {
            Name = name;
            ID = iD;
        }

        public string PropertiesString
        {
            get
            {
                return GetPropertiesString();
            }
        }

        public string GetPropertiesString()
        {
            StringBuilder b = new StringBuilder();
            int i = 1;

            if (MedicalNeeds.Count > 0)
            { // Medical preferences
                b.AppendLine("- Medical Needs:");
                foreach (var mn in MedicalNeeds)
                {
                    b.AppendLine($"\t{i++}. {mn.Name()}");
                }
            }
            else
            {
                b.AppendLine("- No medical needs listed.");
            }

            if (PositivePreferences.Count > 0)
            { // Positive preferences
                b.AppendLine("- Positive Effects:");
                i = 1;
                foreach (var mn in PositivePreferences)
                {
                    b.AppendLine($"\t{i++}. {mn.Name()}");
                }
            }
            else
            {
                b.AppendLine("- No positive effects listed.");
            }

            if (NegativePreferences.Count > 0)
            { // Negative preferences
                b.AppendLine("- Negative Effects:");
                i = 1;
                foreach (var mn in NegativePreferences)
                {
                    b.AppendLine($"\t{i++}. {mn.Name()}");
                }
            }
            else
            {
                b.AppendLine("- No negative effects listed.");
            }

            return b.ToString().Substring(0, b.Length - 2);
        }

        static double CountSetBits(int n)
        { // Count bits turned on
            int count = 0;
            while (n > 0)
            {
                count += n & 1;
                n >>= 1;
            }
            return count;
        }

        public static double operator /(Strain x, UserData y)
        {

            double medical = CountSetBits(x.BitmapMedicalNeeds & y.Data.BitmapMedicalNeeds) / CountSetBits(y.Data.BitmapMedicalNeeds);
            double positive = 1.0;
            if (y.Data.BitmapPositivePreferences > 0)
            {
                positive = CountSetBits(x.BitmapPositivePreferences & y.Data.BitmapPositivePreferences) / CountSetBits(y.Data.BitmapPositivePreferences);
            }
            var val = 100 * ((0.5 * medical) + (0.5 * positive));
            //AppDebug.Line($"User {y.Data.Username}, Strain {x.Name}, medical: {Convert.ToString(x.BitmapMedicalNeeds & y.Data.BitmapMedicalNeeds,2)}/{Convert.ToString(y.Data.BitmapMedicalNeeds,2)}={medical}, pos: {Convert.ToString(x.BitmapPositivePreferences & y.Data.BitmapPositivePreferences,2)}/{Convert.ToString(y.Data.BitmapPositivePreferences,2)}={positive}, percent {val}");
            return val;
        }

        public double MatchingPercent { get; set; } = 100;

        /******************************************************************************/

        public static Comparison<Strain> MatchComparison = (s1, s2) =>
        { // Compare strains by match percentage
            var r = -1 * s1.MatchingPercent.CompareTo(s2.MatchingPercent);
            if (r == 0)
            {
                r = s1.Name.CompareTo(s2.Name);
            }
            return r;
        };

        public static Comparison<Strain> RankComparison = (s1, s2) =>
        { // Compare strains by rank
            var r = -1 * s1.Rank.CompareTo(s2.Rank);
            if (r == 0)
            {
                r = s1.Name.CompareTo(s2.Name);
            }
            return r;
        };

        public static Comparison<Strain> CountComparison = (s1, s2) =>
        { // Compare strains by number of usages
            var r = -1 * s1.NumberOfUsages.CompareTo(s2.NumberOfUsages);
            if (r == 0)
            {
                r = s1.Name.CompareTo(s2.Name);
            }
            return r;
        };
    }
}
