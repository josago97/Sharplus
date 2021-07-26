using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sharplus.Tests.System
{
    public class SequenceMatcherTest
    {
        #region Levenshtein

        [Theory]
        [InlineData(0, "test", "test")]
        [InlineData(2, "test", "estt")]
        [InlineData(1, "test", "est")]
        [InlineData(2, "test", "restt")]
        public void DistanceLevenshtein(int expected, IEnumerable<char> sequence1, IEnumerable<char> sequence2)
        {
            var matcher = new SequenceMatcher<char, char>(sequence1, sequence2);
            Assert.Equal(expected, matcher.LevenshteinDistance());
        }

        [Theory]
        [InlineData(0, "test", "test")]
        [InlineData(0.5, "test", "estt")]
        [InlineData(0.25, "test", "est")]
        [InlineData(0.4, "test", "restt")]
        public void DistanceNormalizedLevenshtein(double expected, IEnumerable<char> sequence1, IEnumerable<char> sequence2)
        {
            var matcher = new SequenceMatcher<char, char>(sequence1, sequence2);
            Assert.Equal(expected, matcher.LevenshteinNormalizedDistance());
        }

        [Theory]
        [InlineData(1, "test", "test")]
        [InlineData(0.5, "test", "estt")]
        [InlineData(0.75, "test", "est")]
        [InlineData(0.6, "test", "restt")]
        public void SimilarityNormalizedLevenshtein(double expected, IEnumerable<char> sequence1, IEnumerable<char> sequence2)
        {
            var matcher = new SequenceMatcher<char, char>(sequence1, sequence2);
            Assert.Equal(expected, matcher.LevenshteinNormalizedSimilarity());
        }

        #endregion

        #region Damerau

        [Theory]
        [InlineData(0, "test", "test")]
        [InlineData(2, "test", "estt")]
        [InlineData(1, "test", "est")]
        [InlineData(2, "test", "restt")]
        public void DistanceDamerau(int expected, IEnumerable<char> sequence1, IEnumerable<char> sequence2)
        {
            var matcher = new SequenceMatcher<char, char>(sequence1, sequence2);
            Assert.Equal(expected, matcher.DamerauDistance());
        }

        #endregion

        #region Longest Common Subsequence

        [Theory]
        [InlineData(0, "test", "test")]
        [InlineData(2, "test", "estt")]
        [InlineData(1, "test", "est")]
        [InlineData(3, "test", "restt")]
        public void DistanceLongestCommonSubsequence(int expected, IEnumerable<char> sequence1, IEnumerable<char> sequence2)
        {
            var matcher = new SequenceMatcher<char, char>(sequence1, sequence2, (c1, c2) => c1 == c2);
            Assert.Equal(expected, matcher.LongestCommonSubsequenceDistance());
        }

        #endregion
    }
}
