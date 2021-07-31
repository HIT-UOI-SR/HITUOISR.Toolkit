using System;
using Xunit;

namespace HITUOISR.Toolkit.Common.Tests
{
    public class PropertyExtensionsTests
    {
        private class Person
        {
            public int Id { get; init; }
            public string? Name { get; set; }
            public string? Info => $"{Name} {Id}";
        }

        private record Point(int X, int Y);

        [Theory]
        [InlineData(typeof(Person), nameof(Person.Id))]
        [InlineData(typeof(Point), nameof(Point.X))]
        [InlineData(typeof(Point), nameof(Point.Y))]
        public void IsInitOnly_True_AsExpected(Type type, string propertyName)
        {
            var prop = type.GetProperty(propertyName)!;
            Assert.True(prop.IsInitOnly(), $"{type}.{propertyName} should be init-only.");
        }

        [Theory]
        [InlineData(typeof(Person), nameof(Person.Name))]
        [InlineData(typeof(Person), nameof(Person.Info))]
        public void IsInitOnly_False_AsExpected(Type type, string propertyName)
        {
            var prop = type.GetProperty(propertyName)!;
            Assert.False(prop.IsInitOnly(), $"{type}.{propertyName} should not be init-only.");
        }
    }
}
