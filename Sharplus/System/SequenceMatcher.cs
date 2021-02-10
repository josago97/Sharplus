using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public class SequenceMatcher<T, G>
    {
        private T[] _sequence1;
        private G[] _sequence2;
        private Func<T, G, bool> _equalsFunc;

        public SequenceMatcher(IEnumerable<T> sequence1, IEnumerable<G> sequence2, Func<T, G, bool> equals)
        {
            _sequence1 = sequence1.ToArray();
            _sequence2 = sequence2.ToArray();
            _equalsFunc = equals;
        }

        public SequenceMatcher(IEnumerable<T> sequence1, IEnumerable<G> sequence2) 
            : this(sequence1, sequence2, (t, g) => t.Equals(g))
        {
        }

        public double Levenshtein()
        {
            return CalculateLevenshtein(_sequence1, _sequence2);
        }

        public double NormalizedLevenshtein()
        {
            int maxLength = Math.Max(_sequence1.Length, _sequence2.Length);
            double result = 1 - (Levenshtein() / maxLength);

            return result;
        }

        private double CalculateLevenshtein(T[] sequence1, G[] sequence2)
        {
            double result;

            if (sequence1.Length == 0) result = sequence2.Length;
            else if (sequence2.Length == 0) result = sequence1.Length;
            else
            {
                T[] tailSequence1 = sequence1.Skip(1).ToArray();
                G[] tailSequence2 = sequence2.Skip(1).ToArray();

                if (_equalsFunc(sequence1[0], sequence2[0]))
                {
                    result = CalculateLevenshtein(tailSequence1, tailSequence2);
                }
                else
                {
                    double rts1s2 = CalculateLevenshtein(tailSequence1, sequence2);
                    double rs1st2 = CalculateLevenshtein(sequence1, tailSequence2);
                    double rts1st2 = CalculateLevenshtein(tailSequence1, tailSequence2);

                    double min = MathPlus.Min(rts1s2, rs1st2, rts1st2);

                    result = 1 + min;
                }
            }

            return result;
        }

        /*public double DamerauLevenshtein()
        {
            return CalculateDamerauLevenshtein(_sequence1, _sequence2);
        }
        
        private double CalculateDamerauLevenshtein(T[] sequence1, G[] sequence2)
        {
            double result;

            if (s1.Equals(s2))
            {
                return 0;
            }

            // Infinite distance is the max possible distance
            int inf = int.MaxValue;

            // Create and initialize the character array indices
            var da = new Dictionary<char, int>();

            for (int d = 0; d < s1.Length; d++)
            {
                da[s1[d]] = 0;
            }

            for (int d = 0; d < s2.Length; d++)
            {
                da[s2[d]] = 0;
            }

            // Create the distance matrix H[0 .. s1.length+1][0 .. s2.length+1]
            int[,] h = new int[sequence1.Length + 2, sequence2.Length + 2];

            // Initialize the left and top edges of H
            for (int i = 0; i <= s1.Length; i++)
            {
                h[i + 1, 0] = inf;
                h[i + 1, 1] = i;
            }

            for (int j = 0; j <= s2.Length; j++)
            {
                h[0, j + 1] = inf;
                h[1, j + 1] = j;
            }

            // Fill in the distance matrix H
            // Look at each character in s1
            for (int i = 1; i <= s1.Length; i++)
            {
                int db = 0;

                // Look at each character in b
                for (int j = 1; j <= s2.Length; j++)
                {
                    int i1 = da[s2[j - 1]];
                    int j1 = db;

                    int cost = 1;
                    if (s1[i - 1] == s2[j - 1])
                    {
                        cost = 0;
                        db = j;
                    }

                    h[i + 1, j + 1] = MathPlus.Min(
                        h[i, j] + cost,  // Substitution
                        h[i + 1, j] + 1, // Insertion
                        h[i, j + 1] + 1, // Deletion
                        h[i1, j1] + (i - i1 - 1) + 1 + (j - j1 - 1)
                    );
                }

                da[s1[i - 1]] = i;
            }

            return h[s1.Length + 1, s2.Length + 1];
        }*/
    }
}
