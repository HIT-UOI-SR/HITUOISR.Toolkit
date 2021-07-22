using System;
using System.Collections.Generic;
using Xunit;

namespace HITUOISR.Toolkit.Common.Comparers.Tests
{
    public class SetEqualityComparerTests
    {
        private static TheoryData<HashSet<int>, SortedSet<int>> IntEqualTestData() => new()
        {
            {
                new() { 1, 4, 3 },
                new() { 3, 1, 4 }
            },
        };

        private static TheoryData<HashSet<string>, SortedSet<string>> StringEqualTestData() => new()
        {
            {
                new() { "a", "Ab", "#" },
                new() { "Ab", "#", "a" }
            },
        };

        [Theory]
        [MemberData(nameof(IntEqualTestData))]
        [MemberData(nameof(StringEqualTestData))]
        public void Equal<T>(ISet<T> s1, ISet<T> s2)
        {
            SetEqualityComparer<T> comparer = new();
            Assert.True(comparer.Equals(s1, s2));
            Assert.Equal(comparer.GetHashCode(s1), comparer.GetHashCode(s2));
        }

        private static TheoryData<HashSet<int>, SortedSet<int>> IntNotEqualTestData() => new()
        {
            {
                new() { 1, 4, 3 },
                new() { 3, 1, -4 }
            },
        };

        private static TheoryData<HashSet<string>, SortedSet<string>> StringNotEqualTestData() => new()
        {
            {
                new() { "a", "Ab", "#" },
                new() { "AB", "#", "A" }
            },
        };

        [Theory]
        [MemberData(nameof(IntNotEqualTestData))]
        [MemberData(nameof(StringNotEqualTestData))]
        public void NotEqual<T>(ISet<T> s1, ISet<T> s2)
        {
            SetEqualityComparer<T> comparer = new();
            Assert.False(comparer.Equals(s1, s2));
            Assert.NotEqual(comparer.GetHashCode(s1), comparer.GetHashCode(s2));
        }
    }
}
