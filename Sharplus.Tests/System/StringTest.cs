using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sharplus.Tests.System
{
    public class StringTest
    {
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

    }
}
