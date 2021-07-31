using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Xunit;

namespace HITUOISR.Toolkit.Settings.Memory.Tests
{
    public class MemorySettingsProviderTests
    {
        private static Dictionary<string, object?> DefaultSettingsSourceData => new()
        {
            [""] = ' ',
            ["num"] = 10,
            ["text.lipsum"] = "Lorem ipsum dolor sit amet",
        };

        [Theory]
        [InlineData("", ' ')]
        [InlineData("num", 10)]
        [InlineData("text.lipsum", "Lorem ipsum dolor sit amet")]
        public void TryLoad_Exists<T>(string key, T expected)
        {
            MemorySettingsSource source = new(DefaultSettingsSourceData);
            MemorySettingsProvider provider = new(source);
            SettingsKeyInfo keyInfo = new(key, typeof(T));
            Assert.True(provider.TryLoad(keyInfo), "TryLoad failed.");
            Assert.True(provider.TryGet(key, out var value), "TryGet failed.");
            Assert.IsType<T>(value);
            Contract.Assume(value is T);
            Assert.Equal(expected, (T)value);
        }

        [Theory]
        [InlineData(".")]
        [InlineData("text")]
        [InlineData("lipsum")]
        public void TryLoad_NotExists(string key)
        {
            MemorySettingsSource source = new(DefaultSettingsSourceData);
            MemorySettingsProvider provider = new(source);
            SettingsKeyInfo keyInfo = new(key, typeof(object));
            Assert.False(provider.TryLoad(keyInfo), "TryLoad failed.");
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("num", 16)]
        [InlineData(".num", 65536)]
        [InlineData("text", "To be or not to be")]
        [InlineData("text.lipsum", "Nulla accumsan ultrices sem")]
        public void Save_AfterModified<T>(string key, T value)
        {
            var sourceData = DefaultSettingsSourceData;
            MemorySettingsSource source = new(sourceData);
            MemorySettingsProvider provider = new(source);
            Assert.True(provider.Set(key, value), "Set failed.");
            Assert.True(provider.Save(), "Save failed");
            var expected = DefaultSettingsSourceData;
            expected[key] = value;
            Assert.Equal(expected, sourceData);
        }
    }
}
