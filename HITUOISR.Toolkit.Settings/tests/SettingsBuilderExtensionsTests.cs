using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HITUOISR.Toolkit.Settings.Tests
{
    public class SettingsBuilderExtensionsTests
    {
        [Fact()]
        public void GetSerices_WithoutSet_Null()
        {
            SettingsBuilder builder = new();
            Assert.Null(builder.GetServices());
        }

        [Fact()]
        public void SetServices_GetSerices_AsExpected()
        {
            ServiceCollection services = new();
            SettingsBuilder builder = new();
            builder.SetServices(services);
            Assert.Equal(services, builder.GetServices());
        }

        [Theory]
        [InlineData("num", 0, false)]
        [InlineData("key", "num", true)]
        public void RegisterKey_AsExpected<T>(string path, T defValue, bool isReadonly) where T : notnull
        {
            SettingsBuilder builder = new();
            builder.RegisterKey(path, defValue, isReadonly);
            Assert.Collection(builder.SettingsKeys,
                key =>
                {
                    Assert.Equal(path, key.Path);
                    Assert.Equal(typeof(T), key.Type);
                    Assert.Equal(defValue, key.DefaultValue);
                    Assert.Equal(isReadonly, key.IsReadOnly);
                });
        }
    }
}
