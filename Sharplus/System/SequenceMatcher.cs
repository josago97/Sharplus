using System.Collections.Generic;
using System.Linq;

namespace System
{
    public class SequenceMatcher<T, G>
    {
        private T[] _sequence1;
        private G[] _sequence2;
        private Func<T, G, bool> _equals;

        public SequenceMatcher(IEnumerable<T> sequence1, IEnumerable<G> sequence2, Func<T, G, bool> equals)
        {
            _sequence1 = sequence1.ToArray();
            _sequence2 = sequence2.ToArray();
            _equals = equals;
        }

        public SequenceMatcher(IEnumerable<T> sequence1, IEnumerable<G> sequence2) 
            : this(sequence1, sequence2, (t, g) => t.Equals(g))
        {
        }

        #region Levenshtein

        public double LevenshteinDistance()
        {
            return CalculateLevenshtein(_sequence1, _sequence2);
        }

        public double LevenshteinNormalizedDistance()
        {
            int maxLength = Math.Max(_sequence1.Length, _sequence2.Length);
            return LevenshteinDistance() / maxLength;
        }

        public double LevenshteinNormalizedSimilarity()
        {
            return 1 - LevenshteinNormalizedDistance();
        }

        private double CalculateLevenshtein(T[] sequence1, G[] sequence2)
        {
            int[,] d = new int[sequence1.Length + 1, sequence2.Length + 1];

            for (int i = 1; i < d.GetLength(0); i++)
            {
                for (int j = 1; j < d.GetLength(1); j++)
                {
                    int cost = _equals(sequence1[i - 1], sequence2[j - 1]) ? 0 : 1;

                    d[i, j] = MathPlus.Min(
                        d[i - 1, j] + 1,  // Deletion
                        d[i, j - 1] + 1, // Insertion
                        d[i - 1, j - 1] + cost // Substitution
                        );
                }
            }

            return d[sequence1.Length, sequence2.Length];
        }

        #endregion

        #region Damerau

        public double DamerauDistance()
        {
            return CalculateDamerau(_sequence1, _sequence2);
        }

        private double CalculateDamerau(T[] sequence1, G[] sequence2)
        {
            int[,] d = new int[sequence1.Length + 1, sequence2.Length + 1];

            for (int i = 1; i < d.GetLength(0); i++)
            {
                for (int j = 1; j < d.GetLength(1); j++)
                {
                    int cost = _equals(sequence1[i - 1], sequence2[j - 1]) ? 0 : 1;

                    d[i, j] = MathPlus.Min(
                        d[i - 1, j] + 1,  // Deletion
                        d[i, j - 1] + 1, // Insertion
                        d[i - 1, j - 1] + cost // Substitution
                        );

                    if (i > 1 && j > 1
                        && _equals(sequence1[i - 1], sequence2[j - 2])
                        && _equals(sequence1[i - 2], sequence2[j - 1]))
                    {
                        d[i, j] = Math.Min(
                            d[i, j],
                            d[i - 2, j - 2] + 1 //Transposition
                        );
                    }
                }
            }

            return d[sequence1.Length, sequence2.Length];
        }

        #endregion

        #region Longest Common Subsequence

        public double LongestCommonSubsequenceDistance()
        {
            return CalculateLongestCommonSubsequence(_sequence1, _sequence2);
        }

        public double CalculateLongestCommonSubsequence(T[] sequence1, G[] sequence2)
        {
            int length1 = sequence1.Length;
            int length2 = sequence2.Length;

            int[,] d = new int[length1 + 1, length2 + 1];

            for (int i = 1; i < d.GetLength(0); i++)
            {
                for (int j = 1; j < d.GetLength(1); j++)
                {
                    if (_equals(sequence1[i - 1], sequence2[j - 1]))
                    {
                        d[i, j] = d[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        d[i, j] = Math.Max(d[i, j - 1], d[i - 1, j]);
                    }
                }
            }

            return length1 + length2 - 2 * d[length1, length2];
        }

        #endregion
    }
}
