using System;
using System.Collections.Generic;
using System.Linq;

namespace Sharplus.SequenceMatching
{
    public class NormalizedLevenshtein : INormalizedSequenceDistance, INormalizedSequenceSimilarity
    {
        private readonly Levenshtein _levenshtein = new Levenshtein();

        public double Distance<T, G>(IEnumerable<T> sequence1, IEnumerable<G> sequence2, IEqualityComparer<T, G> comparer)
        {
            return Calculate(sequence1.ToArray(), sequence2.ToArray(), comparer);
        }

        public double Similarity<T, G>(IEnumerable<T> sequence1, IEnumerable<G> sequence2, IEqualityComparer<T, G> comparer)
        {
            return 1 - Distance(sequence1, sequence2, comparer);
        }

        private double Calculate<T, G>(T[] sequence1, G[] sequence2, IEqualityComparer<T, G> comparer)
        {
            int maxLength = Math.Max(sequence1.Length, sequence2.Length);
            return _levenshtein.Distance(sequence1, sequence2, comparer) / maxLength;
        }
    }
}
