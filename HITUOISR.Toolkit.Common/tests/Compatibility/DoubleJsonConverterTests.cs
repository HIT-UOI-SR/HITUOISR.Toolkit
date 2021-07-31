using System.Text.Json;
using Xunit;

namespace HITUOISR.Toolkit.Common.Compatibility.Tests
{
    public class DoubleJsonConverterTests
    {
        [Theory]
        [InlineData(0D)]
        [InlineData(1D)]
        [InlineData(-1D)]
        [InlineData(double.MaxValue)]
        [InlineData(double.MinValue)]
        [InlineData(double.Epsilon)]
        [InlineData(-double.Epsilon)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.NegativeInfinity)]
        [InlineData(double.NaN)]
        public void SerDe_ShouldRestore(double origin)
        {
            JsonSerializerOptions options = new() { Converters = { new DoubleJsonConverter() } };
            var json = JsonSerializer.Serialize(origin, options);
            var restore = JsonSerializer.Deserialize<double>(json, options);
            Assert.Equal(origin, restore, 14);
        }
    }
}
