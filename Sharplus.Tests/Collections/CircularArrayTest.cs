using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sharplus.Tests.Collections
{
    public class CircularArrayTest
    {
        [Fact]
        public void Empty()
        {
            CircularArray<int> circularArray = new CircularArray<int>(0);

            Assert.Empty(circularArray);
        }

        [Fact]
        public void Full()
        {
            CircularArray<int> circularArray = new CircularArray<int>(new int[] { 1, 2 });

            Assert.Equal(2, circularArray.Length);
        }

        [Fact]
        public void Add()
        {
            CircularArray<int> circularArray = new CircularArray<int>(2);
            circularArray.Add(1);

            Assert.Single(circularArray);
        }

        [Fact]
        public void AddRange()
        {
            int count = 5;
            CircularArray<int> circularArray = new CircularArray<int>(count);
            circularArray.AddRange(Enumerable.Range(0, count));

            Assert.Equal(count, circularArray.Length);
        }

        [Theory]
        [InlineData(new int[] {3, 4, 5}, new int[] {1, 2, 3, 4, 5}, 3)]
        public void AddOverflow(int[] expected, int[] items, int size)
        {
            CircularArray<int> circularArray = new CircularArray<int>(size);

            items.ForEach(x => circularArray.Add(x));

            Assert.Equal(size, circularArray.Length);
            Assert.Equal(expected, circularArray);
        }

        [Theory]
        [InlineData(new int[] { 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 }, 3)]
        public void ToArray(int[] expected, int[] items, int size)
        {
            CircularArray<int> circularArray = new CircularArray<int>(size);

            items.ForEach(x => circularArray.Add(x));

            Assert.Equal(size, circularArray.Length);
            Assert.Equal(expected, circularArray.ToArray());
        }
    }
}
