using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class MathPlus
    {
        #region Factorial

        /// <summary>
        /// Returns the factorial of a number.
        /// </summary>
        /// <param name="value">A number to calculate its factorial.</param>
        /// <returns>
        /// The factorial of <paramref name="value" />.
        /// </returns>
        public static long Factorial(long value)
        {
            long result;

            if (value < 0) throw new ArgumentException("value cannot be negative");

            if (value == 0 || value == 1)
                result =  1;
            else
                result = value * Factorial(value - 1);

            return result;
        }

        #endregion

        #region Fibonacci
        /// <summary>
        /// Returns the fibonacci sequence of a number.
        /// </summary>
        /// <param name="value">A number to calculate its fibonacci sequence.</param>
        /// <returns>
        /// An <see cref="IEnumerable{long}"/> that contains the fibonacci sequence of <paramref name="value" />.
        /// </returns>
        public static IEnumerable<long> Fibonacci(int value)
        {
            if (value < 0) throw new ArgumentException("value cannot be negative");

            int f0 = 0;
            int f1 = 1;
            int fn1 = f1;
            int fn2 = f0;

            yield return f0;

            if (value > 0) yield return f1;

            for (int i = 1; i < value; i++)
            {
                int fn = fn1 + fn2;
                fn2 = fn1;
                fn1 = fn;

                yield return fn;
            }
        }

        #endregion

        #region InverseLerp

        /// <summary>
        /// Calculates the linear parameter that produces the interpolant value within a range.
        /// </summary>
        /// <param name="a">Start value of the range.</param>
        /// <param name="b">End value of the range.</param>
        /// <param name="value">Value between <paramref name="a" /> and <paramref name="b" />.</param>
        /// <returns>
        /// Percentage of value between <paramref name="a" /> and <paramref name="b" />.
        /// </returns>
        public static double InverseLerp(double a, double b, double value)
        {
            return a != b ? Math.Clamp((value - a) / (b - a), 0, 1) : 0f;
        }

        /// <summary>
        /// Calculates the linear parameter that produces the interpolant value within a range.
        /// </summary>
        /// <param name="a">Start value of the range.</param>
        /// <param name="b">End value of the range.</param>
        /// <param name="value">Value between <paramref name="a" /> and <paramref name="b" />.</param>
        /// <returns>
        /// Percentage of value between <paramref name="a" /> and <paramref name="b" />.
        /// </returns>
        public static decimal InverseLerp(decimal a, decimal b, decimal value)
        {
            return a != b ? Math.Clamp((value - a) / (b - a), 0, 1) : 0m;
        }

        #endregion

        #region IsErrorInRange

        /// <summary>
        /// Determines whether the absolute error between two numbers is inside a range.
        /// </summary>        
        /// <param name="expectedValue">The expected value.</param>
        /// <param name="inferredValue">The inferred value to check.</param>
        /// <param name="errorRange">The range of the error.</param>
        /// <returns>
        /// <see langword="true"/> if the error is inside the range; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsErrorInRange(double expectedValue, double inferredValue, double errorRange = 0)
        {
            errorRange = Math.Abs(errorRange);
            double error = AbsoluteError(expectedValue, inferredValue);

            return -errorRange <= error && error <= errorRange;
        }

        /// <summary>
        /// Determines whether the absolute error between two numbers is inside a range.
        /// </summary>        
        /// <param name="expectedValue">The expected value.</param>
        /// <param name="inferredValue">The inferred value to check.</param>
        /// <param name="errorRange">The range of the error.</param>
        /// <returns>
        /// <see langword="true"/> if the error is inside the range; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsErrorInRange(decimal expectedValue, decimal inferredValue, decimal errorRange = 0)
        {
            errorRange = Math.Abs(errorRange);
            decimal error = AbsoluteError(expectedValue, inferredValue);

            return -errorRange <= error && error <= errorRange;
        }

        #endregion

        #region Lerp

        /// <summary>
        /// Calculates the linear interpolation between a range by a interpolation value.
        /// </summary>
        /// <param name="a">The start value of the range.</param>
        /// <param name="b">The end value of the range.</param>
        /// <param name="t">The interpolation value between [0, 1].</param>
        /// <returns>
        /// The interpolated value between <paramref name="a" /> and <paramref name="b" /> by <paramref name="t" />.
        /// </returns>
        public static double Lerp(double a, double b, double t)
        {
            return LerpUnclamped(a, b, Math.Clamp(t, 0, 1));
        }

        /// <summary>
        /// Calculates the linear interpolation between a range by a interpolation value.
        /// </summary>
        /// <param name="a">The start value of the range.</param>
        /// <param name="b">The end value of the range.</param>
        /// <param name="t">The interpolation value between [0, 1].</param>
        /// <returns>
        /// The interpolated value between <paramref name="a" /> and <paramref name="b" /> by <paramref name="t" />.
        /// </returns>
        public static decimal Lerp(decimal a, decimal b, decimal t)
        {
            return LerpUnclamped(a, b, Math.Clamp(t, 0, 1));
        }

        #endregion

        #region LerpUnclamped

        /// <summary>
        /// Calculates the linear interpolation between a range by a interpolation value.
        /// </summary>
        /// <param name="a">The start value of the range.</param>
        /// <param name="b">The end value of the range.</param>
        /// <param name="t">The interpolation value.</param>
        /// <returns>
        /// The interpolated value between <paramref name="a" /> and <paramref name="b" /> by <paramref name="t" />.
        /// </returns>
        public static double LerpUnclamped(double a, double b, double t)
        {
            return a + (b - a) * t;
        }

        /// <summary>
        /// Calculates the linear interpolation between a range by a interpolation value.
        /// </summary>
        /// <param name="a">The start value of the range.</param>
        /// <param name="b">The end value of the range.</param>
        /// <param name="t">The interpolation value.</param>
        /// <returns>
        /// The interpolated value between <paramref name="a" /> and <paramref name="b" /> by <paramref name="t" />.
        /// </returns>
        public static decimal LerpUnclamped(decimal a, decimal b, decimal t)
        {
            return a + (b - a) * t;
        }

        #endregion

        #region Max

        /// <summary>
        /// Returns the maximum value of a series of <see cref="int"/>.
        /// </summary>
        /// <param name="values">A series of <see cref="int"/> values to determine the maximum value.</param>
        /// <returns>
        /// The maximum value in the series.
        /// </returns>
        public static int Max(params int[] values) => values.Max();

        /// <summary>
        /// Returns the maximum value of a series of <see cref="long"/>.
        /// </summary>
        /// <param name="values">A series of <see cref="long"/> values to determine the maximum value.</param>
        /// <returns>
        /// The maximum value in the series.
        /// </returns>
        public static long Max(params long[] values) => values.Max();

        /// <summary>
        /// Returns the maximum value of a series of <see cref="float"/>.
        /// </summary>
        /// <param name="values">A series of <see cref="float"/> values to determine the maximum value.</param>
        /// <returns>
        /// The maximum value in the series.
        /// </returns>
        public static float Max(params float[] values) => values.Max();

        /// <summary>
        /// Returns the maximum value of a series of <see cref="double"/>.
        /// </summary>
        /// <param name="values">A series of <see cref="double"/> values to determine the maximum value.</param>
        /// <returns>
        /// The maximum value in the series.
        /// </returns>
        public static double Max(params double[] values) => values.Max();

        /// <summary>
        /// Returns the maximum value of a series of <see cref="decimal"/>.
        /// </summary>
        /// <param name="values">A series of <see cref="decimal"/> values to determine the maximum value.</param>
        /// <returns>
        /// The maximum value in the series.
        /// </returns>
        public static decimal Max(params decimal[] values) => values.Max();

        #endregion

        #region Min

        /// <summary>
        /// Returns the minimum value of a series of <see cref="int"/>.
        /// </summary>
        /// <param name="values">A series of <see cref="int"/> values to determine the minimum value.</param>
        /// <returns>
        /// The minimum value in the series.
        /// </returns>
        public static int Min(params int[] values) => values.Min();

        /// <summary>
        /// Returns the minimum value of a series of <see cref="long"/>.
        /// </summary>
        /// <param name="values">A series of <see cref="long"/> values to determine the minimum value.</param>
        /// <returns>
        /// The minimum value in the series.
        /// </returns>
        public static long Min(params long[] values) => values.Min();

        /// <summary>
        /// Returns the minimum value of a series of <see cref="float"/>.
        /// </summary>
        /// <param name="values">A series of <see cref="float"/> values to determine the minimum value.</param>
        /// <returns>
        /// The minimum value in the series.
        /// </returns>
        public static float Min(params float[] values) => values.Min();

        /// <summary>
        /// Returns the minimum value of a series of <see cref="double"/>.
        /// </summary>
        /// <param name="values">A series of <see cref="double"/> values to determine the minimum value.</param>
        /// <returns>
        /// The minimum value in the series.
        /// </returns>
        public static double Min(params double[] values) => values.Min();

        /// <summary>
        /// Returns the minimum value of a series of <see cref="decimal"/>.
        /// </summary>
        /// <param name="values">A series of <see cref="decimal"/> values to determine the minimum value.</param>
        /// <returns>
        /// The minimum value in the series.
        /// </returns>
        public static decimal Min(params decimal[] values) => values.Min();

        #endregion

        #region Mod

        /// <summary>
        /// Calculates the remainder or signed remainder of a division.
        /// </summary>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>
        /// The remainder or signed remainder of the division.
        /// </returns>
        public static int Mod(int dividend, int divisor)
        {
            if (divisor <= 0) throw new ArgumentException("divisor can not be less or equal zero");

            int remainder = dividend % divisor;
            if (remainder < 0) remainder += divisor;

            return remainder;
        }

        /// <summary>
        /// Calculates the remainder or signed remainder of a division.
        /// </summary>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>
        /// The remainder or signed remainder of the division.
        /// </returns>
        public static long Mod(long dividend, long divisor)
        {
            if (divisor <= 0) throw new ArgumentException("divisor can not be less or equal zero");

            var remainder = dividend % divisor;
            if (remainder < 0) remainder += divisor;

            return remainder;
        }

        #endregion

        #region GreatestCommonDivisor

        /// <summary>
        /// Calculates the greates common divisor of a series of <see cref="int"/>.
        /// </summary>
        /// <param name="values">A series of <see cref="int"/> values to calculate the greates common divisor.</param>
        /// <returns>
        /// The greates common divisor of the numbers.
        /// </returns>
        public static int GreatestCommonDivisor(params int[] values)
        {
            long gcd = 1;

            if (values.Length > 0)
            {
                gcd = values[0];

                for (int i = 1; i < values.Length; i++)
                {
                    gcd = CalculateGreatestCommonDivisor(gcd, values[i]);
                }
            }

            return (int)gcd;
        }

        /// <summary>
        /// Calculates the greates common divisor of a series of <see cref="long"/>.
        /// </summary>
        /// <param name="values">A series of <see cref="long"/> values to calculate the greates common divisor.</param>
        /// <returns>
        /// The greates common divisor of the numbers.
        /// </returns>
        public static long GreatestCommonDivisor(params long[] values)
        {
            long gcd = 1;

            if (values.Length > 0)
            {
                gcd = values[0];

                for (int i = 1; i < values.Length; i++)
                {
                    gcd = CalculateGreatestCommonDivisor(gcd, values[i]);
                }
            }

            return gcd;
        }

        private static long CalculateGreatestCommonDivisor(long a, long b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            long gcd = Math.Max(a, b);

            if (Math.Min(a, b) != 0)
            {
                gcd = a % b == 0 ? b : GreatestCommonDivisor(b, a % b);
            }

            return gcd;
        }

        #endregion

        #region LessCommonMultiple

        /// <summary>
        /// Calculates the less common multiple of a series of <see cref="int"/>.
        /// </summary>
        /// <param name="values">A series of <see cref="int"/> values to calculate the less common multiple.</param>
        /// <returns>
        /// The less common multiple of the numbers.
        /// </returns>
        public static int LessCommonMultiple(params int[] values)
        {
            long lcm = 0;

            if (values.Length > 0)
            {
                lcm = values[0];

                for (int i = 1; i < values.Length; i++)
                {
                    lcm = CalculateLessCommonMultiple(lcm, values[i]);
                }
            }

            return (int)lcm;
        }

        /// <summary>
        /// Calculates the less common multiple of a series of <see cref="long"/>.
        /// </summary>
        /// <param name="values">A series of <see cref="long"/> values to calculate the less common multiple.</param>
        /// <returns>
        /// The less common multiple of the numbers.
        /// </returns>
        public static double LessCommonMultiple(params long[] values)
        {
            long lcm = 0;

            if (values.Length > 0)
            {
                lcm = values[0];

                for (int i = 1; i < values.Length; i++)
                {
                    lcm = CalculateLessCommonMultiple(lcm, values[i]);
                }
            }

            return lcm;
        } 

        private static long CalculateLessCommonMultiple(long a, long b)
        {
            long gcd = GreatestCommonDivisor(a, b);
            long lcm = gcd != 0 ? a * b / gcd : 0;
            return lcm;
        }

        #endregion

        #region AbsoluteError

        /// <summary>
        /// Calculates the absolute error between two numbers.
        /// </summary>
        /// <param name="realValue">The real value.</param>
        /// <param name="inferredValue">The inferred value.</param>
        /// <returns>
        /// The absolute error between two values.
        /// </returns>
        public static double AbsoluteError(double realValue, double inferredValue)
        {
            return inferredValue - realValue;
        }

        /// <summary>
        /// Calculates the absolute error between two numbers.
        /// </summary>
        /// <param name="realValue">The real value.</param>
        /// <param name="inferredValue">The inferred value.</param>
        /// <returns>
        /// The absolute error between two values.
        /// </returns
        public static decimal AbsoluteError(decimal realValue, decimal inferredValue)
        {
            return inferredValue - realValue;
        }

        #endregion

        #region RelativeError

        /// <summary>
        /// Calculates the relative error between two numbers.
        /// </summary>
        /// <param name="realValue">The real value.</param>
        /// <param name="inferredValue">The inferred value.</param>
        /// <returns>
        /// The relative error between two values.
        /// </returns
        public static double RelativeError(double realValue, double inferredValue)
        {
            return AbsoluteError(realValue, inferredValue) / realValue;
        }

        /// <summary>
        /// Calculates the relative error between two numbers.
        /// </summary>
        /// <param name="realValue">The real value.</param>
        /// <param name="inferredValue">The inferred value.</param>
        /// <returns>
        /// The relative error between two values.
        /// </returns
        public static decimal RelativeError(decimal realValue, decimal inferredValue)
        {
            return AbsoluteError(realValue, inferredValue) / realValue;
        }

        #endregion
    }
}
