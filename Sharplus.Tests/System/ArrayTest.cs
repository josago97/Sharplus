using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sharplus.Tests.System
{
    public class ArrayTest
    {
        [Theory]
        [InlineData(new int[] { 0, 1 }, new int[] { 0, 1 }, 0, -1)]
        [InlineData(new int[] { 0, 1 }, new int[] { 0, 1 }, 0, 2)]
        [InlineData(new int[] { 1 }, new int[] { 0, 1 }, 1, 1)]
        [InlineData(new int[] { 1 }, new int[] { 0, 1 }, 1, 2)]
        public void SubArray(int[] expected, int[] values, int startIndex, int count)
        {
            Assert.Equal(expected, count < 0 ? values.SubArray(startIndex) : values.SubArray(startIndex, count));
        }
    }
}
