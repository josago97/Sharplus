using System;
using System.Collections.Generic;
using System.Text;
using Sharplus;
using Xunit;

namespace Sharplus.Tests.System
{
    public class MathPlusTest
    {
        [Theory]
        [InlineData(1, 1, 0, 2)]
        [InlineData(1, 0, 1, 2)]
        [InlineData(1, 2, 0, 1)]
        public void Clamp(double expected, double value, double min, double max)
        {
            Assert.Equal(expected, MathPlus.Clamp(value, min, max));
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
        public void Mod(int expected, int dividend, int divider)
        {
            Assert.Equal(expected, MathPlus.Mod(dividend, divider));
        }

        [Theory]
        [InlineData(0, new [] { 0 })]
        [InlineData(2, new [] { 0, 2 })]
        [InlineData(1, new [] { 2, 6, 9, 10 })]
        [InlineData(2, new [] { 2, 6, 8, 10 })]
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
