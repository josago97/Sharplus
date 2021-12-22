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
        public void Concat(int[] sequence1, int[] sequence2)
        {
            int expected = sequence1.Length + 2 * sequence2.Length;
            int result = sequence1.Concat(sequence2, sequence2).Count();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Dump()
        {
            int[] items = new[] { 0, 1, 2, 3 };
            string expected = $"[{string.Join(", ", items)}]";
            Assert.True(expected == items.Dump());
        }

        public static IEnumerable<object[]> FindAllIndexesData => new[]
        {
            new object[] { new int[] { }, new int[] { 0, 1 }, 0, -1, (int x) => x < 0 },
            new object[] { new int[] { 0 }, new int[] { 0, 1 }, 0, -1, (int x) => x == 0 },
            new object[] { new int[] { }, new int[] { 0, 1, 2 }, 1, -1, (int x) => x == 0 },
            new object[] { new int[] { }, new int[] { 0, 1, 2 }, 0, 1, (int x) => x == 2 },
            new object[] { new int[] { 0, 1 }, new int[] { 0, 1, 2 }, 0, -1, (int x) => x < 2 }
        };

        [Theory]
        [MemberData(nameof(FindAllIndexesData))]
        public void FindAllIndexes(int[] expected, int[] sequence, int startIndex, int count, Func<int, bool> match)
        {
            Assert.Equal(expected, sequence.FindAllIndexes(startIndex, count, match));
        }

        public static IEnumerable<object[]> FindIndexData => new[]
        {
            new object[] { -1, new int[] { 0, 1 }, 0, -1, (int x) => x < 0 },
            new object[] { 0, new int[] { 0, 1 }, 0, -1, (int x) => x == 0 },
            new object[] { -1, new int[] { 0, 1, 2 }, 1, -1, (int x) => x == 0 },
            new object[] { -1, new int[] { 0, 1, 2 }, 0, 1, (int x) => x == 2 }
        };

        [Theory]
        [MemberData(nameof(FindIndexData))]
        public void FindIndex(int expected, int[] sequence, int startIndex, int count, Func<int, bool> match)
        {
            Assert.Equal(expected, sequence.FindIndex(startIndex, count, match));
        }

        public static IEnumerable<object[]> FlattenData => new []
        {
            new object[] { new int[] { 0, 1, 2, 3 }, new int[,] { { 0, 1 }, { 2, 3 } }, -1 },
            new object[] { new int[] { 0, 1, 2, 3 }, new List<int[]> { new int[] { 0, 1 }, new int[] { 2, 3 } }, -1 },
            new object[] { new object[] { 0, "hola", true, 3.0 }, new List<List<object[]>> { new List<object[]>() { new object[] { 0 }, new object[] { "hola" } }, new List<object[]>() { new object[] { true }, new object[] { 3.0 } } }, -1 },
            new object[] { new int[,] { { 0, 1 }, { 2, 3 } }, new int[,] { { 0, 1 }, { 2, 3 } }, 0 },
            new object[] { new int[] { 0, 1, 2, 3 }, new List<int[]> { new int[] { 0, 1 }, new int[] { 2, 3 } }, 1 },
        };

        [Theory]
        [MemberData(nameof(FlattenData))]
        public void Flatten(IEnumerable expected, IEnumerable values, int depth)
        {
            Assert.Equal(expected, values.Flatten<object>(depth));
        }

        [Theory]
        [InlineData(true, new int[] { }, new long[] { }, true)]
        [InlineData(true, new int[] { 1 }, new long[] { 1 }, false)]
        [InlineData(false, new int[] { 1 }, new long[] { 1 }, true)]
        [InlineData(false, new int[] { 1 }, new long[] { 2 }, true)]
        [InlineData(false, new int[] { 1 }, new long[] { 1, 2 }, true)]
        public void SequenceEquals(bool expected, IEnumerable<int> sequence1, IEnumerable<long> sequence2, bool useEquals)
        {
            Func<int, long, bool> objectEquals = (o1, o2) => o1.Equals(o2);
            Func<int, long, bool> operatorEquals = (o1, o2) => o1 == o2;
            Func<int, long, bool> equals = useEquals ? objectEquals : operatorEquals;

            bool result = sequence1.SequenceEquals(sequence2, equals);

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
        [InlineData(2, new int[] { 1, 1 }, 0, -1)]
        public void SubSequence(int lengthExpected, int[] values, int startIndex, int length)
        {
            var result = values.SubSequence(startIndex, length).Count();

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
