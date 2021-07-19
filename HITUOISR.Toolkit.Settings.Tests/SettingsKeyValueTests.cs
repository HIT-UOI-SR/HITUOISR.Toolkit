using HITUOISR.Toolkit.Common;
using System.Collections.Generic;
using Xunit;

namespace HITUOISR.Toolkit.Settings.Tests
{
    public class SettingsKeyValueTests
    {
        [Theory]
        [InlineData("", 0, true)]
        [InlineData("num", 1D, false)]
        [InlineData("text.lipsum", "Lorem ipsum dolor sit amet", false)]
        public void Ctor_ShouldInitProperties<T>(string path, T defValue, bool isReadonly)
        {
            ISettingsKeyInfo keyInfo = new SettingsKeyInfo(path, typeof(T))
            {
                DefaultValue = defValue,
                IsReadOnly = isReadonly,
            };
            SettingsKeyValue keyValue = new(keyInfo);

            Assert.Equal(path, keyValue.KeyInfo.Path);
            Assert.Equal(typeof(T), keyValue.KeyInfo.Type);
            Assert.Equal(isReadonly, keyValue.KeyInfo.IsReadOnly);
            Assert.Equal(defValue, keyValue.KeyInfo.DefaultValue);

            Assert.Null(keyValue.Provider);
            Assert.False(keyValue.IsModified, "Settings should not be modified.");
            Assert.True(keyValue.UsingDefaultValue, "Settings should use the default value without Provider.");
            Assert.Equal(defValue, keyValue.Value);
        }

        private static Dictionary<string, object?> DefaultData => new()
        {
            ["num"] = 16D,
            ["text.lipsum"] = "Lorem ipsum dolor sit amet",
        };

        private static ISettingsProvider DefaultProvider() => new Memory.MemorySettingsProvider(new(DefaultData));

        [Theory]
        [InlineData("num", 1D)]
        [InlineData("text.lipsum", "Lorem ipsum dolor sit amet")]
        public void GetValue_FromProvider<T>(string path, T defValue)
        {
            ISettingsKeyInfo keyInfo = new SettingsKeyInfo(path, typeof(T))
            {
                DefaultValue = defValue,
            };
            var provider = DefaultProvider();
            Assert.True(provider.TryLoad(keyInfo), "Cannot load key.");
            SettingsKeyValue keyValue = new(keyInfo)
            {
                Provider = provider,
            };
            Assert.False(keyValue.UsingDefaultValue, "Settings should not use the default value when the Provider has the key.");
            Assert.Equal(DefaultData[path].RestoreTo<T>(), keyValue.Value.RestoreTo<T>());
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData(".num", 1D)]
        [InlineData("text", "To be or not to be")]
        public void GetValue_NotFromProvider_UsingDefault<T>(string path, T defValue)
        {
            ISettingsKeyInfo keyInfo = new SettingsKeyInfo(path, typeof(T))
            {
                DefaultValue = defValue,
            };
            var provider = DefaultProvider();
            Assert.False(provider.TryLoad(keyInfo), "Provider should not load a missing key.");
            SettingsKeyValue keyValue = new(keyInfo)
            {
                Provider = provider,
            };
            Assert.True(keyValue.UsingDefaultValue, "Settings should use the default value when the Provider doesnot have the key.");
            Assert.Equal(defValue, keyValue.Value.RestoreTo<T>());
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData(".num", -1D)]
        [InlineData("num", 1D)]
        [InlineData("text", "To be or not to be")]
        [InlineData("text.lipsum", "Lorem ipsum dolor sit amet")]
        public void GetValue_WithoutProvider_AlwaysDefault<T>(string path, T defValue)
        {
            ISettingsKeyInfo keyInfo = new SettingsKeyInfo(path, typeof(T))
            {
                DefaultValue = defValue,
            };
            SettingsKeyValue keyValue = new(keyInfo);
            Assert.True(keyValue.UsingDefaultValue, "Settings should use the default value without a provider.");
            Assert.Equal(defValue, keyValue.Value.RestoreTo<T>());
        }

        [Theory]
        [InlineData("num", 1D, double.PositiveInfinity)]
        [InlineData("text", "To be or not to be", "that's a question")]
        [InlineData("text.lipsum", "Lorem ipsum dolor sit amet", "Nulla accumsan ultrices sem")]
        public void SetValue_ToProvider<T>(string path, T defValue, T value)
        {
            ISettingsKeyInfo keyInfo = new SettingsKeyInfo(path, typeof(T))
            {
                DefaultValue = defValue,
            };
            var provider = DefaultProvider();
            provider.TryLoad(keyInfo);
            SettingsKeyValue keyValue = new(keyInfo)
            {
                Provider = provider,
            };
            keyValue.Value = value;
            Assert.True(keyValue.IsModified, "Settings should be modified after setting value.");
            Assert.Equal(value, keyValue.Value.RestoreTo<T>());
            Assert.True(provider.TryGet(path, out var pvalue));
            Assert.Equal(value, pvalue.RestoreTo<T>());
        }

        [Theory]
        [InlineData("num", 1D, 'Ⅰ')]
        [InlineData("text", "To be or not to be", double.Epsilon)]
        [InlineData("text.lipsum", "Lorem ipsum dolor sit amet", 0xFF)]
        public void SetValue_InconsistentType_Throws<TKey, TNew>(string path, TKey defValue, TNew newValue)
        {
            ISettingsKeyInfo keyInfo = new SettingsKeyInfo(path, typeof(TKey))
            {
                DefaultValue = defValue,
            };
            var provider = DefaultProvider();
            provider.TryLoad(keyInfo);
            SettingsKeyValue keyValue = new(keyInfo)
            {
                Provider = provider,
            };
            Assert.Throws<SettingsTypeMismatchException>(() => keyValue.Value = newValue);
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData(".num", -1D)]
        [InlineData("num", 1D)]
        [InlineData("text.lipsum", "Lorem ipsum dolor sit amet")]
        public void SetValue_WithoutProvider_Throws<T>(string path, T value)
        {
            ISettingsKeyInfo keyInfo = new SettingsKeyInfo(path, typeof(T));
            SettingsKeyValue keyValue = new(keyInfo);
            Assert.Throws<MissingSettingsProviderException>(() => keyValue.Value = value);
        }
    }
}
