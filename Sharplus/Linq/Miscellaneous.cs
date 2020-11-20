using System;
using System.Collections.Generic;

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

        public static TSource GetRandom<TSource>(this IEnumerable<TSource> source, Random random, out int index)
        {
            if (source == null) throw Error.ArgumentNull("source");
            TSource[] sourceArray = source.ToArray();
            if (sourceArray.Length == 0) throw Error.NoElements();
            index = random.Next(0, sourceArray.Length);
            return sourceArray[index];
        }

        public static TSource GetRandom<TSource>(this IEnumerable<TSource> source, out int index)
        {
            return GetRandom(source, new Random(), out index);
        }

        public static TSource GetRandom<TSource>(this IEnumerable<TSource> source)
        {
            return GetRandom(source, out int index);
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer = default)
        {
            return source.MaxAllBy(selector, comparer).First();
        }

        public static IEnumerable<TSource> MaxAllBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer = default)
        {
            if (source == null) throw Error.ArgumentNull("source");
            if (selector == null) throw Error.ArgumentNull("selector");

            comparer = comparer ?? Comparer<TKey>.Default;
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

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer = default)
        {
            return source.MinAllBy(selector, comparer).First();
        }

        public static IEnumerable<TSource> MinAllBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer = default)
        {
            if (source == null) throw Error.ArgumentNull("source");
            if (selector == null) throw Error.ArgumentNull("selector");

            comparer = comparer ?? Comparer<TKey>.Default;
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
    }
}
