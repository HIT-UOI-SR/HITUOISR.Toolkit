using HITUOISR.Toolkit.Settings.Memory;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Xunit;

namespace HITUOISR.Toolkit.Settings.Model.Tests
{
    public class ModelKeyGeneratorTests
    {
        [SettingsKey("sample")]
        private class SampleModel : ObservableObject
        {
            [SettingsDefaultValueMember(nameof(DefaultData))]
            public float[] Data
            {
                get => _Data;
                set => SetProperty(ref _Data, value);
            }
            private float[] _Data = Array.Empty<float>();

            public static readonly float[] DefaultData = new float[10];

            public char Ident { get; init; } = ' ';

            [SettingsKey("num")]
            public int Number
            {
                get => _Number;
                set => SetProperty(ref _Number, value);
            }
            private int _Number = -1;

            [DefaultValue("lorem ipsum")]
            public string Text
            {
                get => _Text;
                set => SetProperty(ref _Text, value);
            }
            private string _Text = string.Empty;
        }

        private static Dictionary<string, object?> SampleSourceData => new(StringComparer.OrdinalIgnoreCase)
        {
            ["sample.data"] = new[] { 1F, 2F, 3F },
            ["sample.ident"] = '@',
            ["sample.num"] = 255,
        };

        [Fact()]
        public void GenerateModelInitializer_InitializeModel()
        {
            SampleModel model = new();
            var propkeys = ModelKeyGenerator.GetPropKeys<SampleModel>();
            ISettingsProvider provider = new MemorySettingsSource(SampleSourceData).Build(new SettingsBuilder());
            SettingsRoot settings = new(new[] { provider }, propkeys.Select(pk => pk.Item2));
            var initializer = ModelKeyGenerator.GenerateModelInitializer<SampleModel>(propkeys);
            initializer.Invoke(model, settings);

            Assert.Equal(settings.GetValue<float[]>("sample.data"), model.Data);
            Assert.Equal(settings.GetValue<char>("sample.ident"), model.Ident);
            Assert.Equal(settings.GetValue<int>("sample.num"), model.Number);
            Assert.Equal(settings.GetValue<string>("sample.text"), model.Text);
        }

        private static TheoryData<string, string, object> SampleModelTrackerTestData() => new()
        {
            {
                nameof(SampleModel.Data),
                "sample.data",
                Array.Empty<float>()
            },
            {
                nameof(SampleModel.Number),
                "sample.num",
                default(int)
            },
            {
                nameof(SampleModel.Text),
                "sample.text",
                string.Empty
            },
        };

        [Theory]
        [MemberData(nameof(SampleModelTrackerTestData))]
        public void GenerateModelSettingsTracker_UpdateSettings<T>(string propertyName, string key, T _)
        {
            Assert.IsType<T>(_); // 没啥用，避免xUnit对泛型类型占位参数警告

            SampleModel model = new();
            var propkeys = ModelKeyGenerator.GetPropKeys<SampleModel>();
            ISettingsProvider provider = new MemorySettingsSource(SampleSourceData).Build(new SettingsBuilder());
            SettingsRoot settings = new(new[] { provider }, propkeys.Select(pk => pk.Item2));
            var tracker = ModelKeyGenerator.GenerateModelSettingsTracker<SampleModel>(propkeys);
            tracker.Invoke(settings, model, propertyName);

            var reflectValue = typeof(SampleModel).GetProperty(propertyName)!.GetValue(model);
            Assert.IsType<T>(reflectValue);
            Contract.Assume(reflectValue is T);
            Assert.Equal((T)reflectValue, settings.GetValue<T>(key));
        }

        [Fact()]
        public void GetPropKeys_GenericType_SampleModel_WithoutRoot()
        {
            var propkeys = ModelKeyGenerator.GetPropKeys<SampleModel>();
            var keyroot = "sample";
            Assert_GetPropKeys_SampleModel(keyroot, propkeys);
        }

        [Fact()]
        public void GetPropKeys_GenericType_SampleModel_WithRoot()
        {
            var keyroot = "root";
            var propkeys = ModelKeyGenerator.GetPropKeys<SampleModel>(keyroot);
            Assert_GetPropKeys_SampleModel(keyroot, propkeys);
        }

        [Fact()]
        public void GetPropKeys_RuntimeType_SampleModel_WithoutRoot()
        {
            var propkeys = ModelKeyGenerator.GetPropKeys(typeof(SampleModel));
            var keyroot = "sample";
            Assert_GetPropKeys_SampleModel(keyroot, propkeys);
        }

        [Fact()]
        public void GetPropKeys_RuntimeType_SampleModel_WithRoot()
        {
            var keyroot = "root";
            var propkeys = ModelKeyGenerator.GetPropKeys(typeof(SampleModel), keyroot);
            Assert_GetPropKeys_SampleModel(keyroot, propkeys);
        }

        internal static void Assert_GetPropKeys_SampleModel(string keyroot, IEnumerable<(PropertyInfo, ISettingsKeyInfo)> propkeys) =>
            Assert.Collection(
                propkeys,
                pk =>
                {
                    var (prop, key) = pk;
                    Assert.Equal(nameof(SampleModel.Data), prop.Name);
                    Assert.Equal(SettingsPath.Combine(keyroot, "Data"), key.Path, StringComparer.OrdinalIgnoreCase);
                    Assert.Equal(prop.PropertyType, key.Type);
                    Assert.False(key.IsReadOnly);
                    Assert.IsType<float[]>(key.DefaultValue);
                    Contract.Assume(key.DefaultValue is float[]);
                    Assert.Equal(SampleModel.DefaultData, (float[])key.DefaultValue);
                },
                pk =>
                {
                    var (prop, key) = pk;
                    Assert.Equal(nameof(SampleModel.Ident), prop.Name);
                    Assert.Equal(SettingsPath.Combine(keyroot, "Ident"), key.Path, StringComparer.OrdinalIgnoreCase);
                    Assert.Equal(prop.PropertyType, key.Type);
                    Assert.True(key.IsReadOnly);
                    Assert.IsType<char>(key.DefaultValue);
                    Contract.Assume(key.DefaultValue is char);
                    Assert.Equal(default, (char)key.DefaultValue);
                },
                pk =>
                {
                    var (prop, key) = pk;
                    Assert.Equal(nameof(SampleModel.Number), prop.Name);
                    Assert.Equal(SettingsPath.Combine(keyroot, "num"), key.Path, StringComparer.OrdinalIgnoreCase);
                    Assert.Equal(prop.PropertyType, key.Type);
                    Assert.False(key.IsReadOnly);
                    Assert.IsType<int>(key.DefaultValue);
                    Contract.Assume(key.DefaultValue is int);
                    Assert.Equal(default, (int)key.DefaultValue);
                },
                pk =>
                {
                    var (prop, key) = pk;
                    Assert.Equal(nameof(SampleModel.Text), prop.Name);
                    Assert.Equal(SettingsPath.Combine(keyroot, "Text"), key.Path, StringComparer.OrdinalIgnoreCase);
                    Assert.Equal(prop.PropertyType, key.Type);
                    Assert.False(key.IsReadOnly);
                    Assert.IsType<string>(key.DefaultValue);
                    Contract.Assume(key.DefaultValue is string);
                    Assert.Equal("lorem ipsum", (string)key.DefaultValue);
                });
    }
}
