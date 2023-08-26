using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Sharplus.System;

namespace Sharplus.Tests.System
{
    public class StringTest
    {
        [Theory]
        [InlineData(false, -1, "hello", new string[] { })]
        [InlineData(true, 0, "hello", new string[] { "hello" })]
        [InlineData(true, 1, "hello", new string[] { "a", "llo" })]
        public void EndsWith(bool expected, int indexExpected, string text, string[] values)
        {
            bool result = text.EndsWith(out int index, values);
            Assert.Equal(expected, result);
            Assert.Equal(indexExpected, index);
        }

        [Theory]
        [InlineData(new int[] { 0, 1, 2 }, "hi", "", false)]
        [InlineData(new int[] { 0, 1, 2 }, "hi", "", true)]
        [InlineData(new int[] { }, "hello world", "HeLLo", false)]
        [InlineData(new int[] { 0 }, "hello world", "hello", false)]
        [InlineData(new int[] { 0 }, "hello world", "HeLLo", true)]
        [InlineData(new int[] { }, "hello world", "L", false)]
        [InlineData(new int[] { 2, 3, 9 }, "hello world", "l", false)]
        [InlineData(new int[] { 2, 3, 9 }, "hello world", "L", true)]
        [InlineData(new int[] { }, "test test test", "EST", false)]
        [InlineData(new int[] { 1, 6, 11 }, "test test test", "est", false)]
        [InlineData(new int[] { 1, 6, 11 }, "test test test", "EST", true)]
        public void FindOcurrences(int[] expected, string text, string pattern, bool ignoreCase)
        {
            int[] result = text.FindOcurrences(pattern, ignoreCase);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("hello", "hello", "ñ")]
        [InlineData("a", "á", "")]
        [InlineData("añ", "añ", "ñ")]
        [InlineData("añ", "áñ", "ñ")]
        public void RemoveAccents(string expected, string text, string ignore)
        {
            Assert.Equal(expected, text.RemoveAccents(ignore));
        }

        [Theory]
        [InlineData("", "hello", -1)]
        [InlineData("", "hello", 0)]
        [InlineData("hello", "hello", 1)]
        [InlineData("hellohello", "hello", 2)]
        public void Repeat(string expected, string text, int count)
        {
            Assert.Equal(expected, text.Repeat(count));
        }

        [Theory]
        [InlineData(false, -1, "hello", new string[] { })]
        [InlineData(true, 0, "hello", new string[] { "hello" })]
        [InlineData(true, 1, "hello", new string[] { "a", "he" })]
        public void StartsWith(bool expected, int indexExpected, string text, string[] values)
        {
            bool result = text.StartsWith(out int index, values);
            Assert.Equal(expected, result);
            Assert.Equal(indexExpected, index);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("A", "a")]
        [InlineData("A", "A")]
        [InlineData("Aa", "aa")]
        [InlineData("Aa a", "aa a")]
        [InlineData("Aa A", "aa a", false)]
        public void ToTitleCase(string expected, string text, bool onlyFirst = true)
        {
            Assert.Equal(expected, text.ToTitleCase(onlyFirst));
        }

        public static IEnumerable<object[]> TranslateData => new List<object[]>
        {
            new object[] { "hola", "hola", new Dictionary<string, string>() },
            new object[] { "ola", "hola", new Dictionary<string, string>(){ { "h", "" } } },
            new object[] { "hola", "hola", new Dictionary<string, string>() { { "x", "" } } },
            new object[] { "hola", "hola", new Dictionary<string, string>() { { "x", "h" } } },
            new object[] { "ola", "hola", new Dictionary<string, string>() { { "h", "" } } },
            new object[] { "la", "hola", new Dictionary<string, string>() { { "h", "" }, { "o", "" } } },
            new object[] { "holhholh", "holahola", new Dictionary<string, string>() { {"a","h"} } },
            new object[] { "olholh", "holahola", new Dictionary<string, string>() { { "a", "h" }, { "h", "" } } },
            new object[] { "--la--la", "holahola", new Dictionary<string, string>() { { "ho", "--" } } },
            new object[] { "-la-la", "holahola", new Dictionary<string, string>() { { "ho", "-" } } },
            new object[] { "laholaho", "holahola", new Dictionary<string, string>() { { "ho", "la" }, { "la", "ho" } } },
            new object[] { "llohellohe", "hellohello", new Dictionary<string, string>() { { "he", "llo" }, { "llo", "he" } } },
            new object[] { "he", "hello", new Dictionary<string, string>() { { "he", "" }, { "llo", "he" } } },
            new object[] { "hehe", "hellohello", new Dictionary<string, string>() { { "he", "" }, { "llo", "he" } } },
            new object[] { "-he", "hello", new Dictionary<string, string>() { { "llo", "he" }, { "he", "-" } } },
            new object[] { "-he-he", "hellohello", new Dictionary<string, string>() { { "llo", "he" }, { "he", "-" } } },
            new object[] { "sxdsxd", "asdasd", new Dictionary<string, string>() { { "a", "s" }, { "s", "x" } } },
            new object[] { "dxdx", "asdasd", new Dictionary<string, string>() { { "as", "d" }, { "d", "x" } } },
            new object[] { "xx", "asdasd", new Dictionary<string, string>() { { "as", "" }, { "d", "x" } } },
        };

        [Theory]
        [MemberData(nameof(TranslateData))]
        public void Translate(string expected, string text, Dictionary<string, string> replacements)
        {
            Assert.Equal(expected, text.Translate(replacements));
        }
    }
}
