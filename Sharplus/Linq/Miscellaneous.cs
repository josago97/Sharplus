using System.Collections.Generic;

namespace System.Linq
{
    public static class Miscellaneous
    {
        public static string Dump<T>(this IEnumerable<T> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return $"[{string.Join(", ", source)}]";
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw Error.ArgumentNull("source");

            if (source is T[])
            {
                Array.ForEach((T[])source, action);
            }
            else if (source is List<T>)
            {
                ((List<T>)source).ForEach(action);
            }
            else
            {
                foreach (T item in source) action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<int, T> action)
        {
            int cont = 0;
            source.ForEach(t => { action(cont, t); cont++; });
        }

        public static T GetRandom<T>(this IEnumerable<T> source, Random random, out int index)
        {
            if (source == null) throw Error.ArgumentNull("source");
            T[] sourceArray = source.ToArray();
            if (sourceArray.Length == 0) throw Error.NoElements();
            index = random.Next(0, sourceArray.Length);
            return sourceArray[index];
        }

        public static T GetRandom<T>(this IEnumerable<T> source, out int index)
        {
            return GetRandom(source, new Random(), out index);
        }

        public static T GetRandom<T>(this IEnumerable<T> source)
        {
            return GetRandom(source, out int index);
        }

        public static IEnumerable<T> Suffle<T>(this IEnumerable<T> source)
        {
            return source.Suffle(new Random());
        }

        public static IEnumerable<T> Suffle<T>(this IEnumerable<T> source, Random random)
        {
            return source.OrderBy(_ => random.Next());
        }

        public static IEnumerable<T> SymmetricExcept<T>(this IEnumerable<T> source1, IEnumerable<T> source2)
        {
            if (source1 == null) throw Error.ArgumentNull("source1");
            if (source2 == null) throw Error.ArgumentNull("source2");

            HashSet<T> hashSet = new HashSet<T>(source1);
            hashSet.SymmetricExceptWith(source2);
            return hashSet;
        }
    }
}
