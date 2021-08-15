using System;
using Xunit;

namespace CovRNADetect.Maths.Tests
{
    public class LinearInterpolationTests
    {
        [Theory]
        [ClassData(typeof(LinearInterpolationTestData))]
        public void ApplyTest((double, double)[] points, double x, double y)
        {
            LinearInterpolation interpolation = new(points);
            Assert.Equal(interpolation.Apply(x), y, 8);
        }

        [Theory]
        [ClassData(typeof(LinearInterpolationTestData))]
        public void ConvertFuncTest((double, double)[] points, double x, double y)
        {
            Func<double, double> interpolation = new LinearInterpolation(points);
            Assert.Equal(interpolation(x), y, 8);
        }
    }
}