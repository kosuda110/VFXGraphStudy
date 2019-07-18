using System.Text.RegularExpressions;
using System.Linq;

namespace Kosu.UnityLibrary
{
    /// <summary>
    /// Enum型の便利クラス
    /// </summary>
    public static class EnumUtils
    {
        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result) where TEnum : struct
        {
            return TryParse<TEnum>(value, ignoreCase, out result, "", "");
        }

        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result, string pattern, string replace) where TEnum : struct
        {
            string[] names = System.Enum.GetNames(typeof(TEnum));
            result = (TEnum)System.Enum.Parse(typeof(TEnum), names[0]);

            if (value == null)
            {
                return false;
            }

            bool enableRegex = pattern.IsNullOrEmpty() == false;

            foreach (var name in names)
            {
                string enumName = name;
                string targetValue = null;

                if (enableRegex)
                {
                    targetValue = Regex.Replace(value, pattern, replace);
                }
                else
                {
                    targetValue = value;
                }

                if (ignoreCase)
                {
                    enumName = enumName.ToLower();
                    targetValue = targetValue.ToLower();
                }

                if (enumName == targetValue)
                {
                    result = (TEnum)System.Enum.Parse(typeof(TEnum), name);
                    return true;
                }
            }

            return false;
        }

        public static TEnum Parse<TEnum>(string value, bool ignoreCase, TEnum defaultValue) where TEnum : struct
        {
            return Parse<TEnum>(value, ignoreCase, defaultValue, "", "");
        }

        public static TEnum Parse<TEnum>(string value, bool ignoreCase, TEnum defaultValue, string pattern, string replace) where TEnum : struct
        {
            TEnum result;

            if (TryParse(value, ignoreCase, out result, pattern, replace))
            {
                return result;
            }

            return defaultValue;
        }

        public static TEnum Random<TEnum>()
        {
            var v = System.Enum.GetValues(typeof(TEnum));
            return (TEnum)v.GetValue(new System.Random().Next(v.Length));
        }
    }
}
