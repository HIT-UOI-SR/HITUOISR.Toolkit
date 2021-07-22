using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace HITUOISR.Toolkit.Common.Comparers.Tests
{
    public class SequenceEqualityComparerTests
    {
        private static TheoryData<int[], List<int>> IntEqualData() => new()
        {
            {
                new[] { 1, 2, 3 },
                new() { 1, 2, 3 }
            },
        };

        private static TheoryData<string[], List<string>> StringEqualData() => new()
        {
            {
                new[] { "AB", "2", "#" },
                new() { "AB", "2", "#" }
            },
        };

        [Theory]
        [MemberData(nameof(IntEqualData))]
        [MemberData(nameof(StringEqualData))]
        public void Equal_Default<TElement>(IEnumerable<TElement> s1, IEnumerable<TElement> s2)
        {
            SequenceEqualityComparer<TElement> comparer = new();
            Assert.True(comparer.Equals(s1, s2));
            Assert.Equal(comparer.GetHashCode(s1), comparer.GetHashCode(s2));
        }

        private static TheoryData<int[], int[]> IntNotEqualData() => new()
        {
            {
                new[] { 1, 2, 3 },
                new[] { 1, -2, 3 }
            },
        };

        private static TheoryData<string[], string[]> StringNotEqualData() => new()
        {
            {
                new[] { "AB", "2", "#" },
                new[] { "aB", " 2", "#" }
            },
        };

        [Theory]
        [MemberData(nameof(IntNotEqualData))]
        [MemberData(nameof(StringNotEqualData))]
        public void NotEqual_Default<TElement>(IEnumerable<TElement> s1, IEnumerable<TElement> s2)
        {
            SequenceEqualityComparer<TElement> comparer = new();
            Assert.False(comparer.Equals(s1, s2));
            Assert.NotEqual(comparer.GetHashCode(s1), comparer.GetHashCode(s2));
        }

        private class AbsEqualityComparer : EqualityComparer<int>
        {
            public override bool Equals([AllowNull] int x, [AllowNull] int y)
            {
                try
                {
                    return Math.Abs(x) == Math.Abs(y);
                }
                catch (OverflowException)
                {
                    return false;
                }
            }

            public override int GetHashCode([DisallowNull] int obj)
            {
                try
                {
                    return Math.Abs(obj);
                }
                catch (OverflowException)
                {
                    return obj;
                }
            }
        }

        private static TheoryData<int[], List<int>, IEqualityComparer<int>> IntComparerEqualData() => new()
        {
            {
                new[] { 1, 2, 3 },
                new() { 1, -2, 3 },
                new AbsEqualityComparer()
            },
        };

        private static TheoryData<string[], List<string>, IEqualityComparer<string>> StringComparerEqualData() => new()
        {
            {
                new[] { "AB", "2", "#" },
                new() { "aB", "2", "#" },
                StringComparer.OrdinalIgnoreCase
            },
        };

        [Theory]
        [MemberData(nameof(IntComparerEqualData))]
        [MemberData(nameof(StringComparerEqualData))]
        public void Equal_WithComparer<TElement>(IEnumerable<TElement> s1, IEnumerable<TElement> s2, IEqualityComparer<TElement> elemComparer)
        {
            SequenceEqualityComparer<TElement> comparer = new(elemComparer);
            Assert.True(comparer.Equals(s1, s2));
            Assert.Equal(comparer.GetHashCode(s1), comparer.GetHashCode(s2));
        }

        private static TheoryData<int[], int[], IEqualityComparer<int>> IntComparerNotEqualData() => new()
        {
            {
                new[] { 1, 2, int.MaxValue },
                new[] { 1, 2, int.MinValue },
                new AbsEqualityComparer()
            },
        };

        private static TheoryData<string[], string[], IEqualityComparer<string>> StringComparerNotEqualData() => new()
        {
            {
                new[] { "AB", "贰", "#" },
                new[] { "aB", "二", "#" },
                StringComparer.OrdinalIgnoreCase
            },
            {
                new[] { "AB", "2", "#" },
                new[] { "aB", " 2", "#" },
                StringComparer.OrdinalIgnoreCase
            },
        };

        [Theory]
        [MemberData(nameof(IntComparerNotEqualData))]
        [MemberData(nameof(StringComparerNotEqualData))]
        public void NotEqual_WithComparer<TElement>(IEnumerable<TElement> s1, IEnumerable<TElement> s2, IEqualityComparer<TElement> elemComparer)
        {
            SequenceEqualityComparer<TElement> comparer = new(elemComparer);
            Assert.False(comparer.Equals(s1, s2));
            Assert.NotEqual(comparer.GetHashCode(s1), comparer.GetHashCode(s2));
        }
    }
}
