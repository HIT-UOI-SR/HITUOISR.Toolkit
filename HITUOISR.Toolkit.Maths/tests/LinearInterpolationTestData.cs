using System.Collections.Generic;
using Xunit;

namespace CovRNADetect.Maths.Tests
{
    class LinearInterpolationTestData : TheoryData<(double x, double y)[], double, double>
    {
        private static readonly List<((double x, double y)[] key, (double x, double y)[] test)> data = new()
        {
            (
                new[]
                {
                    (0D, 0D),
                    (1D, 1D),
                },
                new[]
                {
                    (-1D, -1D),
                    (0D, 0D),
                    (0.3, 0.3),
                    (0.5, 0.5),
                    (1D, 1D),
                    (114514D, 114514D),
                }
            ),
            (
                new[]
                {
                    (1D, 1D),
                    (2D, 3D),
                    (3D, 4D),
                },
                new[]
                {
                    (0D, -1D),
                    (1D, 1D),
                    (1.7, 2.4),
                    (2D, 3D),
                    (2.2, 3.2),
                    (3D, 4D),
                    (4D, 5D),
                }
            ),
        };

        public LinearInterpolationTestData()
        {
            foreach (var (key, test) in data)
            {
                foreach ((double x, double y) in test)
                {
                    Add(key, x, y);
                }
            }
        }
    }
}
