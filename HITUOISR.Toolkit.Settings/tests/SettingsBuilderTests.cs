using HITUOISR.Toolkit.XunitExtension;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HITUOISR.Toolkit.Settings.Tests
{
    public class SettingsBuilderTests
    {
        private static TheoryData<SettingsKeyInfo[]> SettingsKeysData() => new()
        {
            new SettingsKeyInfo[]
            {
                new("key", typeof(string)),
                new("num", typeof(double)) { DefaultValue = double.PositiveInfinity },
            },
            new SettingsKeyInfo[]
            {
                new("key", typeof(string)) { DefaultValue = string.Empty },
                new("num.int", typeof(int)) { IsReadOnly = true },
            },
        };

        [Theory]
        [MemberData(nameof(SettingsKeysData))]
        public void RegisterKey_AsExpected(IEnumerable<ISettingsKeyInfo> keys)
        {
            SettingsBuilder builder = new();
            foreach (var key in keys)
            {
                builder.RegisterKey(key);
            }
            Assert.Equal(keys, builder.SettingsKeys);
        }

        private static TheoryData<ISettingsKeyInfo> DuplicateKeyData() => new()
        {
            new SettingsKeyInfo("num", typeof(int)),
            new SettingsKeyInfo("num", typeof(double)),
            new SettingsKeyInfo("NUM", typeof(double)),
        };

        [Theory]
        [MemberData(nameof(DuplicateKeyData))]
        public void RegisterKey_Exists_Throw(ISettingsKeyInfo key)
        {
            SettingsBuilder builder = new();
            builder.RegisterKey(new SettingsKeyInfo("num", typeof(int)));
            Assert.Throws<SettingsKeyExistsException>(() => builder.RegisterKey(key));
        }

        private static TheoryData<ISettingsSource[]> SourcesData() => new()
        {
            new ISettingsSource[]
            {
                new Memory.MemorySettingsSource(new Dictionary<string, object?>()),
            },
            new ISettingsSource[]
            {
                new Memory.MemorySettingsSource(new Dictionary<string, object?>()),
                new Memory.MemorySettingsSource(new Dictionary<string, object?>(){ ["key"] = "build" }),
            },
        };

        private static MatrixTheoryData<SettingsKeyInfo[], ISettingsSource[]> BuildTestData() => new(SettingsKeysData(), SourcesData());

        [Theory]
        [MemberData(nameof(BuildTestData))]
        public void Build_AsExpected(IEnumerable<ISettingsKeyInfo> keys, IEnumerable<ISettingsSource> sources)
        {
            SettingsBuilder builder = new();
            foreach (var key in keys)
            {
                builder.RegisterKey(key);
            }
            foreach (var src in sources)
            {
                builder.Sources.Add(src);
            }
            var settings = builder.Build();
            Assert.Equal(keys, settings.Keys);
            Assert.Equal(sources.Count(), settings.Providers.Count());
        }
    }
}
