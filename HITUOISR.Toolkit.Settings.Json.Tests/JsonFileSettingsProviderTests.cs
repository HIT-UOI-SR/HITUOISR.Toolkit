using HITUOISR.Toolkit.Common;
using HITUOISR.Toolkit.Common.Comparers;
using Microsoft.Extensions.FileProviders;
using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.Json;
using Xunit;

namespace HITUOISR.Toolkit.Settings.Json.Tests
{
    public class JsonFileSettingsProviderTests
    {
        private static readonly string BaseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        private static readonly string PhysicalFileName = "file-settings.json";
        private static TheoryData<string, object> LoadExistsTestData() => new()
        {
            {
                "num",
                10
            },
            {
                "num.real",
                double.NaN
            },
            {
                "text.lipsum",
                "Lorem ipsum dolor sit amet"
            },
            {
                "point",
                new Point(3, 4)
            },
            {
                "array.real",
                new[] { -1, 2.35e14, Math.PI }
            }
        };

        [Theory]
        [MemberData(nameof(LoadExistsTestData))]
        public void TryLoad_Exists<T>(string key, T expected)
        {
            JsonFileSettingsSource source = new(PhysicalFileName)
            {
                FileProvider = new PhysicalFileProvider(BaseDirectory),
            };
            JsonFileSettingsProvider provider = new(source);
            SettingsKeyInfo keyInfo = new(key, typeof(T));
            Assert.True(provider.TryLoad(keyInfo), "TryLoad failed.");
            Assert.True(provider.TryGet(key, out var value), "TryGet failed.");
            Assert.IsType<T>(value);
            Contract.Assume(value is T);
            Assert.Equal(expected, (T)value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("text")]
        [InlineData(".lipsum")]
        public void TryLoad_NotExists(string key)
        {
            JsonFileSettingsSource source = new(PhysicalFileName)
            {
                FileProvider = new PhysicalFileProvider(BaseDirectory),
            };
            JsonFileSettingsProvider provider = new(source);
            SettingsKeyInfo keyInfo = new(key, typeof(object));
            Assert.False(provider.TryLoad(keyInfo), "TryLoad failed.");
        }

        private static TheoryData<string, object, string> SaveTestData() => new()
        {
            {
                "num",
                -233,
                "-233"
            },
            {
                "num.real",
                1024D,
                "1024"
            },
            {
                "text.lipsum",
                "Nulla accumsan ultrices sem",
                "\"Nulla accumsan ultrices sem\""
            },
            {
                "point",
                new Point(12, 5),
                "{\"x\": 12, \"y\": 5}"
            },
            {
                "array.real",
                new[] { -1.5, Math.PI, double.PositiveInfinity },
                $"[ -1.5, {Math.PI}, \"{double.PositiveInfinity}\" ]"
            },
            {
                "text.hamlet",
                "To be or not to be",
                "\"To be or not to be\""
            },
        };

        [Theory]
        [MemberData(nameof(SaveTestData))]
        public void Save_AfterModified<T>(string key, T value, string expectedJsonText)
        {
            using var file = CreateTempFile();
            JsonFileSettingsSource source = new(file.File.Name)
            {
                FileProvider = new PhysicalFileProvider(file.File.DirectoryName),
            };
            JsonFileSettingsProvider provider = new(source);
            SettingsKeyInfo keyInfo = new(key, typeof(T));
            provider.TryLoad(keyInfo);
            Assert.True(provider.Set(key, value), "Set failed.");
            Assert.True(provider.Save(), "Save failed");

            JsonDocumentOptions jsonDocumentOptions = new()
            {
                AllowTrailingCommas = true,
            };
            using var fs = file.File.OpenRead();
            using var fulldoc = JsonDocument.Parse(fs, jsonDocumentOptions);
            using var expecteddoc = JsonDocument.Parse(expectedJsonText, jsonDocumentOptions);
            var expectedJson = expecteddoc.RootElement;
            Assert.Equal(expectedJson, fulldoc.RootElement.GetProperty(key), new JsonElementEqualityComparer(StringComparer.OrdinalIgnoreCase));
        }

        private static TemporaryFile CreateTempFile()
        {
            TemporaryFile file = new(BaseDirectory);
            using var writer = file.File.AppendText();
            writer.Write(File.ReadAllText(Path.Combine(BaseDirectory, PhysicalFileName)));
            return file;
        }
    }
}
