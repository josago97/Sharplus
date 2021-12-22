using System.IO;

namespace System
{
    public static class MiscellaneousExtensions
    {
        /// <summary>
        /// Duplicates a stream with its content.
        /// </summary>
        /// <param name="stream">The stream to duplicate.</param>
        /// <returns>
        /// A new stream with the content of the other.
        /// </returns>
        public static Stream Duplicate(this Stream stream)
        {
            stream.Position = 0;
            MemoryStream copy = new MemoryStream();
            stream.CopyTo(copy);
            copy.Position = 0;
            stream.Position = 0;
            return copy;
        }

        /// <summary>
        /// Returns a random floating-point number that is within a specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned.</param>
        /// <param name="max">The exclusive upper bound of the random number returned. max must be greater than or equal to min.</param>
        /// <returns>
        /// A double-precision floating point number greater than or equal to min and less than max;
        /// that is, the range of return values includes min but not max. If min
        /// equals max, min is returned.
        /// </returns>
        public static double NextDouble(this Random random, double min, double max)
        {
            if (max < min) throw new ArgumentException("max must be greater than or equal to min");
            return min + random.NextDouble() * (max - min);
        }
    }
}
