﻿using System.IO;

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
            MemoryStream copy = new MemoryStream();
            long streamPosition = stream.Position;
            stream.Position = 0;
            stream.CopyTo(copy);
            copy.Position = 0;
            stream.Position = streamPosition;

            return copy;
        }

        /// <summary>
        /// Writes the stream contents to a byte array, regardless of the <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <returns>
        /// A new byte array.
        /// </returns>
        public static byte[] ReadAsByteArray(this Stream stream)
        {
            byte[] result;

            if (stream is MemoryStream memoryStream)
            {
                result = memoryStream.ToArray();
            }
            else
            {
                using (memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    result = memoryStream.ToArray();
                }
            }

            return result;
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
