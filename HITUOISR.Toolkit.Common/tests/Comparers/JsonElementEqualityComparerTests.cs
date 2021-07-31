using System;
using System.Text.Json;
using Xunit;

namespace HITUOISR.Toolkit.Common.Comparers.Tests
{
    public class JsonElementEqualityComparerTests
    {
        [Theory]
        [InlineData("-1", " -1")]
        [InlineData("true", "true")]
        [InlineData("[1,2]", "[ 1, 2 ]")]
        [InlineData("{ \"name\": \"hit\", \"count\": 10 }", "{ \"name\":\"hit\",\n\"count\":10 }")]
        public void Equal_OrdinalPropertyName(string json1, string json2)
        {
            JsonElementEqualityComparer comparer = new(StringComparer.Ordinal);
            using var jsondoc1 = JsonDocument.Parse(json1);
            using var jsondoc2 = JsonDocument.Parse(json2);
            var jsonelem1 = jsondoc1.RootElement;
            var jsonelem2 = jsondoc2.RootElement;
            Assert.True(comparer.Equals(jsonelem1, jsonelem2));
            Assert.Equal(comparer.GetHashCode(jsonelem1), comparer.GetHashCode(jsonelem2));
        }

        [Theory]
        [InlineData("\"1\"", "1")]
        [InlineData("true", "\"true\"")]
        [InlineData("[1,2]", "[2,1]")]
        [InlineData("{ \"name\": \"hit\", \"count\": 10 }", "{ \"Name\": \"hit\", \"Count\": 10 }")]
        public void NotEqual_OrdinalPropertyName(string json1, string json2)
        {
            JsonElementEqualityComparer comparer = new(StringComparer.Ordinal);
            using var jsondoc1 = JsonDocument.Parse(json1);
            using var jsondoc2 = JsonDocument.Parse(json2);
            var jsonelem1 = jsondoc1.RootElement;
            var jsonelem2 = jsondoc2.RootElement;
            Assert.False(comparer.Equals(jsonelem1, jsonelem2));
            Assert.NotEqual(comparer.GetHashCode(jsonelem1), comparer.GetHashCode(jsonelem2));
        }

        [Theory]
        [InlineData("-1", " -1")]
        [InlineData("true", "true")]
        [InlineData("[1,2]", "[ 1, 2 ]")]
        [InlineData("{ \"name\": \"hit\", \"count\": 10 }", "{ \"Name\": \"hit\",\n\"Count\": 10 }")]
        public void Equal_OrdinalIgnoreCasePropertyName(string json1, string json2)
        {
            JsonElementEqualityComparer comparer = new(StringComparer.OrdinalIgnoreCase);
            using var jsondoc1 = JsonDocument.Parse(json1);
            using var jsondoc2 = JsonDocument.Parse(json2);
            var jsonelem1 = jsondoc1.RootElement;
            var jsonelem2 = jsondoc2.RootElement;
            Assert.True(comparer.Equals(jsonelem1, jsonelem2));
            Assert.Equal(comparer.GetHashCode(jsonelem1), comparer.GetHashCode(jsonelem2));
        }

        [Theory]
        [InlineData("\"1\"", "1")]
        [InlineData("true", "\"true\"")]
        [InlineData("[1,2]", "[2,1]")]
        [InlineData("\"TRUE\"", "\"true\"")]
        [InlineData("{ \"name\": \"hit\", \"count\": 10 }", "{ \"name\": \"HIT\",\n\"Count\": 10 }")]
        public void NotEqual_OrdinalIgnoreCasePropertyName(string json1, string json2)
        {
            JsonElementEqualityComparer comparer = new(StringComparer.OrdinalIgnoreCase);
            using var jsondoc1 = JsonDocument.Parse(json1);
            using var jsondoc2 = JsonDocument.Parse(json2);
            var jsonelem1 = jsondoc1.RootElement;
            var jsonelem2 = jsondoc2.RootElement;
            Assert.False(comparer.Equals(jsonelem1, jsonelem2));
            Assert.NotEqual(comparer.GetHashCode(jsonelem1), comparer.GetHashCode(jsonelem2));
        }
    }
}
