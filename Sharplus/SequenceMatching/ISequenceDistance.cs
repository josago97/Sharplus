using System.Collections.Generic;
using Sharplus.System;

namespace Sharplus.SequenceMatching
{
    public interface ISequenceDistance
    {
        /// <summary>
        /// Calculates the distance between two sequences.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the first sequence.</typeparam>
        /// <typeparam name="G">The type of the elements of the second sequence.</typeparam>
        /// <param name="sequence1">The first sequence to compare.</param>
        /// <param name="sequence2">The second sequence to compare.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T, G}"/> to compare values.</param>
        /// <returns>
        /// The distance between the two sequences.
        /// </returns>
        public double Distance<T, G>(IEnumerable<T> sequence1, IEnumerable<G> sequence2, IEqualityComparer<T, G> comparer);

        /// <summary>
        /// Calculates the distance between two sequences.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the first sequence.</typeparam>
        /// <typeparam name="G">The type of the elements of the second sequence.</typeparam>
        /// <param name="sequence1">The first sequence to compare.</param>
        /// <param name="sequence2">The second sequence to compare.</param>
        /// <returns>
        /// The distance between the two sequences.
        /// </returns>
        public double Distance<T, G>(IEnumerable<T> sequence1, IEnumerable<G> sequence2);
    }
}
