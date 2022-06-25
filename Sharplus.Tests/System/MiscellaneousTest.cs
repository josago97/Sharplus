using System;
using System.IO;
using Xunit;

namespace Sharplus.Tests.System
{
    public class MiscellaneousTest
    {
        [Fact]
        public void ReadAsByteArray()
        {
            byte[] data = new byte[] { 0, 1, 2, 3, 4, 5 };

            using MemoryStream memoryStream = new MemoryStream(data);

            Assert.Equal(data, memoryStream.ReadAsByteArray());
        }
    }
}
