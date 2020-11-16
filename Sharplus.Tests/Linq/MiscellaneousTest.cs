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
