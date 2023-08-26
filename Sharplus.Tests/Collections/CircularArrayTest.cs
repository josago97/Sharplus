using System.Linq;
using Sharplus.Collections;
using Sharplus.System.Linq;
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
        [InlineData(new int[] { 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 }, 3)]
        public void AddOverflow(int[] expected, int[] items, int size)
        {
            CircularArray<int> circularArray = new CircularArray<int>(size);

            items.ForEach(x => circularArray.Add(x));

            Assert.Equal(size, circularArray.Length);
            Assert.Equal(expected, circularArray);
        }

        [Theory]
        [InlineData(false, new int[] { 1, 2, 3, 4, 5 }, 3, 1)]
        [InlineData(true, new int[] { 1, 2, 3, 4, 5 }, 3, 4)]
        public void Contains(bool expected, int[] items, int size, int element)
        {
            CircularArray<int> circularArray = new CircularArray<int>(size);

            items.ForEach(x => circularArray.Add(x));

            Assert.Equal(expected, circularArray.Contains(element));
        }

        [Theory]
        [InlineData(-1, new int[] { 1, 2, 3, 4, 5 }, 3, 1)]
        [InlineData(1, new int[] { 1, 2, 3, 4, 5 }, 3, 4)]
        public void IndexOf(int expected, int[] items, int size, int element)
        {
            CircularArray<int> circularArray = new CircularArray<int>(size);

            items.ForEach(x => circularArray.Add(x));

            Assert.Equal(expected, circularArray.IndexOf(element));
        }

        [Theory]
        [InlineData(new int[] { 3, 1, 2 }, new int[] { 1, 2 }, 3, 0, 3)]
        [InlineData(new int[] { 1, 3, 2 }, new int[] { 1, 2 }, 3, 1, 3)]
        [InlineData(new int[] { 6, 3, 4 }, new int[] { 1, 2, 3, 4, 5 }, 3, 0, 6)]
        [InlineData(new int[] { 3, 6, 4 }, new int[] { 1, 2, 3, 4, 5 }, 3, 1, 6)]
        [InlineData(new int[] { 3, 4, 6 }, new int[] { 1, 2, 3, 4, 5 }, 3, 2, 6)]
        public void Insert(int[] expected, int[] items, int size, int index, int item)
        {
            CircularArray<int> circularArray = new CircularArray<int>(size);

            items.ForEach(x => circularArray.Add(x));
            circularArray.Insert(index, item);

            Assert.Equal(expected, circularArray);
        }

        [Theory]
        [InlineData(new int[] { 2 }, new int[] { 1, 2 }, 3, 0)]
        [InlineData(new int[] { 1 }, new int[] { 1, 2 }, 3, 1)]
        [InlineData(new int[] { 4, 5 }, new int[] { 1, 2, 3, 4, 5 }, 3, 0)]
        [InlineData(new int[] { 3, 5 }, new int[] { 1, 2, 3, 4, 5 }, 3, 1)]
        [InlineData(new int[] { 3, 4 }, new int[] { 1, 2, 3, 4, 5 }, 3, 2)]
        public void RemoveAt(int[] expected, int[] items, int size, int index)
        {
            CircularArray<int> circularArray = new CircularArray<int>(size);

            items.ForEach(x => circularArray.Add(x));
            circularArray.RemoveAt(index);

            Assert.Equal(expected, circularArray);
        }

        [Theory]
        [InlineData(new int[] { 2 }, new int[] { 1, 2 }, 3, 1)]
        [InlineData(new int[] { 1 }, new int[] { 1, 2 }, 3, 2)]
        [InlineData(new int[] { 4, 5 }, new int[] { 1, 2, 3, 4, 5 }, 3, 3)]
        [InlineData(new int[] { 3, 5 }, new int[] { 1, 2, 3, 4, 5 }, 3, 4)]
        [InlineData(new int[] { 3, 4 }, new int[] { 1, 2, 3, 4, 5 }, 3, 5)]
        public void Remove(int[] expected, int[] items, int size, int item)
        {
            CircularArray<int> circularArray = new CircularArray<int>(size);

            items.ForEach(x => circularArray.Add(x));
            circularArray.Remove(item);

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
