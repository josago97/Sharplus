﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Sharplus.SequenceMatching
{
    public class Damerau : ISequenceMetricDistance
    {
        public double Distance<T, G>(IEnumerable<T> sequence1, IEnumerable<G> sequence2, IEqualityComparer<T, G> comparer)
        {
            return Calculate(sequence1.ToArray(), sequence2.ToArray(), comparer);
        }

        public double Distance<T, G>(IEnumerable<T> sequence1, IEnumerable<G> sequence2)
        {
            return Distance(sequence1, sequence2, EqualityComparer<T, G>.Default);
        }

        private double Calculate<T, G>(T[] sequence1, G[] sequence2, IEqualityComparer<T, G> comparer)
        {
            int[,] h = new int[sequence1.Length + 1, sequence2.Length + 1];

            for (int i = 0; i < h.GetLength(0); i++) h[i, 0] = i;
            for (int j = 0; j < h.GetLength(1); j++) h[0, j] = j;

            for (int i = 1; i < h.GetLength(0); i++)
            {
                for (int j = 1; j < h.GetLength(1); j++)
                {
                    int cost = comparer.Equals(sequence1[i - 1], sequence2[j - 1]) ? 0 : 1;

                    h[i, j] = MathPlus.Min(
                        h[i - 1, j] + 1,  // Deletion
                        h[i, j - 1] + 1, // Insertion
                        h[i - 1, j - 1] + cost); // Substitution

                    if (i > 1 && j > 1
                        && comparer.Equals(sequence1[i - 1], sequence2[j - 2])
                        && comparer.Equals(sequence1[i - 2], sequence2[j - 1]))
                    {
                        h[i, j] = Math.Min(
                            h[i, j],
                            h[i - 2, j - 2] + cost //Transposition
                        );
                    }
                }
            }

            return h[sequence1.Length, sequence2.Length];
        }
    }
}
