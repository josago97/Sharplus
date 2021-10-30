using System;
using System.Collections;
using System.Collections.Generic;
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
        [MemberData(nameof(FlattenData))]
        public void Flatten(IEnumerable expected, IEnumerable values, int depth)
        {
            Assert.Equal(expected, values.Flatten<object>(depth));
        }

        public static IEnumerable<object[]> FlattenData => new List<object[]>
        {
            new object[] { new int[] { 0, 1, 2, 3 }, new int[,] { { 0, 1 }, { 2, 3 } }, -1 },
            new object[] { new int[] { 0, 1, 2, 3 }, new List<int[]> { new int[] { 0, 1 }, new int[] { 2, 3 } }, -1 },
            new object[] { new object[] { 0, "hola", true, 3.0 }, new List<List<object[]>> { new List<object[]>() { new object[] { 0 }, new object[] { "hola" } }, new List<object[]>() { new object[] { true }, new object[] { 3.0 } } }, -1 },
            new object[] { new int[,] { { 0, 1 }, { 2, 3 } }, new int[,] { { 0, 1 }, { 2, 3 } }, 0 },
            new object[] { new int[] { 0, 1, 2, 3 }, new List<int[]> { new int[] { 0, 1 }, new int[] { 2, 3 } }, 1 },
        };

        [Theory]
        [InlineData(5, new int[] { 1, 2, 3, 4, 5 })]
        public void Max(int expected, int[] values)
        {
            int result = values.Max(x => x, Comparer<int>.Default);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(new int[] { 5, 5 }, new int[] { 5, 1, 2, 3, 4, 5 })]
        public void MaxAll(int[] expected, int[] values)
        {
            var result = values.MaxAll(x => x);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1, new int[] { 1, 2, 3, 4, 5 })]
        public void Min(int expected, int[] values)
        {
            int result = values.Min(x => x, Comparer<int>.Default);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(new int[] { 1, 1 }, new int[] { 1, 2, 3, 4, 5, 1 })]
        public void MinByAll(int[] expected, int[] values)
        {
            var result = values.MinAll(x => x);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Dump()
        {
            int[] items = new[] { 0, 1, 2, 3 };
            string expected = $"[{string.Join(", ", items)}]";
            Assert.True(expected == items.Dump());
        }

        [Theory]
        [InlineData(true, new int[] { }, new long[] { }, true)]
        [InlineData(true, new int[] { 1 }, new long[] { 1 }, false)]
        [InlineData(false, new int[] { 1 }, new long[] { 1 }, true)]
        [InlineData(false, new int[] { 1 }, new long[] { 2 }, true)]
        [InlineData(false, new int[] { 1 }, new long[] { 1, 2 }, true)]
        public void SequenceEqual(bool expected, IEnumerable<int> collection1, IEnumerable<long> collection2, bool useEquals)
        {
            Func<int, long, bool> objectEquals = (o1, o2) => o1.Equals(o2);
            Func<int, long, bool> operatorEquals = (o1, o2) => o1 == o2;
            Func<int, long, bool> equals = useEquals ? objectEquals : operatorEquals;

            bool result = collection1.SequenceEqual(collection2, equals);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, new int[] { }, 0, 0)]
        [InlineData(0, new int[] { }, 1, 2)]
        [InlineData(1, new int[] { 1 }, 0, 1)]
        [InlineData(1, new int[] { 1 }, 0, 2)]
        [InlineData(0, new int[] { 1 }, 0, 0)]
        [InlineData(0, new int[] { 1 }, 1, 0)]
        [InlineData(0, new int[] { 1 }, 1, 2)]
        [InlineData(1, new int[] { 1, 1 }, 0, 1)]
        public void SubCollection(int lengthExpected, int[] values, int startIndex, int length)
        {
            var result = values.SubCollection(startIndex, length).Count();

            Assert.Equal(lengthExpected, result);
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

        [Theory]
        [InlineData(new int[] { 0, 1 }, new int[] { 0, 1 }, 0, 0)]
        [InlineData(new int[] { 1, 0 }, new int[] { 0, 1 }, 0, 1)]
        [InlineData(new int[] { 1, 0 }, new int[] { 0, 1 }, 1, 0)]
        [InlineData(new int[] { 2, 1, 0 }, new int[] { 0, 1, 2 }, 0, 2)]
        public void Swap(int[] expected, int[] values, int index1, int index2)
        {
            IEnumerable<int> enumerable = values.AsEnumerable().Swap(index1, index2);
            Assert.Equal(expected, enumerable);

            values.Swap(index1, index2);
            Assert.Equal(expected, values);
        }
    }
}
