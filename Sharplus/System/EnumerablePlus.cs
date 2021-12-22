using System.Collections.Generic;

namespace Sharplus.System
{
    public static class EnumerablePlus
    {
        /// <summary>
        /// Generates a sequence of <see cref="float"/> within a specified range and with a determined increment.
        /// </summary>
        /// <param name="start">The value of the first <see cref="float"/> in the sequence.</param>
        /// <param name="count">The number of sequential <see cref="float"/> to generate.</param>
        /// <param name="step">The increment between the generated numbers</param>
        /// <returns>
        /// An <see cref="IEnumerable{float}"/> that contains a range of sequential numbers.
        /// </returns>
        public static IEnumerable<float> Range(float start, int count, float step = 1)
        {
            float result = start;

            for (int i = 0; i < count; i++)
            {
                yield return result;

                result += step;
            }
        }

        /// <summary>
        /// Generates a sequence of <see cref="double"/> within a specified range and with a determined increment.
        /// </summary>
        /// <param name="start">The value of the first <see cref="double"/> in the sequence.</param>
        /// <param name="count">The number of sequential <see cref="double"/> to generate.</param>
        /// <param name="step">The increment between the generated numbers</param>
        /// <returns>
        /// An <see cref="IEnumerable{double}"/> that contains a range of sequential numbers.
        /// </returns>
        public static IEnumerable<double> Range(double start, int count, double step = 1)
        {
            double result = start;

            for (int i = 0; i < count; i++)
            {
                yield return result;

                result += step;
            }
        }

        /// <summary>
        /// Generates a sequence of <see cref="decimal"/> within a specified range and with a determined increment.
        /// </summary>
        /// <param name="start">The value of the first <see cref="decimal"/> in the sequence.</param>
        /// <param name="count">The number of sequential <see cref="decimal"/> to generate.</param>
        /// <param name="step">The increment between the generated numbers</param>
        /// <returns>
        /// An <see cref="IEnumerable{decimal}"/> that contains a range of sequential numbers.
        /// </returns>
        public static IEnumerable<decimal> Range(decimal start, int count, decimal step = 1)
        {
            decimal result = start;

            for (int i = 0; i < count; i++)
            {
                yield return result;

                result += step;
            }
        }
    }
}
