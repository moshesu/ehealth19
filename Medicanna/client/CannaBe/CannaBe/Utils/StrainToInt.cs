using System;
using System.Collections.Generic;
using System.Linq;

namespace CannaBe
{
    static class StrainToInt
    {
        public static string Name(this Enum val)
        {
            var words = val.ToString().Split('_').ToArray();

            for (int i = 0; i < words.Length; i++)
            {
                var word = words[i];
                if (word != "of")
                {
                    var w1 = word.ToLower();
                    words[i] = word[0].ToString().ToUpper() + w1.Substring(1);
                }
            }

            return string.Join(" ", words);
        }

        public static int FromIntListToBitmap(this List<int> intList)
        {
            int bitmap = 0;

            foreach (int i in intList)
            {
                bitmap |= 1 << (i - 1);
            }

            return bitmap;
        }


        public static List<TEnum> FromBitmapToEnumList<TEnum>(this int bitmap)
        {
            List<TEnum> lst = new List<TEnum>();

            try
            {
                foreach (TEnum val in Enum.GetValues(typeof(TEnum)))
                {
                    if((bitmap & (1 << (Convert.ToInt32(val)- 1))) != 0)
                    {
                        lst.Add(val);
                    }
                }
            }
            catch (Exception x)
            {
                AppDebug.Exception(x, $"FromBitmapToEnumList Type:{typeof(TEnum).Name}");
            }

            return lst;
        }
    }
}
