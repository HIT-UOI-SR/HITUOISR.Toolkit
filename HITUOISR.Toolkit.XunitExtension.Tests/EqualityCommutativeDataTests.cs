using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HITUOISR.Toolkit.XunitExtension.Tests
{
    public class EqualityCommutativeDataTests
    {
        private static TheoryData<TheoryData<bool, int, int>, List<(bool, int, int)>> IntTestData() => new()
        {
            {
                new()
                {
                    { false, 1, 2 },
                    { false, 2, 1 },
                    { true, 4, 4 },
                    { true, 4, 4 },
                },
                new()
                {
                    (false, 1, 2),
                    (true, 4, 4),
                }
            }
        };

        private static TheoryData<TheoryData<bool, string, string>, List<(bool, string, string)>> StringTestData() => new()
        {
            {
                new()
                {
                    { false, "A", "a" },
                    { false, "a", "A" },
                    { true, string.Empty, string.Empty },
                },
                new()
                {
                    (false, "A", "a"),
                    (true, string.Empty, string.Empty),
                }
            },
            {
                new()
                {
                    { false, "a", "aa" },
                    { false, "aa", "a" },
                    { true, "cat", BuildCat() },
                    { true, BuildCat(), "cat" },
                },
                new()
                {
                    (false, "a", "aa"),
                    (true, "cat", BuildCat()),
                }
            }
        };

        private static string BuildCat() // suspend constant optimization
        {
            StringBuilder builder = new();
            builder.Append('c');
            builder.Append('a');
            builder.Append('t');
            return builder.ToString();
        }

        [Theory]
        [MemberData(nameof(IntTestData))]
        [MemberData(nameof(StringTestData))]
        public void Ctor_AsExpected<T>(TheoryData<bool, T, T> expected, IEnumerable<(bool, T, T)> source)
        {
            EqualityCommutativeData<T> data = new(source);
            Assert.Equal(expected.AsValueTuples(), data.AsValueTuples());
        }
    }
}
