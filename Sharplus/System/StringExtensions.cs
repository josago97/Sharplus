using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Sharplus.System.Linq;

namespace Sharplus.System
{
    public static class StringExtensions
    {
        #region EndsWith

        /// <summary>
        /// Determines if the end of a <see cref="string"/> matches any of the specified strings.
        /// </summary>        
        /// <param name="text">The string to compare to the substrings at its end.</param>
        /// <param name="suffixIndex">The zero-based index of the first occurrence, if found; otherwise, -1.</param>
        /// <param name="values">The specified strings to compare to the end of <paramref name="text"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the end of <paramref name="text"/> matches any of the strings of <paramref name="values"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool EndsWith(this string text, out int suffixIndex, IEnumerable<string> values)
        {
            suffixIndex = values.FindIndex(s => text.EndsWith(s));
            return suffixIndex >= 0;
        }

        /// <summary>
        /// Determines if the end of <paramref name="text"/> matches any of the specified strings.
        /// </summary>        
        /// <param name="text">The string to compare to the substrings at its end.</param>
        /// <param name="suffixIndex">The zero-based index of the first occurrence, if found; otherwise, -1.</param>
        /// <param name="values">The specified strings to compare to the end of <paramref name="text"/>.</param>
        /// <param name="comparisonType">One of the enumeration values that determines how <paramref name="text"/> and other <see cref="string"/> are compared.</param>
        /// <returns>
        /// <see langword="true"/> if the end of <paramref name="text"/> matches any of the strings of <paramref name="values"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool EndsWith(this string text, out int suffixIndex, IEnumerable<string> values, StringComparison comparisonType)
        {
            suffixIndex = values.FindIndex(s => text.EndsWith(s, comparisonType));
            return suffixIndex >= 0;
        }

        /// <summary>
        /// Determines if the end of <paramref name="text"/> matches any of the specified strings.
        /// </summary>        
        /// <param name="text">The string to compare to the substrings at its end.</param>
        /// <param name="suffixIndex">The zero-based index of the first occurrence, if found; otherwise, -1.</param>
        /// <param name="values">The specified strings to compare to the end of <paramref name="text"/>.</param>
        /// <param name="ignoreCase"><see langword="true"/> to ignore case during the comparison; otherwise, <see langword="false"/>.</param>
        /// <param name="culture">
        /// Cultural information that determines how <paramref name="text"/> and other <see cref="string"/> are compared.
        /// If culture is null, the current culture is used.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the end of <paramref name="text"/> matches any of the strings of <paramref name="values"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool EndsWith(this string text, out int suffixIndex, IEnumerable<string> values, bool ignoreCase, CultureInfo culture)
        { 
            suffixIndex = values.FindIndex(s => text.EndsWith(s, ignoreCase, culture));
            return suffixIndex >= 0;
        }

        #endregion

        #region FindOcurrences

        /// <summary>
        /// Searches all present occurrences of a pattern in a <see cref="string"/> and returns the indices where each occurrence starts.
        /// </summary>        
        /// <param name="text">The string to search for.</param>
        /// <param name="pattern">The string to seek.</param>
        /// <param name="ignoreCase"><see langword="true"/> to ignore case; <see langword="false"/> to regard case.</param>
        /// <returns>
        /// The indices of where each found occurrence starts.
        /// </returns>
        public static int[] FindOcurrences(this string text, string pattern, bool ignoreCase = false)
        {
            Regex regex;

            if (ignoreCase)
                regex = new Regex(Regex.Escape(pattern), RegexOptions.IgnoreCase);
            else
                regex = new Regex(Regex.Escape(pattern));

            return regex.Matches(text).Select(m => m.Index).ToArray();
        }

        /// <summary>
        /// Searches all present occurrences of a pattern in a <see cref="string"/> and returns the indices where each occurrence starts.
        /// </summary>        
        /// <param name="text">The string to search for.</param>
        /// <param name="pattern">The string to seek.</param>
        /// <returns>
        /// The indices of where each found occurrence starts.
        /// </returns>
        public static int[] FindOcurrences(this string text, string pattern)
        {
            return text.FindOcurrences(pattern, false);
        }

