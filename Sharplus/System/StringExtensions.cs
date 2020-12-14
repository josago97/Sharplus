using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
