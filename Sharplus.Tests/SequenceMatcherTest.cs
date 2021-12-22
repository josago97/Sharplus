using Sharplus.SequenceMatching;
using Xunit;

namespace Sharplus.Tests
{
    public class SequenceMatcherTest
    {
        #region Levenshtein

        [Theory]
        [InlineData(0, "test", "test")]
        [InlineData(2, "test", "estt")]
        [InlineData(1, "test", "est")]
        [InlineData(2, "test", "restt")]
        public void LevenshteinDistance(int expected, string sequence1, string sequence2)
        {
            ISequenceDistance matcher = new Levenshtein();
            Assert.Equal(expected, matcher.Distance(sequence1, sequence2));
        }

        [Theory]
        [InlineData(0, "test", "test")]
        [InlineData(0.5, "test", "estt")]
        [InlineData(0.25, "test", "est")]
        [InlineData(0.4, "test", "restt")]
        public void NormalizedLevenshteinDistance(double expected, string sequence1, string sequence2)
        {
            INormalizedSequenceDistance matcher = new NormalizedLevenshtein();
            Assert.Equal(expected, matcher.Distance(sequence1, sequence2));
        }

        [Theory]
        [InlineData(1, "test", "test")]
        [InlineData(0.5, "test", "estt")]
        [InlineData(0.75, "test", "est")]
        [InlineData(0.6, "test", "restt")]
        public void NormalizedLevenshteinSimilarity(double expected, string sequence1, string sequence2)
        {
            INormalizedSequenceSimilarity matcher = new NormalizedLevenshtein();
            Assert.Equal(expected, matcher.Similarity(sequence1, sequence2));
        }

        #endregion

        #region Damerau

        [Theory]
        [InlineData(0, "test", "test")]
        [InlineData(2, "test", "estt")]
        [InlineData(1, "test", "est")]
        [InlineData(2, "test", "restt")]
        public void DamerauDistance(int expected, string sequence1, string sequence2)
        {
            ISequenceDistance matcher = new Damerau();
            Assert.Equal(expected, matcher.Distance(sequence1, sequence2));
        }

        #endregion

        #region Longest Common Subsequence

        [Theory]
        [InlineData(0, "test", "test")]
        [InlineData(2, "test", "estt")]
        [InlineData(1, "test", "est")]
        [InlineData(3, "test", "restt")]
        public void LongestCommonSubsequenceDistance(int expected, string sequence1, string sequence2)
        {
            ISequenceDistance matcher = new LongestCommonSubsequence();
            Assert.Equal(expected, matcher.Distance(sequence1, sequence2));
        }

        #endregion
    }
}
