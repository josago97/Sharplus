﻿using System;
using System.Linq;
using Xunit;

namespace Sharplus.Tests.Linq
{
    public class StatisticsTest
    {
        [Theory]
        [InlineData(-1.3, 0, new double[] { 1, 2, 3, 4, 5 })]
        [InlineData(-1.15, 0.01, new double[] { 1, 2, 3, 2, 1 })]
        public void Kurtosis(double expected, double error, double[] values)
        {
            double result = values.Kurtosis();
            Assert.InRange(result, expected - error, expected + error);
        }

        [Theory]
        [InlineData(0, new double[] { 0 })]
        [InlineData(1.5, new double[] { 0, 1, 2, 3 })]
        [InlineData(2, new double[] { 0, 1, 2, 3, 4 })]
        public void Median(double expected, double[] values)
        {
            double result = values.Median();
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(new double[] { 0 }, new double[] { 0 })]
        [InlineData(new double[] { 0, 1, 2, 3 }, new double[] { 0, 1, 2, 3 })]
        [InlineData(new double[] { 2 }, new double[] { 0, 1, 2, 2, 3, 4 })]
        public void Mode(double[] expected, double[] values)
        {
            var result = values.Mode();
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, new double[] { 0 })]
        [InlineData(1.25, new double[] { 0, 1, 2, 3 })]
        public void Variance(double expected, double[] values)
        {
            var result = values.Variance();
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, 0, new double[] { 1, 2, 3, 4, 5 })]
        [InlineData(0.34, 0.01, new double[] { 1, 2, 3, 2, 1 })]
        public void Skewness(double expected, double error, double[] values)
        {
            double result = values.Skewness();
            Assert.InRange(result, expected - error, expected + error);
        }

        [Theory]
        [InlineData(0, new double[] { 0 })]
        [InlineData(0.5, new double[] { 0, 1 })]
        public void StandardDeviation(double expected, double[] values)
        {
            var result = values.StandardDeviation();
            Assert.Equal(expected, result);
        }
    }
}
