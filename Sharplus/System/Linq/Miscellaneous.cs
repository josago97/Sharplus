using System.Collections;
using System.Collections.Generic;
using Sharplus.System.Linq;

namespace System.Linq
{
    public static class Miscellaneous
    {
        #region Batch

        /// <summary>
        /// Batches the source sequence into sized sequences.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="size">Size of buckets.</param>
        /// <returns>A sequence of equally sized sequences containing elements of the source collection.</returns>
        public static IEnumerable<IEnumerable<TSource>> Batch<TSource>(this IEnumerable<TSource> source, int size)
        {
            if (source == null) throw Error.ArgumentNull("source");
            if (size < 0) throw Error.BadArguments("size cannot be negative");

            List<TSource> batch = new List<TSource>(size);

            if (size > 0)
            {
                foreach (var item in source)
                {
                    batch.Add(item);

                    if (batch.Count == size)
                    {
                        yield return batch;
                        batch.Clear();
                    }
                }

                if (batch.Count > 0) yield return batch;
            }
        }

        #endregion

        #region Concat

        /// <summary>
        /// Concatenates sequences.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="source">The first sequence to concatenate.</param>
        /// <param name="sequences">The sequences to concatenate to the first sequence.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TSource}"/> that contains the concatenated elements of the input sequences.
        /// </returns>
        public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> source, IEnumerable<IEnumerable<TSource>> sequences)
        {
            if (source == null) throw Error.ArgumentNull("source");
            if (sequences == null) throw Error.ArgumentNull("sequences");

            foreach (var item in source) yield return item;
            foreach (var sequence in sequences)
            {
                if (sequence == null) throw Error.ArgumentNull("A sequence is null");
                else foreach (var item in sequence) yield return item;
            }
        }

