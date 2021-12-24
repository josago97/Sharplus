using System;
using System.Collections.Generic;
using Xunit;

namespace Sharplus.Tests.System
{
    public class MathPlusTest
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(5 * 4 * 3 * 2, 5)]
        public void Factorial(long expected, long value)
        {
            Assert.Equal(expected, MathPlus.Factorial(value));
        }

        [Theory]
        [InlineData(new long[] { 0 }, 0)]
        [InlineData(new long[] { 0, 1 }, 1)]
        [InlineData(new long[] { 0, 1, 1, 2, 3, 5 }, 5)]
        public void Fibonacci(IEnumerable<long> expected, int value)
        {
            Assert.Equal(expected, MathPlus.Fibonacci(value));
        }

        [Theory]
        [InlineData(0, 1, 2, 0)]
        [InlineData(0.5, 1, 2, 1.5)]
        [InlineData(1, 1, 2, 3)]
        public void InverseLerp(double expected, double a, double b, double value)
        {
            Assert.Equal(expected, MathPlus.InverseLerp(a, b, value));
        }

        [Theory]
        [InlineData(true, 1, 1, 0)]
        [InlineData(true, 1, 1, 1)]
        [InlineData(false, 1, 2, 0)]
        [InlineData(true, 1, 2, 1)]
        public void IsErrorInRange(bool expected, double inferredValue, double expectedValue, double errorRange)
        {
            Assert.Equal(expected, MathPlus.IsErrorInRange(inferredValue, expectedValue, errorRange));
        }

        [Theory]
        [InlineData(1, 1, 2, 0)]
        [InlineData(1.5, 1, 2, 0.5)]
        [InlineData(2, 1, 2, 2)]
        public void Lerp(double expected, double a, double b, double t)
        {
            Assert.Equal(expected, MathPlus.Lerp(a, b, t));
        }

        [Theory]
        [InlineData(0, 2, 2)]
        [InlineData(1, 1, 2)]
        [InlineData(0, -2, 2)]
        [InlineData(1, -1, 2)]
        public void Mod(int expected, int dividend, int divisor)
        {
            Assert.Equal(expected, MathPlus.Mod(dividend, divisor));
        }

        [Theory]
        [InlineData(0, new[] { 0 })]
        [InlineData(2, new[] { 0, 2 })]
        [InlineData(1, new[] { 2, 6, 9, 10 })]
        [InlineData(2, new[] { 2, 6, 8, 10 })]
        public void GreatestCommonDivisor(int expected, int[] values)
        {
            Assert.Equal(expected, MathPlus.GreatestCommonDivisor(values));
        }

        [Theory]
        [InlineData(0, new[] { 0 })]
        [InlineData(2, new[] { 2, 2 })]
        [InlineData(6, new[] { 2, 3 })]
        public void LessCommonMultiple(int expected, int[] values)
        {
            Assert.Equal(expected, MathPlus.LessCommonMultiple(values));
        }
    }
}
