using System;
using System.Collections.Generic;
using System.Linq;
using Sharplus.System;

namespace Sharplus.SequenceMatching
{
    public class LongestCommonSubsequence : ISequenceDistance
    {
        public double Distance<T, G>(IEnumerable<T> sequence1, IEnumerable<G> sequence2, IEqualityComparer<T, G> comparer)
        {
            return Calculate(sequence1.ToArray(), sequence2.ToArray(), comparer);
        }

        public double Calculate<T, G>(T[] sequence1, G[] sequence2, IEqualityComparer<T, G> comparer)
        {
            int length1 = sequence1.Length;
            int length2 = sequence2.Length;

            int[,] d = new int[length1 + 1, length2 + 1];

            for (int i = 1; i < d.GetLength(0); i++)
            {
                for (int j = 1; j < d.GetLength(1); j++)
                {
                    if (comparer.Equals(sequence1[i - 1], sequence2[j - 1]))
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

        public double Distance<T, G>(IEnumerable<T> sequence1, IEnumerable<G> sequence2)
        {
            return Distance(sequence1, sequence2, EqualityComparer<T, G>.Default);
        }
    }
}
