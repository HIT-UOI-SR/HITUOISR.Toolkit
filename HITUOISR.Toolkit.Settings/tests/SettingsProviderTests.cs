using System.Diagnostics.Contracts;
using Xunit;

namespace HITUOISR.Toolkit.Settings.Tests
{
    public class SettingsProviderTests
    {
        /// <summary>
        /// 空设置源（不提供任何来源）。
        /// </summary>
        internal class NullSettingsSource : ISettingsSource
        {
            public bool IsReadOnly => false;

            public ISettingsProvider Build(ISettingsBuilder builder) => new NullSettingsProvider(this);
        }

        /// <summary>
        /// 空设置提供器（空设置源）。仅用于测试基类默认实现。
        /// </summary>
        internal class NullSettingsProvider : SettingsProvider
        {
            public NullSettingsProvider(NullSettingsSource source) : base(source) { }
        }

        [Fact()]
        public void TryLoad_Default_NothingButFalse()
        {
            NullSettingsSource source = new();
            NullSettingsProvider provider = new(source);
            ISettingsKeyInfo key = new SettingsKeyInfo("this", typeof(object));
            Assert.False(provider.TryLoad(key));
        }

        [Fact()]
        public void Save_Default_NothingButFalse()
        {
            NullSettingsSource source = new();
            NullSettingsProvider provider = new(source);
            Assert.False(provider.Save());
        }

        [Fact()]
        public void TryGet_Default_False()
        {
            NullSettingsSource source = new();
            NullSettingsProvider provider = new(source);
            Assert.False(provider.TryGet("num", out _));
        }

        [Fact()]
        public void Set_Then_TryGet_AsExpected()
        {
            var key = "num";
            var setval = 1.0;
            NullSettingsSource source = new();
            NullSettingsProvider provider = new(source);
            Assert.True(provider.Set(key, setval), "Set failed.");
            Assert.True(provider.TryGet(key, out var value), "TryGet failed.");
            Assert.IsType<double>(value);
            Contract.Assume(value is double);
            Assert.Equal(setval, (double)value);
        }

        [Fact()]
        public void ToString_Default_TypeName()
        {
            NullSettingsSource source = new();
            NullSettingsProvider provider = new(source);
            Assert.Equal(nameof(NullSettingsProvider), provider.ToString());
        }
    }
}
