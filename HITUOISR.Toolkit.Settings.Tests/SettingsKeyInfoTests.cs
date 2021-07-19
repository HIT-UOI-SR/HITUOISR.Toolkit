using HITUOISR.Toolkit.Common;
using System;
using Xunit;

namespace HITUOISR.Toolkit.Settings.Tests
{
    public class SettingsKeyInfoTests
    {
        [Theory]
        [InlineData("", typeof(object))]
        [InlineData("num", typeof(int))]
        [InlineData("text", typeof(string))]
        public void Ctor_ShouldInitProperties(string path, Type type)
        {
            SettingsKeyInfo keyInfo = new(path, type);
            Assert.Equal(path, keyInfo.Path);
            Assert.Equal(type, keyInfo.Type);
            Assert.False(keyInfo.IsReadOnly);
            Assert.Equal(type.GetLanguageDefault(), keyInfo.DefaultValue);
        }
    }
}
