using Xunit;
using System.Collections.Generic;

namespace HITUOISR.Toolkit.XunitExtension.Tests
{
    public class ComparationCommutativeDataTests
    {
        private static TheoryData<TheoryData<int, int, int>, List<(int, int, int)>> IntTestData() => new()
        {
            {
                new()
                {
                    { -1, 1, 2 },
                    { 1, 2, 1 },
                    { 0, 4, 4 },
                },
                new()
                {
                    (-1, 1, 2),
                    (0, 4, 4),
                }
            }
        };

        [Theory]
        [MemberData(nameof(IntTestData))]
        public void Ctor_AsExpected<T>(TheoryData<int, T, T> expected, IEnumerable<(int, T, T)> source)
        {
            ComparationCommutativeData<T> data = new(source);
            Assert.Equal(expected.AsValueTuples(), data.AsValueTuples());
        }

        private static TheoryData<TheoryData<int, int, int, string>, List<(int, int, int, string)>> IntExtraTestData() => new()
        {
            {
                new()
                {
                    { -1, 1, 2, "g" },
                    { 1, 2, 1, "g" },
                    { 0, 4, 4, "f" },
                },
                new()
                {
                    (-1, 1, 2, "g"),
                    (0, 4, 4, "f"),
                }
            }
        };

        [Theory]
        [MemberData(nameof(IntExtraTestData))]
        public void CtorExtra_AsExpected<T, TExtra>(TheoryData<int, T, T, TExtra> expected, IEnumerable<(int, T, T, TExtra)> source)
        {
            ComparationCommutativeData<T, TExtra> data = new(source);
            Assert.Equal(expected.AsValueTuples(), data.AsValueTuples());
        }
    }
}
