using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class MathPlus
    {
        public static double Clamp(double value, double min, double max)
        {
            if (max < min) throw new ArgumentException("Max must be greater than Min");

            double result;

            if (value < min) result = min;
            else if (value > max) result = max;
            else result = value;

            return result;
        }

        public static decimal Clamp(decimal value, decimal min, decimal max)
        {
            if (max < min) throw new ArgumentException("Max must be greater than Min");

            decimal result;

            if (value < min) result = min;
            else if (value > max) result = max;
            else result = value;

            return result;
        }

        public static long Factorial(long value)
        {
            long result;

            if (value < 0) throw new ArgumentException("Value cannot be negative");

            if (value == 0 || value == 1)
                result =  1;
            else
                result = value * Factorial(value - 1);

            return result;
        }

        public static IEnumerable<long> Fibonacci(int value)
        {
            if (value < 0) throw new ArgumentException("Value cannot be negative");

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

        public static bool IsInRange(double value, double expected, double errorRange = 0)
        {
            errorRange = Math.Abs(errorRange);
            double error = AbsoluteError(expected, value);

            return -errorRange <= error && error <= errorRange; 
        }

        public static bool IsInRange(decimal value, decimal expected, decimal errorRange = 0)
        {
            errorRange = Math.Abs(errorRange);
            decimal error = AbsoluteError(expected, value);

            return -errorRange <= error && error <= errorRange;
        }

        public static double InverseLerp(double a, double b, double value)
        {
            return a != b ? Clamp((value - a) / (b - a), 0, 1) : 0f;
        }

        public static decimal InverseLerp(decimal a, decimal b, decimal value)
        {
            return a != b ? Clamp((value - a) / (b - a), 0, 1) : 0m;
        }

        public static double Lerp(double a, double b, double t)
        {
            return LerpUncampled(a, b, Clamp(t, 0, 1));
        }

        public static decimal Lerp(decimal a, decimal b, decimal t)
        {
            return LerpUncampled(a, b, Clamp(t, 0, 1));
        }

        public static double LerpUncampled(double a, double b, double t)
        {
            return a + (b - a) * t;
        }

        public static decimal LerpUncampled(decimal a, decimal b, decimal t)
        {
            return a + (b - a) * t;
        }

        public static int Max(params int[] values) => values.Max();
        public static long Max(params long[] values) => values.Max();
        public static float Max(params float[] values) => values.Max();
        public static double Max(params double[] values) => values.Max();
        public static decimal Max(params decimal[] values) => values.Max();

        public static int Min(params int[] values) => values.Min();
        public static long Min(params long[] values) => values.Min();
        public static float Min(params float[] values) => values.Min();
        public static double Min(params double[] values) => values.Min();
        public static decimal Min(params decimal[] values) => values.Min();

        public static int Mod(int dividend, int divider)
        {
            if (divider <= 0) throw new ArgumentException("Divider can not be less or equal zero");

            var remainder = dividend % divider;
            if (remainder < 0) remainder += divider;

            return remainder;
        }

        public static long Mod(long dividend, long divider)
        {
            if (divider <= 0) throw new ArgumentException("Divider can not be less or equal zero");

            var remainder = dividend % divider;
            if (remainder < 0) remainder += divider;

            return remainder;
        }

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

        public static double AbsoluteError(double realValue, double inferredValue)
        {
            return inferredValue - realValue;
        }

        public static decimal AbsoluteError(decimal realValue, decimal inferredValue)
        {
            return inferredValue - realValue;
        }

        public static double RelativeError(double realValue, double inferredValue)
        {
            return AbsoluteError(realValue, inferredValue) / realValue;
        }

        public static decimal RelativeError(decimal realValue, decimal inferredValue)
        {
            return AbsoluteError(realValue, inferredValue) / realValue;
        }
    }
}