        #endregion

        #region RemoveAccents

        /// <summary>
        /// Returns a copy of a <see cref="string"/> without accents.
        /// </summary>        
        /// <param name="text">The string to remove specified accents.</param>
        /// <param name="ignore">The characters with accent to ignore.</param>
        /// <returns>
        /// The specified <see cref="string"/> without accents.
        /// </returns>
        public static string RemoveAccents(this string text, string ignore = "")
        {
            StringBuilder stringBuilder = new StringBuilder();
            IEnumerable<char> textNormalized = text.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);

            textNormalized.ForEach((i, cn) =>
            {
                char c = text[i];

                if (ignore.Contains(c))
                {
                    stringBuilder.Append(c);
                }
                else
                {
                    stringBuilder.Append(cn);
                }
            });

            return stringBuilder.ToString();
        }

        #endregion

        #region Repeat
        /// <summary>
        /// Returns a new <see cref="string"/> which contains the specified number of copies of the <see cref="string"/> on which it was called, concatenated together.
        /// </summary>        
        /// <param name="text">The <see cref="string"/> to repeat.</param>
        /// <param name="count">An <see cref="int"/> indicating the number of times to repeat the <see cref="string"/>.</param>
        /// <returns>
        /// A new <see cref="string"/> containing the specified number of copies of the given <see cref="string"/>.
        /// </returns>
        public static string Repeat(this string text, int count)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < count; i++)
                stringBuilder.Append(text);

            return stringBuilder.ToString();
        }

        #endregion

        #region StartsWith

        /// <summary>
        /// Determines if the start of a<see cref="string"/> matches any of the specified strings.
        /// </summary>        
        /// <param name="text">The string to compare to the substrings at its start.</param>
        /// <param name="prefixIndex">The zero-based index of the first occurrence, if found; otherwise, -1.</param>
        /// <param name="values">The specified strings to compare to the start of <paramref name="text"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the start of <paramref name="text"/> matches any of the strings of <paramref name="values"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool StartsWith(this string text, out int prefixIndex, IEnumerable<string> values)
        {
            prefixIndex = values.FindIndex(s => text.StartsWith(s));
            return prefixIndex >= 0;
        }

        /// <summary>
        /// Determines if the start of a <see cref="string"/> matches any of the specified strings.
        /// </summary>        
        /// <param name="text">The string to compare to the substrings at its start.</param>
        /// <param name="prefixIndex">The zero-based index of the first occurrence, if found; otherwise, -1.</param>
        /// <param name="values">The specified strings to compare to the start of <paramref name="text"/>.</param>
        /// <param name="comparisonType">One of the enumeration values that determines how <paramref name="text"/> and other <see cref="string"/> are compared.</param>
        /// <returns>
        /// <see langword="true"/> if the start of <paramref name="text"/> matches any of the strings of <paramref name="values"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool StartsWith(this string text, out int prefixIndex, IEnumerable<string> values, StringComparison comparisonType)
        {
            prefixIndex = values.FindIndex(s => text.StartsWith(s, comparisonType));
            return prefixIndex >= 0;
        }

        /// <summary>
        /// Determines if the start of a <see cref="string"/> matches any of the specified strings.
        /// </summary>        
        /// <param name="text">The string to compare to the substrings at its start.</param>
        /// <param name="prefixIndex">The zero-based index of the first occurrence, if found; otherwise, -1.</param>
        /// <param name="values">The specified strings to compare to the start of <paramref name="text"/>.</param>
        /// <param name="ignoreCase"><see langword="true"/> to ignore case during the comparison; otherwise, <see langword="false"/>.</param>
        /// <param name="culture">
        /// Cultural information that determines how <paramref name="text"/> and other <see cref="string"/> are compared.
        /// If culture is null, the current culture is used.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the start of <paramref name="text"/> matches any of the strings of <paramref name="values"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool StartsWith(this string text, out int prefixIndex, IEnumerable<string> values, bool ignoreCase, CultureInfo culture)
        {
            prefixIndex = values.FindIndex(s => text.StartsWith(s, ignoreCase, culture));
            return prefixIndex >= 0;
        }

        #endregion

        #region Base64

        /// <summary>
        /// Returns a copy of a <see cref="string"/> that is encoded with base-64 digits.
        /// </summary>        
        /// <param name="text">The string to encoded.</param>
        /// <param name="encoding">The source encoding of <paramref name="text"/>.</param>
        /// <returns>
        /// The specified <see cref="string"/> encoded with base-64 digits.
        /// </returns>
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

        /// <summary>
        /// Returns a copy of <paramref name="text"/> that is encoded with base-64 digits.
        /// </summary>        
        /// <param name="text">The string to encoded.</param>
        /// <returns>
        /// The specified <see cref="string"/> encoded with base-64 digits.
        /// </returns>
        public static string ToBase64(this string text)
        {
            return ToBase64(text, Encoding.UTF8);
        }

        /// <summary>
        /// Tries to decode a string that is encoded with base-64 digits and encodes it to another specified encoding.
        /// </summary>        
        /// <param name="text">The <see cref="string"/> to decoded.</param>
        /// <param name="encoding">The destination encoding of <paramref name="text"/>.</param>
        /// <param name="decodedText">If the decoding succeeded, this contains the decoded <see cref="string"/>, or null if the decoding failed.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was decoded successfully; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryFromBase64(this string text, Encoding encoding, out string decodedText)
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

        /// <summary>
        /// Tries to decode a string that is encoded with base-64 digits and encodes it to the default encoding.
        /// </summary>        
        /// <param name="text">The <see cref="string"/> to decoded.</param>
        /// <param name="decodedText">If the decoding succeeded, this contains the decoded <see cref="string"/>, or null if the decoding failed.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was decoded successfully; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryFromBase64(this string text, out string decodedText)
        {
            return TryFromBase64(text, Encoding.Default, out decodedText);
        }

        #endregion

        #region ToTitleCase

        /// <summary>
        /// Returns a copy of a <see cref="string"/> converted to title case.
        /// </summary>        
        /// <param name="text">The string to convert.</param>
        /// <param name="onlyFirst"><see langword="true"/> to set upper case only the first character; <see langword="false"/> to convert all.</param>
        /// <returns>
        /// The specified <see cref="string"/> converted to title case.
        /// </returns>
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

        /// <summary>
        /// Returns a copy of a <see cref="string"/> converted to title case (except for words 
        /// that are entirely in uppercase, which are considered to be acronyms).
        /// </summary>        
        /// <param name="text">The string to convert.</param>
        /// <param name="cultureInfo">The <see cref="CultureInfo"/> that defines the writing system.</param>
        /// <returns>
        /// The specified <see cref="string"/> converted to title case.
        /// </returns>
        public static string ToTitleCase(this string text, CultureInfo cultureInfo)
        {
            return cultureInfo.TextInfo.ToTitleCase(text);
        }

        #endregion

        #region Translate

        /// <summary>
        /// Returns a copy of a <see cref="string"/> in which all occurrences of all specified replacements are replaced.
        /// </summary>        
        /// <param name="text">The string to replace with the specified replacements.</param>
        /// <param name="replacements">The replacements that define which <see cref="string"/> is replaced by another <see cref="string"/>.</param>
        /// <returns>
        /// The specified <see cref="string"/> with all replacements applied.
        /// </returns>
        public static string Translate(this string text, Dictionary<string, string> replacements)
        {
            if (replacements.Count == 0) return text;

            StringBuilder result = new StringBuilder();
            StringBuilder pattern = new StringBuilder();
            int countIndex = 0;

            while (countIndex < text.Length)
            {
                bool isReplaced = false;

                for (int i = countIndex; i < text.Length && !isReplaced; i++)
                {
                    pattern.Append(text[i]);

                    if (replacements.TryGetValue(pattern.ToString(), out string newValue))
                    {
                        result.Append(newValue);
                        isReplaced = true;
                    }
                }

                if (isReplaced)
                {
                    countIndex += pattern.Length;
                }
                else
                {
                    result.Append(text[countIndex]);
                    countIndex++;
                }

                pattern.Clear();
            }

            return result.ToString();
        }

        #endregion

    }
}
