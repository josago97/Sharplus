using Sharplus.Linq;
using System.Collections.Generic;

namespace System.Linq
{
    public static class Statistics
    {
        #region Median

        public static double Median(this IEnumerable<int> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateMedian(source.Cast<double>().ToArray());
        }

        public static double Median(this IEnumerable<long> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateMedian(source.Cast<double>().ToArray());
        }

        public static double Median(this IEnumerable<float> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateMedian(source.Cast<double>().ToArray());
        }

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

        public static double Kurtosis(this IEnumerable<int> source, bool isNormal = true)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateKurtosis(source.Cast<double>().ToArray(), isNormal);
        }

        public static double Kurtosis(this IEnumerable<long> source, bool isNormal = true)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateKurtosis(source.Cast<double>().ToArray(), isNormal);
        }

        public static double Kurtosis(this IEnumerable<float> source, bool isNormal = true)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateKurtosis(source.Cast<double>().ToArray(), isNormal);
        }

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

        public static IEnumerable<T> Mode<T>(this IEnumerable<T> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateMode(source, out int count);
        }

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

        public static double Skewness(this IEnumerable<int> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateSkewness(source.Cast<double>().ToArray());
        }

        public static double Skewness(this IEnumerable<long> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateSkewness(source.Cast<double>().ToArray());
        }

        public static double Skewness(this IEnumerable<float> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateSkewness(source.Cast<double>().ToArray());
        }

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

        public static double StandardDeviation(this IEnumerable<int> source, bool isSample = false)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateStandardDeviation(source.Cast<double>().ToArray(), isSample);
        }

        public static double StandardDeviation(this IEnumerable<long> source, bool isSample = false)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateStandardDeviation(source.Cast<double>().ToArray(), isSample);
        }

        public static double StandardDeviation(this IEnumerable<float> source, bool isSample = false)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateStandardDeviation(source.Cast<double>().ToArray(), isSample);
        }

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

        public static double Variance(this IEnumerable<int> source, bool isSample = false)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateVariance(source.Cast<double>().ToArray(), isSample);
        }

        public static double Variance(this IEnumerable<long> source, bool isSample = false)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateVariance(source.Cast<double>().ToArray(), isSample);
        }

        public static double Variance(this IEnumerable<float> source, bool isSample = false)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateVariance(source.Cast<double>().ToArray(), isSample);
        }

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
