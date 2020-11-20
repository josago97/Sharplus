using System.Linq;
using Xunit;

namespace Sharplus.Tests.Linq
{
    public class MiscellaneousTest
    {
        [Theory]
        [InlineData(new int[] { 1, 2 }, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2 }, new int[] { })]
        public void Concat(int[] collection1, int[] collection2)
        {
            int expected = collection1.Length + 2 * collection2.Length;
            int result = collection1.Concat(collection2, collection2).Count();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(5, new int[] { 1, 2, 3, 4, 5 })]
        public void MaxBy(int expected, int[] values)
        {
            int result = values.MaxBy(x => x);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(new int[] { 5, 5 }, new int[] { 5, 1, 2, 3, 4, 5 })]
        public void MaxAllBy(int[] expected, int[] values)
        {
            var result = values.MaxAllBy(x => x);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1, new int[] { 1, 2, 3, 4, 5 })]
        public void MinBy(int expected, int[] values)
        {
            int result = values.MinBy(x => x);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(new int[] { 1, 1 }, new int[] { 1, 2, 3, 4, 5, 1 })]
        public void MinByAll(int[] expected, int[] values)
        {
            var result = values.MinAllBy(x => x);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Dump()
        {
            int[] items = new[] { 0, 1, 2, 3 };
            string expected = $"[{string.Join(", ", items)}]";
            Assert.True(expected == items.Dump());
        }

        [Fact]
        public void SymmetricExcept()
        {
            int[] items1 = new[] { 0, 1, 2, 3 };
            int[] items2 = new[] { 1, 3, 5 };

            int[] result = items1.SymmetricExcept(items2).ToArray();
            
            Assert.Contains(0, result);
            Assert.DoesNotContain(1, result);
            Assert.Contains(2, result);
            Assert.DoesNotContain(3, result);
            Assert.Contains(5, result);
        }
    }
}
