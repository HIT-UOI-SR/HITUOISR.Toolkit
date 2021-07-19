using System;
using Xunit;

namespace HITUOISR.Toolkit.Settings.Tests
{
    public class SettingsPathTests
    {
        [Theory]
        [InlineData("key", "key")]
        [InlineData("parent.key", "parent", "key")]
        [InlineData("root.parent.key", "root", "parent", "key")]
        [InlineData("parent..key", "parent", "", "key")]
        [InlineData("parent..", "parent", "", "")]
        public void Combine_AsExpected(string expected, params string[] segments) => Assert.Equal(expected, SettingsPath.Combine(segments), StringComparer.OrdinalIgnoreCase);

        [Theory]
        [InlineData("", "")]
        [InlineData("key", "key")]
        [InlineData("key", ".key")]
        [InlineData("key", "parent.key")]
        [InlineData("key", ".parent.key")]
        [InlineData("key", "root.parent.key")]
        [InlineData("y", "k.e.y")]
        [InlineData("", "key...")]
        public void GetSectionKey_AsExpected(string expected, string path) => Assert.Equal(expected, SettingsPath.GetSectionKey(path), StringComparer.OrdinalIgnoreCase);

        [Theory]
        [InlineData("", "")]
        [InlineData("", "key")]
        [InlineData("", ".key")]
        [InlineData("parent", "parent.key")]
        [InlineData(".parent", ".parent.key")]
        [InlineData("root.parent", "root.parent.key")]
        [InlineData("k.e", "k.e.y")]
        [InlineData("key..", "key...")]
        public void GetParentPath_AsExpected(string expected, string path) => Assert.Equal(expected, SettingsPath.GetParentPath(path), StringComparer.OrdinalIgnoreCase);

        [Theory]
        [InlineData("", "", null)]
        [InlineData("key", "key", null)]
        [InlineData("parent", "parent.key", null)]
        [InlineData("root", "root.parent.key", null)]
        [InlineData("", ".parent.key", null)]
        [InlineData(".parent", ".parent.key", "")]
        [InlineData("root.parent", "root.parent.key", "root")]
        [InlineData("root.parent.key", "root.parent.key", "root.parent")]
        [InlineData(null, "parent.key", "")]
        [InlineData(null, "root.parent.key", "parent.key")]
        [InlineData(null, "root.parent.key", "parent")]
        [InlineData(null, "root.parent.key", "key")]
        public void GetSubsection_AsExpected(string? expected, string path, string? section) => Assert.Equal(expected, SettingsPath.GetSubsection(path, section), StringComparer.OrdinalIgnoreCase);
    }
}
