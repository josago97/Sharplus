using System;
using System.IO;

namespace Sharplus.System
{
    public static class MiscellaneousExtensions
    {
        public static Stream Duplicate(this Stream stream)
        {
            stream.Position = 0;
            var copy = new MemoryStream();
            stream.CopyTo(copy);
            copy.Position = 0;
            stream.Position = 0;
            return copy;
        }

        public static double NextDouble(this Random random, double min, double max)
        {
            return min + random.NextDouble() * (max - min);
        }
    }
}
