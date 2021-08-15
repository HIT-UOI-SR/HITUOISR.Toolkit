using System;
using Xunit;

namespace CovRNADetect.Maths.Tests
{
    public class LinearInterpolationTests
    {
        private static object[][] InsufficientPointsData() => new object[][]
        {
            Array.Empty<object>(),
            new object[] { (0D, 0D) },
        };

        [Theory]
        [MemberData(nameof(InsufficientPointsData))]
        public void Ctor_InsufficientPoint_Throws(params (double, double)[] points) =>
            Assert.Throws<ArgumentException>(() => new LinearInterpolation(points));

        [Theory]
        [ClassData(typeof(LinearInterpolationTestData))]
        public void Apply_AsExpected((double, double)[] points, double x, double y)
        {
            LinearInterpolation interpolation = new(points);
            Assert.Equal(interpolation.Apply(x), y, 8);
        }

        [Theory]
        [ClassData(typeof(LinearInterpolationTestData))]
        public void ConvertFunc_AsExpected((double, double)[] points, double x, double y)
        {
            Func<double, double> interpolation = new LinearInterpolation(points);
            Assert.Equal(interpolation(x), y, 8);
        }
    }
}
