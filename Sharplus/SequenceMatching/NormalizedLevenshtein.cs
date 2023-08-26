using System;
using System.Collections.Generic;
using System.Linq;
using Sharplus.System;

namespace Sharplus.SequenceMatching
{
    public class NormalizedLevenshtein : INormalizedSequenceDistance, INormalizedSequenceSimilarity
    {
        private Levenshtein _levenshtein = new Levenshtein();

        public double Distance<T, G>(IEnumerable<T> sequence1, IEnumerable<G> sequence2, IEqualityComparer<T, G> comparer)
        {
            T[] array1 = sequence1.ToArray();
            G[] array2 = sequence2.ToArray();
            int maxLength = Math.Max(array1.Length, array2.Length);

            return _levenshtein.Distance(array1, array2, comparer) / maxLength;
        }

        public double Distance<T, G>(IEnumerable<T> sequence1, IEnumerable<G> sequence2)
        {
            return Distance(sequence1, sequence2, EqualityComparer<T, G>.Default);
        }

        public double Similarity<T, G>(IEnumerable<T> sequence1, IEnumerable<G> sequence2, IEqualityComparer<T, G> comparer)
        {
            return 1 - Distance(sequence1, sequence2, comparer);
        }

        public double Similarity<T, G>(IEnumerable<T> sequence1, IEnumerable<G> sequence2)
        {
            return Similarity(sequence1, sequence2, EqualityComparer<T, G>.Default);
        }
    }
}
