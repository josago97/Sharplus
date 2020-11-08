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
                double left = values[values.Length / 2];
                double right = values[values.Length / 2 + 1];
                return (left + right) / 2;
            }
            else
            {
                return values[values.Length / 2 + 1];
            }
        }

        #endregion

        #region Mode

        public static T Mode<T>(this IEnumerable<T> source)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateMode(source, out int count);
        }

        public static T Mode<T>(this IEnumerable<T> source, out int count)
        {
            if (source == null) throw Error.ArgumentNull("source");
            return CalculateMode(source, out count);
        }

        private static T CalculateMode<T>(IEnumerable<T> values, out int count)
        {
            var bag = new Dictionary<T, int>();

            T result = default;
            count = 0;

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

                if (valueCount > count)
                {
                    count = valueCount;
                    result = value;
                }
            }

            return result;
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
            double sum = values.Sum(x => checked(Math.Pow(x - average, 2)));

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
    }
}
