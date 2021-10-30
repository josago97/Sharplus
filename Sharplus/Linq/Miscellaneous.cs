using System.Collections;
using System.Collections.Generic;
using Sharplus.Linq;

namespace System.Linq
{
    public static class Miscellaneous
    {
        public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> source, params IEnumerable<TSource>[] collections)
        {
            if (source == null) throw Error.ArgumentNull("source");
            if (collections.Any(c => c == null)) throw Error.ArgumentNull("A collection is null");

            foreach (var item in source) yield return item;
            foreach (var collection in collections) foreach (var item in collection) yield return item;
        }

        public static string Dump<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return $"[{string.Join(", ", source)}]";
        }

        public static IEnumerable<TSource> Flatten<TSource>(this IEnumerable source, int depth)
        {
            if (source == null) throw Error.ArgumentNull("source");
            
            foreach (object item in source)
            {
                bool isCollection;

                switch (item)
                {
                    case string _:
                    default:
                        isCollection = false;
                        break;

                    case IEnumerable _:
                        isCollection = true;
                        break;
                }

                if (isCollection && depth != 0)
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

        public static IEnumerable<TSource> Flatten<TSource>(this IEnumerable source)
        {
            return source.Flatten<TSource>(-1);
        }

        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null) throw Error.ArgumentNull("source");

            if (source is TSource[])
            {
                Array.ForEach((TSource[])source, action);
            }
            else if (source is List<TSource>)
            {
                ((List<TSource>)source).ForEach(action);
            }
            else
            {
                foreach (TSource item in source) action(item);
            }
        }

        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<int, TSource> action)
        {
            int cont = 0;
            source.ForEach(t => { action(cont, t); cont++; });
        }

        public static IEnumerable<TSource> GetAllRandom<TSource>(this IEnumerable<TSource> source, Random random)
        {
            if (source == null) throw Error.ArgumentNull("source");
            List<TSource> sourceList = source.ToList();

            while (sourceList.Count > 0)
            {
                int index = random.Next(0, sourceList.Count);
                TSource item = sourceList[index];
                sourceList.RemoveAt(index);

                yield return item;
            }
        }

        public static IEnumerable<TSource> GetAllRandom<TSource>(this IEnumerable<TSource> source)
        {
            return source.GetAllRandom(new Random());
        }

        public static TSource GetRandom<TSource>(this IEnumerable<TSource> source, Random random, out int index)
        {
            if (source == null) throw Error.ArgumentNull("source");

            index = -1;
            TSource result = default;
            TSource[] sourceArray = source.ToArray();

            if (sourceArray.Length > 0)
            {
                index = random.Next(0, sourceArray.Length);
                result = sourceArray[index];
            }

            return result;
        }

        public static TSource GetRandom<TSource>(this IEnumerable<TSource> source, out int index)
        {
            return GetRandom(source, new Random(), out index);
        }

        public static TSource GetRandom<TSource>(this IEnumerable<TSource> source)
        {
            return GetRandom(source, out int index);
        }

        public static TSource Max<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            return source.MaxAll(selector, comparer).First();
        }

        public static IEnumerable<TSource> MaxAll<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer = default)
        {
            if (source == null) throw Error.ArgumentNull("source");
            if (selector == null) throw Error.ArgumentNull("selector");

            if (comparer == null) comparer = Comparer<TKey>.Default;
            var elements = new List<TSource>();

            using (var iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                {
                    throw Error.NoElements();
                }

                TSource item = iterator.Current;
                TKey key = selector(item);
                TKey maxKey = key;
                elements.Add(item);

                while (iterator.MoveNext())
                {
                    item = iterator.Current;
                    key = selector(item);
                    int comparation = comparer.Compare(key, maxKey);

                    if (comparation > 0)
                    {
                        elements.Clear();
                        elements.Add(item);
                        maxKey = key;
                    }
                    else if (comparation == 0)
                    {
                        elements.Add(item);
                    }
                }
            }

            return elements;
        }

        public static TSource Min<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            return source.MinAll(selector, comparer).First();
        }

        public static IEnumerable<TSource> MinAll<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer = default)
        {
            if (source == null) throw Error.ArgumentNull("source");
            if (selector == null) throw Error.ArgumentNull("selector");

            if (comparer == null) comparer = Comparer<TKey>.Default;
            var elements = new List<TSource>();

            using (var iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                {
                    throw Error.NoElements();
                }

                TSource item = iterator.Current;
                TKey key = selector(item);
                TKey minKey = key;
                elements.Add(item);

                while (iterator.MoveNext())
                {
                    item = iterator.Current;
                    key = selector(item);
                    int comparation = comparer.Compare(key, minKey);

                    if (comparation < 0)
                    {
                        elements.Clear();
                        elements.Add(item);
                        minKey = key;
                    }
                    else if (comparation == 0)
                    {
                        elements.Add(item);
                    }
                }
            }

            return elements;
        }

        public static bool SequenceEqual<TFirst, TSecond>(this IEnumerable<TFirst> source1, IEnumerable<TSecond> source2, Func<TFirst, TSecond, bool> equals)
        {
            if (source1 == null) throw Error.ArgumentNull("source1");
            if (source2 == null) throw Error.ArgumentNull("source2");
            if (equals == null) throw Error.ArgumentNull("equals");

            TFirst[] array1 = source1.ToArray();
            TSecond[] array2 = source2.ToArray();
            bool result = array1.Length == array2.Length;

            if (result)
            {
                for (int i = 0; i < array1.Length; i++)
                {
                    if (!equals(array1[i], array2[i]))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        public static IEnumerable<TSource> SubCollection<TSource>(this IEnumerable<TSource> source, int startIndex, int length)
        {
            if (source == null) throw Error.ArgumentNull("source");

            return source.Skip(startIndex).Take(length);
        }

        public static IEnumerable<TSource> Suffle<TSource>(this IEnumerable<TSource> source)
        {
            return source.Suffle(new Random());
        }

        public static IEnumerable<TSource> Suffle<TSource>(this IEnumerable<TSource> source, Random random)
        {
            return source.OrderBy(_ => random.Next());
        }

        public static IEnumerable<TSource> SymmetricExcept<TSource>(this IEnumerable<TSource> source1, IEnumerable<TSource> source2)
        {
            if (source1 == null) throw Error.ArgumentNull("source1");
            if (source2 == null) throw Error.ArgumentNull("source2");

            HashSet<TSource> hashSet = new HashSet<TSource>(source1);
            hashSet.SymmetricExceptWith(source2);
            return hashSet;
        }

        public static void Swap<TSource>(this IList<TSource> source, int index1, int index2)
        {
            if (source == null) throw Error.ArgumentNull("source");

            if (index1 != index2)
            {
                TSource temp = source[index2];
                source[index2] = source[index1];
                source[index1] = temp;
            }
        }

        public static IEnumerable<TSource> Swap<TSource>(this IEnumerable<TSource> source, int index1, int index2)
        {
            if (source == null) throw Error.ArgumentNull("source");

            int firstIndex = Math.Min(index1, index2);
            int secondIndex = Math.Max(index1, index2);

            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                for (int i = 0; i < firstIndex; i++)
                {
                    if (!e.MoveNext()) yield break;

                    yield return e.Current;
                }

                if (firstIndex != secondIndex)
                {
                    if (!e.MoveNext()) yield break;

                    TSource rememberedItem = e.Current;
                    List<TSource> subCollection = new List<TSource>(secondIndex - firstIndex - 1);

                    for (int i = 0; i < subCollection.Capacity && e.MoveNext(); i++)
                    {
                        subCollection.Add(e.Current);
                    }

                    if (e.MoveNext()) yield return e.Current;

                    foreach (TSource item in subCollection) yield return item;

                    yield return rememberedItem;
                }

                while (e.MoveNext()) yield return e.Current;
            }

        }
    }
}
