using System.Linq;

namespace System
{
    public static class MathPlus
    {
        public static double Clamp(double value, double min, double max)
        {
            double result;

            if (value < min) result = min;
            else if (value > max) result = max;
            else result = value;

            return result;
        }

        public static decimal Clamp(decimal value, decimal min, decimal max)
        {
            decimal result;

            if (value < min) result = min;
            else if (value > max) result = max;
            else result = value;

            return result;
        }

        public static double Lerp(double a, double b, double t)
        {
            return LerpUncampled(a, b, Clamp(0, 1, t));
        }

        public static decimal Lerp(decimal a, decimal b, decimal t)
        {
            return LerpUncampled(a, b, Clamp(0, 1, t));
        }

        public static double LerpUncampled(double a, double b, double t)
        {
            return a + (b - a) * t;
        }

        public static decimal LerpUncampled(decimal a, decimal b, decimal t)
        {
            return a + (b - a) * t;
        }

        public static double InverseLerp(double a, double b, double value)
        {
            return a != b ? Clamp((value - a) / (b - a), 0, 1) : 0f;
        }

        public static decimal InverseLerp(decimal a, decimal b, decimal value)
        {
            return a != b ? Clamp((value - a) / (b - a), 0, 1) : 0m;
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

        public static double Mod(double dividend, double divider)
        {
            return dividend - (divider * ((long)dividend / (long)divider));
        }

        public static double GreatestCommonDivisor(long a, long b)
        {
            double gcd = 0;

            a = Math.Abs(a);
            b = Math.Abs(b);

            if (Math.Min(a, b) != 0)
            {
                gcd = a % b == 0 ? b : GreatestCommonDivisor(b, a % b);
            }

            return gcd;
        }

        public static double GreatestCommonDivisor(params long[] values)
        {
            double gcd = 0;

            if (values.Length > 0)
            {
                gcd = values[0];

                for (int i = 1; i < values.Length; i++)
                {
                    gcd = GreatestCommonDivisor((long)gcd, values[i]);
                }
            }

            return gcd;
        }

        public static double LessCommonMultiple(long a, long b)
        {
            double gcd = GreatestCommonDivisor(a, b);
            double lcm = gcd != 0 ? a * b / gcd : 0;
            return lcm;
        }

        public static double LessCommonMultiple(params long[] values)
        {
            double lcm = 0;

            if (values.Length > 0)
            {
                lcm = values[0];

                for (int i = 1; i < values.Length; i++)
                {
                    lcm = LessCommonMultiple((long)lcm, values[i]);
                }
            }

            return lcm;
        }
    }
}
