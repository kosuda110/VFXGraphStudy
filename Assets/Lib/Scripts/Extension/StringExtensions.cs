namespace Kosu.UnityLibrary
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotNullOrEmpty(this string str)
        {
            return IsNullOrEmpty(str) == false;
        }

        public static string Formats(this string format, params object[] values)
        {
            return string.Format(format, values);
        }

        /// <summary>
        /// 単語の先頭を大文字にする
        /// </summary>
        public static string ToTitleCase(this string self)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(self);
        }
    }
}
