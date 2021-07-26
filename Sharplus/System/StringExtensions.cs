using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace System
{
    public static class StringExtensions
    {
        public static string ToBase64(this string text)
        {
            return ToBase64(text, Encoding.UTF8);
        }

        public static string ToBase64(this string text, Encoding encoding)
        {
            string result = text;

            if (!string.IsNullOrEmpty(text))
            {
                byte[] textAsBytes = encoding.GetBytes(text);
                result = Convert.ToBase64String(textAsBytes);
            }

            return result;
        }

        public static bool TryParseBase64(this string text, out string decodedText)
        {
            return TryParseBase64(text, Encoding.UTF8, out decodedText);
        }

        public static bool TryParseBase64(this string text, Encoding encoding, out string decodedText)
        {
            bool parsed = false;

            if (string.IsNullOrEmpty(text))
            {
                decodedText = text;
            }
            else
            {
                try
                {
                    byte[] textAsBytes = Convert.FromBase64String(text);
                    decodedText = encoding.GetString(textAsBytes);
                    parsed = true;
                }
                catch (Exception)
                {
                    decodedText = null;
                }
            }

            return parsed;
        }

        public static string ToTitleCase(this string text, bool onlyFirst = true)
        {
            string result = string.Empty;

            if (text.Length > 0)
            {
                if (onlyFirst)
                {
                    string aux = text.ToLowerInvariant();
                    result = char.ToUpper(aux[0]) + aux.Substring(1);
                }
                else
                {
                    result = text.ToTitleCase(Thread.CurrentThread.CurrentCulture);
                }
            }

            return result;
        }

        public static string ToTitleCase(this string text, CultureInfo cultureInfo)
        {
            return cultureInfo.TextInfo.ToTitleCase(text.ToLower());
        }
    }
}
