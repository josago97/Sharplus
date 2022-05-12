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

        [Fact]
        public void AddOverflow()
        {
            int[] items = new int[] { 1, 2, 3, 4 };
            int newItem = 5;
            CircularArray<int> circularArray = new CircularArray<int>(items);
            circularArray.Add(newItem);

            Assert.Equal(items.Length, circularArray.Length);
            Assert.Equal(newItem, circularArray[circularArray.Length - 1]);
        }
    }
}
