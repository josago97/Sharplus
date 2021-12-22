using Sharplus.System.Linq;
using System.Collections.Generic;

namespace System.Linq
{
    public static class Statistics
    {
        #region AllMax

        /// <summary>
        /// Returns all maximum values in a generic sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum values.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{TSource}"/> to compare values.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TSource}"/> with the maximum values in the sequence.
        /// </returns>
        public static IEnumerable<TSource> AllMax<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw Error.ArgumentNull("source");
            if (selector == null) throw Error.ArgumentNull("selector");
            if (comparer == null) throw Error.ArgumentNull("comparer");

            List<TSource> elements = new List<TSource>();

            using (IEnumerator<TSource> iterator = source.GetEnumerator())
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

        /// <summary>
        /// Returns all maximum values in a generic sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum values.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TSource}"/> with the maximum values in the sequence.
        /// </returns>
        public static IEnumerable<TSource> AllMax<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.AllMax(selector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Returns all maximum values in a generic sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum values.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{TSource}"/> to compare values.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TSource}"/> with the maximum values in the sequence.
        /// </returns>
        public static IEnumerable<TSource> AllMax<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            return source.AllMax(_ => _, comparer);
        }

        /// <summary>
        /// Returns all maximum values in a generic sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum values.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TSource}"/> with the maximum values in the sequence.
        /// </returns>
        public static IEnumerable<TSource> AllMax<TSource>(this IEnumerable<TSource> source)
        {
            return source.AllMax(Comparer<TSource>.Default);
        }

        #endregion

        #region AllMin

        /// <summary>
        /// Returns all minimum values in a generic sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum values.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{TSource}"/> to compare values.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TSource}"/> with the minimum values in the sequence.
        /// </returns>
        public static IEnumerable<TSource> AllMin<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw Error.ArgumentNull("source");
            if (selector == null) throw Error.ArgumentNull("selector");
            if (comparer == null) throw Error.ArgumentNull("comparer");

            List<TSource> elements = new List<TSource>();

            using (IEnumerator<TSource> iterator = source.GetEnumerator())
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

        /// <summary>
        /// Returns all minimum values in a generic sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum values.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TSource}"/> with the minimum values in the sequence.
        /// </returns>
        public static IEnumerable<TSource> AllMin<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.AllMin(selector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Returns all minimum values in a generic sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum values.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{TSource}"/> to compare values.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TSource}"/> with the minimum values in the sequence.
        /// </returns>
        public static IEnumerable<TSource> AllMin<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            return source.AllMin(_ => _, Comparer<TSource>.Default);
        }

        /// <summary>
        /// Returns all minimum values in a generic sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum values.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TSource}"/> with the minimum values in the sequence.
        /// </returns>
        public static IEnumerable<TSource> AllMin<TSource>(this IEnumerable<TSource> source)
        {
            return source.AllMin(Comparer<TSource>.Default);
        }

        #endregion

        #region Median

        /// <summary>
        /// Computes the median of a sequence of <see cref="int"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="int"/> values to calculate the median.</param>
        /// <returns>
        /// The median of the sequence of values.
        /// </returns>
        public static double Median(this IEnumerable<int> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateMedian(source.Select(Convert.ToDouble).ToArray());
        }

        /// <summary>
        /// Computes the median of a sequence of <see cref="long"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="long"/> values to calculate the median.</param>
        /// <returns>
        /// The median of the sequence of values.
        /// </returns>
        public static double Median(this IEnumerable<long> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateMedian(source.Select(Convert.ToDouble).ToArray());
        }

        /// <summary>
        /// Computes the median of a sequence of <see cref="float"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="float"/> values to calculate the median.</param>
        /// <returns>
        /// The median of the sequence of values.
        /// </returns>
        public static double Median(this IEnumerable<float> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateMedian(source.Select(Convert.ToDouble).ToArray());
        }

        /// <summary>
        /// Computes the median of a sequence of <see cref="double"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="double"/> values to calculate the median.</param>
        /// <returns>
        /// The median of the sequence of values.
        /// </returns>
        public static double Median(this IEnumerable<double> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateMedian(source.ToArray());
        }

        private static double CalculateMedian(double[] values)
        {
            if (values.Length == 0) throw Error.NoElements();

            if (values.Length % 2 == 0)
            {
                double left = values[values.Length / 2 - 1];
                double right = values[values.Length / 2];
                return (left + right) / 2;
            }
            else
            {
                return values[values.Length / 2];
            }
        }

        #endregion

        #region Kurtosis