        /// <summary>
        /// Concatenates sequences.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="source">The first sequence to concatenate.</param>
        /// <param name="sequences">The sequences to concatenate to the first sequence.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TSource}"/> that contains the concatenated elements of the input sequences.
        /// </returns>
        public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> source, params IEnumerable<TSource>[] sequences)
        {
            return source.Concat((IEnumerable<IEnumerable<TSource>>)sequences);
        }

        #endregion

        #region Dump

        /// <summary>
        /// Shows the content of the sequence in a <see cref="string"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to show.</param>
        /// <returns>A <see cref="string"/> with the content of the sequence.</returns>
        public static string Dump<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw Error.ArgumentNull("source");

            return $"[{string.Join(", ", source)}]";
        }

        #endregion

        #region FindAllIndexes

        /// <summary>
        /// Searches for all elements that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the all occurrences within the
        /// range of elements in the <see cref="IEnumerable{TSource}"/> that starts at the
        /// specified index and contains the specified number of elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">A function that defines the conditions of the element to search for.</param>
        /// <returns>
        /// An <see cref="IEnumerable{int}"/> that contains the zero-based indexes of the all occurrences 
        /// of all elements that match the conditions defined by match.
        /// </returns>
        public static IEnumerable<int> FindAllIndexes<TSource>(this IEnumerable<TSource> source, int startIndex, int count, Func<TSource, bool> match)
        {
            if (source == null) throw Error.ArgumentNull("source");
            if (match == null) throw Error.ArgumentNull("match");

            int indexOffset = 0;

            foreach (TSource item in source.SubSequence(startIndex, count))
            {
                if (match(item))
                {
                    yield return startIndex + indexOffset;
                }

                indexOffset++;
            }
        }

        /// <summary>
        /// Searches for all elements that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the all occurrences within the
        /// range of elements in the <see cref="IEnumerable{TSource}" /> that extends from
        /// the specified index to the last element.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="match">A function that defines the conditions of the element to search for.</param>
        /// <returns>
        /// An <see cref="IEnumerable{int}"/> that contains the zero-based indexes of the all occurrences 
        /// of all elements that match the conditions defined by match.
        /// </returns>
        public static IEnumerable<int> FindAllIndexes<TSource>(this IEnumerable<TSource> source, int startIndex, Func<TSource, bool> match)
        {
            return source.FindAllIndexes(startIndex, -1, match);
        }

        /// <summary>
        /// Searches for all elements that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the all occurrences within the
        /// entire <see cref="IEnumerable{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">A function that defines the conditions of the element to search for.</param>
        /// <returns>
        /// An <see cref="IEnumerable{int}"/> that contains the zero-based indexes of the all occurrences 
        /// of all elements that match the conditions defined by match.
        /// </returns>
        public static IEnumerable<int> FindAllIndexes<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> match)
        {
            return source.FindAllIndexes(0, -1, match);
        }

        #endregion

        #region FindIndex

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the first occurrence within the
        /// range of elements in the <see cref="IEnumerable{TSource}"/> that starts at the
        /// specified index and contains the specified number of elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">A function that defines the conditions of the element to search for.</param>
        /// <returns>
        /// The zero-based index of the first occurrence of an element that matches the conditions
        /// defined by match, if found; otherwise, -1.
        /// </returns>
        public static int FindIndex<TSource>(this IEnumerable<TSource> source, int startIndex, int count, Func<TSource, bool> match)
        {
            return source.FindAllIndexes(startIndex, count, match).FirstOrDefault(-1);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the first occurrence within the
        /// range of elements in the <see cref="IEnumerable{TSource}"/> that extends from
        /// the specified index to the last element.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="match">A function that defines the conditions of the element to search for.</param>
        /// <returns>
        /// The zero-based index of the first occurrence of an element that matches the conditions
        /// defined by match, if found; otherwise, -1.
        /// </returns>
        public static int FindIndex<TSource>(this IEnumerable<TSource> source, int startIndex, Func<TSource, bool> match)
        {
            return source.FindIndex(startIndex, -1, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the first occurrence within the
        /// entire <see cref="IEnumerable{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">A function that defines the conditions of the element to search for.</param>
        /// <returns>
        /// The zero-based index of the first occurrence of an element that matches the conditions
        /// defined by match, if found; otherwise, -1.
        /// </returns>
        public static int FindIndex<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> match)
        {
            return source.FindIndex(0, -1, match);
        }

        #endregion

        #region FirstOrDefault

        /// <summary>
        /// Returns the first element of the sequence that satisfies a condition or the default
        /// value if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="default">The default value.</param>
        /// <returns>
        /// The default value if source is empty or if no element passes the test specified
        /// by predicate; otherwise, the first element in source that passes the test specified
        /// by predicate.
        /// </returns>
        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, TSource @default)
        {
            if (source == null) throw Error.ArgumentNull("source");
            if (predicate == null) throw Error.ArgumentNull("predicate");
            if (@default == null) throw Error.ArgumentNull("default");

            foreach (TSource element in source)
            {
                if (predicate(element)) return element;
            }

            return @default;
        }

        /// <summary>
        /// Returns the first element of a sequence, or the default value if the sequence contains
        /// no elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to return an element from.</param>
        /// <param name="default">The default value.</param>
        /// <returns>
        /// The default value if source is empty; otherwise, the first element in source.
        /// </returns>
        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, TSource @default)
        {
            return source.FirstOrDefault(_ => true, @default);
        }

        #endregion

        #region Flatten

        /// <summary>
        /// Flattens a sequence by indicated depth.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to flatten.</param>
        /// <param name="depth">Indicates how many levels of sequences will be flattened; 
        /// if the value is -1, all the sequences will be flattened.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TSource}"/> with the elements of all sequences flattened.
        /// </returns>
        public static IEnumerable<TSource> Flatten<TSource>(this IEnumerable source, int depth = -1)
        {
            if (source == null) throw Error.ArgumentNull("source");

            foreach (object item in source)
            {
                bool isSequence = item is IEnumerable && !(item is string);

                if (isSequence && depth != 0)
                {
                    foreach (TSource item2 in ((IEnumerable)item).Flatten<TSource>(Math.Min(-1, depth - 1)))
                        yield return item2;
                }
                else
                {
                    yield return (TSource)item;
                }
            }
        }

        #endregion

        #region ForEach

        /// <summary>
        /// Performs the specified action on each element of the <see cref="IEnumerable{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to iterate.</param>
        /// <param name="action">
        /// The <see cref="Action{TSource}"/> delegate to perform on each element of the <see cref="IEnumerable{TSource}"/>.
        /// </param>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null) throw Error.ArgumentNull("source");
            if (action == null) throw Error.ArgumentNull("action");

            if (source is TSource[] array)
            {
                Array.ForEach(array, action);
            }
            else if (source is List<TSource> list)
            {
                list.ForEach(action);
            }
            else
            {
                foreach (TSource item in source) action(item);
            }
        }

        /// <summary>
        /// Performs the specified action on each element of the <see cref="IEnumerable{TSource}"/> 
        /// by incorporating the element's index.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to iterate.</param>
        /// <param name="action">
        /// The <see cref="Action{int, TSource}"/> delegate to perform on each element of the <see cref="IEnumerable{TSource}"/>; 
        /// the first parameter of the action represents the index of the source element.
        ///</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<int, TSource> action)
        {
            int cont = 0;
            source.ForEach(t => { action(cont, t); cont++; });
        }

        #endregion

        #region GetRandom

        /// <summary>
        /// Gets a random element of a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence with the elements.</param>
        /// <param name="random">The random generator.</param>
        /// <param name="index"> The position in the sequence of the random obtained element.</param>
        /// <returns>
        /// A random element of the sequence.
        /// </returns>
        public static TSource GetRandom<TSource>(this IEnumerable<TSource> source, Random random, out int index)
        {
            if (source == null) throw Error.ArgumentNull("source");
            if (random == null) throw Error.ArgumentNull("random");

            index = -1;
            TSource result = default;

            if (source is IReadOnlyList<TSource> sourceReadOnlyList && sourceReadOnlyList.Count > 0)
            {
                index = random.Next(0, sourceReadOnlyList.Count);
                result = sourceReadOnlyList[index];
            }
            else
            {
                IList<TSource> sourceList = source as IList<TSource> ?? source.ToArray();

                if (sourceList.Count > 0)
                {
                    index = random.Next(0, sourceList.Count);
                    result = sourceList[index];
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a random element of a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence with the elements.</param>
        /// <param name="index"> The position in the sequence of the random obtained element.</param>
        /// <returns>
        /// A random element of the sequence.
        /// </returns>
        public static TSource GetRandom<TSource>(this IEnumerable<TSource> source, out int index)
        {
            return GetRandom(source, new Random(), out index);
        }

        /// <summary>
        /// Gets a random element of a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence with the elements.</param>
        /// <param name="random">The random generator.</param>
        /// <returns>
        /// A random element of the sequence.
        /// </returns>
        public static TSource GetRandom<TSource>(this IEnumerable<TSource> source, Random random)
        {
            return GetRandom(source, random, out int _);
        }

        /// <summary>
        /// Gets a random element of a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence with the elements.</param>
        /// <returns>
        /// A random element of the sequence.
        /// </returns>
        public static TSource GetRandom<TSource>(this IEnumerable<TSource> source)
        {
            return GetRandom(source, out int _);
        }

        #endregion

        #region IsNullOrEmpty

        /// <summary>
        /// Indicates whether the specified collection is null or does not contain elements.
        /// </summary>
        /// <param name="source">The collection to test.</param>
        /// <returns>
        /// <see langword="true"/> if the value parameter is null or an empty collection; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsNullOrEmpty(this IEnumerable source)
        {
            return source == null || source switch
            {
                string text => text.Length == 0,
                ICollection collection => collection.Count == 0,
                _ => !source.GetEnumerator().MoveNext(),
            };
        }

        #endregion

        #region Repeat

        /// <summary>
        /// Repeats the sequence the specified number of times.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in sequence.</typeparam>
        /// <param name="source">The sequence to repeat.</param>
        /// <param name="count">Number of times to repeat the sequence.</param>
        /// <returns>A sequence produced from the repetition of the original source sequence.</returns>
        public static IEnumerable<TSource> Repeat<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null) throw Error.ArgumentNull("source");

            for (int i = 0; i < count; i++)
            {
                foreach (var item in source) yield return item;
            }
        }

        /// <summary>
        /// Repeats the sequence forever.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in sequence.</typeparam>
        /// <param name="source">The sequence to repeat.</param>
        /// <returns>A sequence produced from the infinite repetition of the original source sequence.</returns>
        public static IEnumerable<TSource> Repeat<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw Error.ArgumentNull("source");

            while (true)
            {
                foreach (var item in source) yield return item;
            }
        }

        #endregion

        #region SequenceEquals

        /// <summary>
        /// Determines whether the first sequence and the second sequence contain the same elements.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements of the first sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements of the second sequence.</typeparam>
        /// <param name="first">The sequence to compare to the second sequence.</param>
        /// <param name="second">The sequence to compare to the first sequence.</param>
        /// <param name="equals">A function to know if two elements are equal.</param>
        /// <returns>
        /// <see langword="true"/> if both sequences contain the same elements; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool SequenceEquals<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, bool> equals)
        {
            if (first == null) throw Error.ArgumentNull("first");
            if (second == null) throw Error.ArgumentNull("second");
            if (equals == null) throw Error.ArgumentNull("equals");

            bool areEqual = true;

            using IEnumerator<TFirst> e1 = first.GetEnumerator();
            using IEnumerator<TSecond> e2 = second.GetEnumerator();
            bool moveNext1 = true;
            bool moveNext2 = true;

            do
            {
                moveNext1 = e1.MoveNext();
                moveNext2 = e2.MoveNext();

                if (moveNext1 != moveNext2 || (moveNext1 && !equals(e1.Current, e2.Current)))
                {
                    areEqual = false;
                }
            } while (moveNext1 && moveNext2 && areEqual);

            return areEqual;
        }

        #endregion

        #region SubSequence

        /// <summary>
        /// Returns a subsequence that contains all elements within a range.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="startIndex">The zero-based starting index of the range.</param>
        /// <param name="count">The number of elements to take.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TSource}"/> with the elements that are present in the range.
        /// </returns>
        public static IEnumerable<TSource> SubSequence<TSource>(this IEnumerable<TSource> source, int startIndex, int count)
        {
            if (source == null) throw Error.ArgumentNull("source");

            IEnumerable<TSource> result = source.Skip(startIndex);
            if (count >= 0) result = result.Take(count);

            return result;
        }

        #endregion

        #region Shuffle

        /// <summary>
        /// Shuffles a sequence randomly.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to shuffle.</param>
        ///<returns>An <see cref="IEnumerable{TSource}"/> with the elements in a random order.</returns>
        public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source)
        {
            return source.Shuffle(new Random());
        }

        /// <summary>
        /// Shuffles a sequence randomly.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to shuffle.</param>
        /// <param name="random">The random generator.</param>
        ///<returns>An <see cref="IEnumerable{TSource}"/> with the elements in a random order.</returns>
        public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source, Random random)
        {
            if (source == null) throw Error.ArgumentNull("source");
            if (random == null) throw Error.ArgumentNull("random");

            List<TSource> sourceList = source.ToList();

            while (sourceList.Count > 0)
            {
                int index = random.Next(0, sourceList.Count);
                TSource item = sourceList[index];
                sourceList.RemoveAt(index);

                yield return item;
            }
        }

        #endregion

        #region SymmetricExcept

        /// <summary>
        /// Gets a sequence that contains the elements that are present either in 
        /// that sequence or in the other specified sequence, but not both.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequences.</typeparam>
        /// <param name="first">The sequence to compare to the second sequence.</param>
        /// <param name="second">The sequence to compare to the first sequence.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{TSource}"/> to compare values.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TSource}"/> with the elements that are present in 
        /// each sequence, but not both.
        /// </returns>
        public static IEnumerable<TSource> SymmetricExcept<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null) throw Error.ArgumentNull("first");
            if (second == null) throw Error.ArgumentNull("second");
            if (comparer == null) throw Error.ArgumentNull("comparer");

            List<TSource> elementsInBoth = new List<TSource>();

            foreach (TSource item in first)
            {
                if (!second.Contains(item, comparer))
                {
                    yield return item;
                }
                else
                {
                    elementsInBoth.Add(item);
                }
            }

            foreach (TSource item in second)
            {
                if (!elementsInBoth.Contains(item, comparer))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Gets a sequence that contains the elements that are present either in 
        /// that sequence or in the other specified sequence, but not both.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequences.</typeparam>
        /// <param name="first">The sequence to compare to the second sequence.</param>
        /// <param name="second">The sequence to compare to the first sequence.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TSource}"/> with the elements that are present in 
        /// each sequence, but not both.
        /// </returns>
        public static IEnumerable<TSource> SymmetricExcept<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return first.SymmetricExcept(second, EqualityComparer<TSource>.Default);
        }

        #endregion

        #region Swap

        /// <summary>
        /// Swaps the positions of two elements in a list.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The list with the elements to swap.</param>
        /// <param name="index1">The position of the first element to swap.</param>
        /// <param name="index2">The position of the second element to swap.</param>
        public static void Swap<TSource>(this IList<TSource> source, int index1, int index2)
        {
            if (source == null) throw Error.ArgumentNull("source");

            if (index1 != index2)
            {
                (source[index1], source[index2]) = (source[index2], source[index1]);
            }
        }

        /// <summary>
        /// Swaps the positions of two elements in a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence with the elements to swap.</param>
        /// <param name="index1">The position of the first element to swap.</param>
        /// <param name="index2">The position of the second element to swap.</param>
        /// <returns>An <see cref="IEnumerable{TSource}"/> with the elements swapped </returns>
        public static IEnumerable<TSource> Swap<TSource>(this IEnumerable<TSource> source, int index1, int index2)
        {
            if (source == null) throw Error.ArgumentNull("source");

            int firstIndex = Math.Min(index1, index2);
            int secondIndex = Math.Max(index1, index2);

            using IEnumerator<TSource> e = source.GetEnumerator();

            for (int i = 0; i < firstIndex; i++)
            {
                if (!e.MoveNext()) yield break;

                yield return e.Current;
            }

            if (firstIndex != secondIndex)
            {
                if (!e.MoveNext()) yield break;

                TSource rememberedItem = e.Current;
                List<TSource> subSequence = new List<TSource>(secondIndex - firstIndex - 1);

                for (int i = 0; i < subSequence.Capacity && e.MoveNext(); i++)
                {
                    subSequence.Add(e.Current);
                }

                if (e.MoveNext()) yield return e.Current;

                foreach (TSource item in subSequence) yield return item;

                yield return rememberedItem;
            }

            while (e.MoveNext()) yield return e.Current;
        }

        #endregion
    }
}
