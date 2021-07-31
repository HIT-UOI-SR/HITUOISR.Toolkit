using HITUOISR.Toolkit.Common;
using HITUOISR.Toolkit.Settings.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace HITUOISR.Toolkit.Settings.Tests
{
    public class SettingsRootTests
    {
        private static Dictionary<string, object?> DefaultSourceData => new(StringComparer.OrdinalIgnoreCase)
        {
            ["num"] = 10,
            ["num.real"] = 1.5,
            ["text.lipsum.1"] = "Lorem ipsum dolor sit amet",
            ["text.lipsum.2"] = "Nulla accumsan ultrices sem",
            ["text.Hamlet"] = "To be or not to be",
            ["unused"] = decimal.One,
        };

        private static List<SettingsKeyInfo> DefaultKeys => new()
        {
            new("num", typeof(int)),
            new("num.int", typeof(int)) { DefaultValue = -1 },
            new("num.real", typeof(double)) { DefaultValue = Math.E },
            new("text.lipsum.1", typeof(string)) { IsReadOnly = true, DefaultValue = string.Empty },
            new("text.lipsum.2", typeof(string)) { IsReadOnly = true, DefaultValue = string.Empty },
            new("text.lipsum.3", typeof(string)) { IsReadOnly = true, DefaultValue = "Etiam purus justo", },
            new("text.hamlet", typeof(string)) { DefaultValue = string.Empty },
        };

        private sealed class SettingsKeyInfoEqualityComparer : EqualityComparer<ISettingsKeyInfo>
        {
            public override bool Equals(ISettingsKeyInfo? x, ISettingsKeyInfo? y) =>
                x == null && y == null ||
                (x != null && y != null &&
                string.Equals(x.Path, y.Path, StringComparison.OrdinalIgnoreCase) &&
                x.Type == y.Type &&
                x.IsReadOnly == y.IsReadOnly);

            public override int GetHashCode([DisallowNull] ISettingsKeyInfo obj) =>
                HashCode.Combine(obj.Path.ToUpperInvariant(), obj.Type, obj.IsReadOnly);
        }

        [Fact()]
        public void Ctor_ShouldInitProperties()
        {
            MemorySettingsProvider provider = new(new(DefaultSourceData));
            SettingsRoot settings = new(new[] { provider }, DefaultKeys);
            Assert.Equal(new List<ISettingsProvider> { provider }, settings.Providers);
            Assert.Equal(DefaultKeys, settings.Keys, new SettingsKeyInfoEqualityComparer());
        }

        [Theory]
        [InlineData("num")]
        [InlineData("text")]
        public void GetSection_AsExpected(string key)
        {
            SettingsRoot settings = new(Array.Empty<ISettingsProvider>(), DefaultKeys);
            Assert.Equal(key, settings.GetSection(key).Key);
            Assert.Equal(key, settings.GetSection(key).Path);
        }

        [Fact()]
        public void GetChildren_AsExpected()
        {
            SettingsRoot settings = new(Array.Empty<ISettingsProvider>(), DefaultKeys);
            Assert.Equal(new[] { "num", "text" }, settings.GetChildren().Select(sec => sec.Path));
        }

        [Theory]
        [InlineData("num", 10)]
        [InlineData("Num.int", -1)]
        [InlineData("num.Real", 1.5)]
        [InlineData("text.lipsum.1", "Lorem ipsum dolor sit amet")]
        [InlineData("text.LIPSUM.2", "Nulla accumsan ultrices sem")]
        [InlineData("tEXt.lipsum.3", "Etiam purus justo")]
        [InlineData("text.hamlet", "To be or not to be")]
        public void GetValue_AsExpected<T>(string key, T value)
        {
            MemorySettingsProvider provider = new(new(DefaultSourceData));
            SettingsRoot settings = new(new[] { provider }, DefaultKeys);
            Assert.Equal(value, settings.GetValue<T>(key));
        }

        [Theory]
        [InlineData("num", default(int))]
        [InlineData("Num.int", -1)]
        [InlineData("num.Real", Math.E)]
        [InlineData("text.lipsum.1", "")]
        [InlineData("text.LIPSUM.2", "")]
        [InlineData("tEXt.lipsum.3", "Etiam purus justo")]
        [InlineData("text.hamlet", "")]
        public void GetValue_WithoutProvider_AlwaysDefault<T>(string key, T def)
        {
            SettingsRoot settings = new(Array.Empty<ISettingsProvider>(), DefaultKeys);
            Assert.Equal(def, settings.GetValue<T>(key));
        }

        [Theory]
        [InlineData("num", default(double))]
        [InlineData("Num.int", "")]
        [InlineData("num.REAL", default(float))]
        [InlineData("text.Hamlet", default(int))]
        public void GetValue_InconsistentType_Throws<T>(string key, T _)
        {
            MemorySettingsProvider provider = new(new(DefaultSourceData));
            SettingsRoot settings = new(new[] { provider }, DefaultKeys);
            Assert.IsType<T>(_);
            Assert.Throws<SettingsTypeMismatchException>(() => settings.GetValue<T>(key));
        }

        [Theory]
        [InlineData("num.")]
        [InlineData(".num")]
        [InlineData("text.lipsum")]
        [InlineData("text.lipsum.0")]
        [InlineData("texthamlet")]
        [InlineData("unused")]
        public void GetValue_UnknownKey_Throws(string key)
        {
            MemorySettingsProvider provider = new(new(DefaultSourceData));
            SettingsRoot settings = new(new[] { provider }, DefaultKeys);
            Assert.Throws<SettingsKeyNotFoundException>(() => settings.GetValue<object?>(key));
        }

        [Theory]
        [InlineData("num", 10)]
        [InlineData("nUm.int", default(int))]
        [InlineData("num.rEAl", 1.5)]
        [InlineData("text.lipsum.1", "Lorem ipsum dolor sit amet")]
        [InlineData("text.LIPSUM.2", "Nulla accumsan ultrices sem")]
        [InlineData("teXt.lipsum.3", default(string))]
        [InlineData("text.hamlet", "To be or not to be")]
        public void GetValueOrElse_AsExpected<T>(string key, T value)
        {
            MemorySettingsProvider provider = new(new(DefaultSourceData));
            SettingsRoot settings = new(new[] { provider }, DefaultKeys);
            Assert.Equal(value, settings.GetValueOrElse(key, () => default(T)));
        }

        [Theory]
        [InlineData("num", -3)]
        [InlineData("Num.int", 255)]
        [InlineData("num.Real", Math.PI)]
        [InlineData("text.lipsum.1", "Nulla accumsan ultrices sem")]
        [InlineData("text.LIPSUM.2", "Lorem ipsum dolor sit amet")]
        [InlineData("tEXt.lipsum.3", default(string))]
        [InlineData("text.hamlet", "To be")]
        public void GetValueOrElse_WithoutProvider_AlwaysDefault<T>(string key, T def)
        {
            SettingsRoot settings = new(Array.Empty<ISettingsProvider>(), DefaultKeys);
            Assert.Equal(def, settings.GetValueOrElse<T>(key, () => def));
        }

        [Theory]
        [InlineData("num", default(double))]
        [InlineData("Num.int", "")]
        [InlineData("num.REAL", default(float))]
        [InlineData("text.Hamlet", default(int))]
        public void GetValueOrElse_InconsistentType_AlwaysDefault<T>(string key, T def)
        {
            MemorySettingsProvider provider = new(new(DefaultSourceData));
            SettingsRoot settings = new(new[] { provider }, DefaultKeys);
            Assert.Equal(def, settings.GetValueOrElse(key, () => def));
        }


        [Theory]
        [InlineData("num.", double.NaN)]
        [InlineData(".num", double.PositiveInfinity)]
        [InlineData("text.lipsum", "Lorem ipsum dolor sit amet")]
        [InlineData("text.lipsum.0", 'L')]
        [InlineData("texthamlet", "that's a question")]
        [InlineData("unused", 1)]
        public void GetValueOrElse_UnknownKey_AlwaysDefault<T>(string key, T def)
        {
            MemorySettingsProvider provider = new(new(DefaultSourceData));
            SettingsRoot settings = new(new[] { provider }, DefaultKeys);
            Assert.Equal(def, settings.GetValueOrElse(key, () => def));
        }

        [Theory]
        [InlineData("num", 16)]
        [InlineData("num.int", 233)]
        [InlineData("num.real", 3.1415)]
        [InlineData("text.Hamlet", "that's a question")]
        public void Reload_AsExpected<T>(string key, T newValue)
        {
            var data = DefaultSourceData;
            MemorySettingsProvider provider = new(new(data));
            SettingsRoot settings = new(new[] { provider }, DefaultKeys);
            Assert.NotEqual(newValue, settings.GetValue<T>(key));
            data[key] = newValue;
            settings.Reload();
            Assert.Equal(newValue, settings.GetValue<T>(key));
        }

        [Theory]
        [InlineData("num", 16)]
        [InlineData("num.int", 233)]
        [InlineData("num.real", 3.1415)]
        [InlineData("text.hamlet", "that's a question")]
        public void Save_AsExpected<T>(string key, T newValue)
        {
            var data = DefaultSourceData;
            MemorySettingsProvider provider = new(new(data));
            SettingsRoot settings = new(new[] { provider }, DefaultKeys);
            settings.SetValue(key, newValue);
            settings.Save();
            Assert.Equal(newValue, data[key].RestoreTo<T>());
        }

        [Theory]
        [InlineData("num", 16)]
        [InlineData("num.int", 233)]
        [InlineData("num.real", 3.1415)]
        [InlineData("text.hamlet", "that's a question")]
        public void SetValue_AsExpected<T>(string key, T newValue)
        {
            MemorySettingsProvider provider = new(new(DefaultSourceData));
            SettingsRoot settings = new(new[] { provider }, DefaultKeys);
            settings.SetValue(key, newValue);
            Assert.Equal(newValue, settings.GetValue<T>(key));
        }

        [Theory]
        [InlineData("num", 16)]
        [InlineData("num.int", 233)]
        [InlineData("num.real", 3.1415)]
        [InlineData("text.hamlet", "that's a question")]
        public void SetValue_WithoutProvider_Throws<T>(string key, T newValue)
        {
            SettingsRoot settings = new(Array.Empty<ISettingsProvider>(), DefaultKeys);
            Assert.Throws<MissingSettingsProviderException>(() => settings.SetValue(key, newValue));
        }

        [Theory]
        [InlineData("num", 16D)]
        [InlineData("num.int", '①')]
        [InlineData("num.real", "π")]
        [InlineData("text.hamlet", 202)]
        public void SetValue_InconsistentType_Throws<T>(string key, T newValue)
        {
            MemorySettingsProvider provider = new(new(DefaultSourceData));
            SettingsRoot settings = new(new[] { provider }, DefaultKeys);
            Assert.Throws<SettingsTypeMismatchException>(() => settings.SetValue(key, newValue));
        }

        [Theory]
        [InlineData("text.lipsum.1", "Aenean vulputate enim at molestie pellentesque")]
        [InlineData("text.LIPSUM.2", "Nulla a tortor sed mi sodales suscipit ut at magna")]
        [InlineData("TeXt.LiPsum.3", "Quisque at ligula nec velit efficitur viverra")]
        public void SetValue_ReadOnly_Throws<T>(string key, T newValue)
        {
            MemorySettingsProvider provider = new(new(DefaultSourceData));
            SettingsRoot settings = new(new[] { provider }, DefaultKeys);
            Assert.Throws<SettingsReadOnlyExcepetion>(() => settings.SetValue(key, newValue));
        }

        [Theory]
        [InlineData("num.")]
        [InlineData(".num")]
        [InlineData("text.lipsum")]
        [InlineData("text.lipsum.0")]
        [InlineData("texthamlet")]
        [InlineData("unused")]
        public void SetValue_UnknownKey_Throws(string key)
        {
            MemorySettingsProvider provider = new(new(DefaultSourceData));
            SettingsRoot settings = new(new[] { provider }, DefaultKeys);
            Assert.Throws<SettingsKeyNotFoundException>(() => settings.SetValue<object?>(key, null));
        }

        [Fact()]
        public void Dispose_AsExpected()
        {
            MemorySettingsProvider provider1 = new(new(DefaultSourceData));
            DisposableFakeSettingsProvider provider2 = new();
            SettingsRoot settings = new(new ISettingsProvider[] { provider1, provider2 }, DefaultKeys);
            settings.Dispose();
            Assert.True(provider2.IsDisposed, "Disposal of SettingsRoot should dispose of all providers.");
        }

        private sealed class DisposableFakeSettingsProvider : ISettingsProvider, IDisposable
        {
            public bool IsReadOnly => false;

            public bool IsDisposed { get; private set; }

            public void Dispose() => IsDisposed = true;
            public bool Save() => false;
            public bool Set(string key, object? value) => false;
            public bool TryGet(string key, out object? value) { value = null; return false; }
            public bool TryLoad(ISettingsKeyInfo key, bool reloadRequest = false) => false;
        }
    }
}