        /// <summary>
        /// Computes the kurtosis of a sequence of <see cref="int"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="int"/> values to calculate the kurtosis.</param>
        /// <param name="isNormal">Indicates if the distribution is normal.</param>
        /// <returns>
        /// The kurtosis of the sequence of values.
        /// </returns>
        public static double Kurtosis(this IEnumerable<int> source, bool isNormal = true)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateKurtosis(source.Select(Convert.ToDouble).ToArray(), isNormal);
        }

        /// <summary>
        /// Computes the kurtosis of a sequence of <see cref="long"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="long"/> values to calculate the kurtosis.</param>
        /// <param name="isNormal">Indicates if the distribution is normal.</param>
        /// <returns>
        /// The kurtosis of the sequence of values.
        /// </returns>
        public static double Kurtosis(this IEnumerable<long> source, bool isNormal = true)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateKurtosis(source.Select(Convert.ToDouble).ToArray(), isNormal);
        }

        /// <summary>
        /// Computes the kurtosis of a sequence of <see cref="float"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="float"/> values to calculate the kurtosis.</param>
        /// <param name="isNormal">Indicates if the distribution is normal.</param>
        /// <returns>
        /// The kurtosis of the sequence of values.
        /// </returns>
        public static double Kurtosis(this IEnumerable<float> source, bool isNormal = true)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateKurtosis(source.Select(Convert.ToDouble).ToArray(), isNormal);
        }

        /// <summary>
        /// Computes the kurtosis of a sequence of <see cref="double"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="double"/> values to calculate the kurtosis.</param>
        /// <param name="isNormal">Indicates if the distribution is normal.</param>
        /// <returns>
        /// The kurtosis of the sequence of values.
        /// </returns>
        public static double Kurtosis(this IEnumerable<double> source, bool isNormal = true)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateKurtosis(source.ToArray(), isNormal);
        }

        private static double CalculateKurtosis(double[] values, bool isNormal)
        {
            double average = values.Average();
            double variance = CalculateVariance(values, false);
            double fourthMoment = values.Average(x => Math.Pow(x - average, 4));

            double result = fourthMoment / Math.Pow(variance, 2); // Math.Pow(variance, 2) is equals Math.Pow(standardDeviation, 4)
            if (isNormal) result -= 3;

            return result;
        }

        #endregion

        #region Mode

        /// <summary>
        /// Computes the mode of a sequence.
        /// </summary>
        /// <param name="source">A sequence to calculate the mode.</param>
        /// <returns>
        /// The most repeated element in the sequence.
        /// </returns>
        public static IEnumerable<T> Mode<T>(this IEnumerable<T> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateMode(source, out int count);
        }

        /// <summary>
        /// Computes the mode of a sequence.
        /// </summary>
        /// <param name="source">A sequence to calculate the mode.</param>
        /// <param name="count">The times that the mode appears in the sequence.</param>
        /// <returns>
        /// The most repeated element in the sequence.
        /// </returns>
        public static IEnumerable<T> Mode<T>(this IEnumerable<T> source, out int count)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateMode(source, out count);
        }

        private static IEnumerable<T> CalculateMode<T>(IEnumerable<T> values, out int count)
        {
            var bag = new Dictionary<T, int>();

            int maxCount = 0;

            foreach (T value in values)
            {
                int valueCount;

                if (!bag.TryGetValue(value, out valueCount))
                {
                    valueCount = 1;
                }
                else
                {
                    valueCount++;
                }

                bag[value] = valueCount;

                if (valueCount > maxCount)
                {
                    maxCount = valueCount;
                }
            }

            count = maxCount;
            return bag.Where(x => x.Value == maxCount).Select(x => x.Key);
        }

        #endregion

        #region Skewness

        /// <summary>
        /// Computes the skewness of a sequence of <see cref="int"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="int"/> values to calculate the skewness.</param>
        /// <returns>
        /// The skewness of the sequence of values.
        /// </returns>
        public static double Skewness(this IEnumerable<int> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateSkewness(source.Select(Convert.ToDouble).ToArray());
        }

        /// <summary>
        /// Computes the skewness of a sequence of <see cref="long"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="long"/> values to calculate the skewness.</param>
        /// <returns>
        /// The skewness of the sequence of values.
        /// </returns>
        public static double Skewness(this IEnumerable<long> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateSkewness(source.Select(Convert.ToDouble).ToArray());
        }

        /// <summary>
        /// Computes the skewness of a sequence of <see cref="float"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="float"/> values to calculate the skewness.</param>
        /// <returns>
        /// The skewness of the sequence of values.
        /// </returns>
        public static double Skewness(this IEnumerable<float> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateSkewness(source.Select(Convert.ToDouble).ToArray());
        }

        /// <summary>
        /// Computes the skewness of a sequence of <see cref="double"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="double"/> values to calculate the skewness.</param>
        /// <returns>
        /// The skewness of the sequence of values.
        /// </returns>
        public static double Skewness(this IEnumerable<double> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateSkewness(source.ToArray());
        }

        private static double CalculateSkewness(double[] values)
        {
            double average = values.Average();
            double variance = CalculateVariance(values, false);
            double thirdMoment = values.Average(x => Math.Pow(x - average, 3));

            double result = thirdMoment / Math.Pow(variance, 1.5); // Math.Pow(variance, 1.5) is equals Math.Pow(standardDeviation, 3)

            return result;
        }

        #endregion

        #region Standard deviation

        /// <summary>
        /// Computes the standard deviation of a sequence of <see cref="int"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="int"/> values to calculate the standard deviation.</param>
        /// <param name="isSample">Indicates if the data are a sample.</param>
        /// <returns>
        /// The standard deviation of the sequence of values.
        /// </returns>
        public static double StandardDeviation(this IEnumerable<int> source, bool isSample = false)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateStandardDeviation(source.Select(Convert.ToDouble).ToArray(), isSample);
        }

        /// <summary>
        /// Computes the standard deviation of a sequence of <see cref="long"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="long"/> values to calculate the standard deviation.</param>
        /// <param name="isSample">Indicates if the data are a sample.</param>
        /// <returns>
        /// The standard deviation of the sequence of values.
        /// </returns>
        public static double StandardDeviation(this IEnumerable<long> source, bool isSample = false)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateStandardDeviation(source.Select(Convert.ToDouble).ToArray(), isSample);
        }

        /// <summary>
        /// Computes the standard deviation of a sequence of <see cref="float"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="float"/> values to calculate the standard deviation.</param>
        /// <param name="isSample">Indicates if the data are a sample.</param>
        /// <returns>
        /// The standard deviation of the sequence of values.
        /// </returns>
        public static double StandardDeviation(this IEnumerable<float> source, bool isSample = false)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateStandardDeviation(source.Select(Convert.ToDouble).ToArray(), isSample);
        }

        /// <summary>
        /// Computes the standard deviation of a sequence of <see cref="double"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="double"/> values to calculate the standard deviation.</param>
        /// <param name="isSample">Indicates if the data are a sample.</param>
        /// <returns>
        /// The standard deviation of the sequence of values.
        /// </returns>
        public static double StandardDeviation(this IEnumerable<double> source, bool isSample = false)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateStandardDeviation(source.ToArray(), isSample);
        }

        private static double CalculateStandardDeviation(double[] values, bool isSample)
        {
            double variance = CalculateVariance(values, isSample);
            return Math.Sqrt(variance);
        }

        #endregion

        #region Variance

        /// <summary>
        /// Computes the variance of a sequence of <see cref="int"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="int"/> values to calculate the variance.</param>
        /// <param name="isSample">Indicates if the data are a sample.</param>
        /// <returns>
        /// The variance of the sequence of values.
        /// </returns>
        public static double Variance(this IEnumerable<int> source, bool isSample = false)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateVariance(source.Select(Convert.ToDouble).ToArray(), isSample);
        }

        /// <summary>
        /// Computes the variance of a sequence of <see cref="long"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="long"/> values to calculate the variance.</param>
        /// <param name="isSample">Indicates if the data are a sample.</param>
        /// <returns>
        /// The variance of the sequence of values.
        /// </returns>
        public static double Variance(this IEnumerable<long> source, bool isSample = false)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateVariance(source.Select(Convert.ToDouble).ToArray(), isSample);
        }

        /// <summary>
        /// Computes the variance of a sequence of <see cref="float"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="float"/> values to calculate the variance.</param>
        /// <param name="isSample">Indicates if the data are a sample.</param>
        /// <returns>
        /// The variance of the sequence of values.
        /// </returns>
        public static double Variance(this IEnumerable<float> source, bool isSample = false)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateVariance(source.Select(Convert.ToDouble).ToArray(), isSample);
        }

        /// <summary>
        /// Computes the variance of a sequence of <see cref="double"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="double"/> values to calculate the variance.</param>
        /// <param name="isSample">Indicates if the data are a sample.</param>
        /// <returns>
        /// The variance of the sequence of values.
        /// </returns>
        public static double Variance(this IEnumerable<double> source, bool isSample = false)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateVariance(source.ToArray(), isSample);
        }

        private static double CalculateVariance(double[] values, bool isSample)
        {
            if (values.Length == 0) throw Error.NoElements();

            double average = values.Average();
            double sum = values.Sum(x => Math.Pow(x - average, 2));

            if (isSample)
            {
                if (values.Length > 1) return sum / (values.Length - 1);
                throw Error.NotEnoughElements(2);
            }
            else
            {
                return sum / values.Length;
            }
        }

        #endregion
    }
}
