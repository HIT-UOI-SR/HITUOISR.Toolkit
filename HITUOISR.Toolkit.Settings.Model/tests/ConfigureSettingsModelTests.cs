using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using Xunit;

namespace HITUOISR.Toolkit.Settings.Model.Tests
{
    public class ConfigureSettingsModelTests
    {
        private class SampleModel : ObservableObject
        {
            public int Value
            {
                get => _Value;
                set => SetProperty(ref _Value, value);
            }
            private int _Value;
        }

        private class FakeSettings : ISettings
        {
            public IEnumerable<ISettingsSection> GetChildren() => Array.Empty<ISettingsSection>();
            public ISettingsSection GetSection(string key) => throw new NotImplementedException();
            public T GetValue<T>(string key) => throw new NotImplementedException();
            public T GetValueOrElse<T>(string key, Func<T> defaultGetter) => defaultGetter();
            public void SetValue<T>(string key, T value) => throw new NotImplementedException();
        }

        [Fact()]
        public void Configure_InitOnly_AsExpecterd()
        {
            var inited = false;
            var tracked = false;
            void Initializer(SampleModel model, ISettings settings) => inited = true;
            void Tracker(ISettings settings, SampleModel model, string? prop) => tracked = true;
            ConfigureSettingsModel<SampleModel> configure = new(Initializer, Tracker);
            FakeSettings settings = new();
            SampleModel model = new();
            configure.Configure(settings, model, bidirection: false);
            Assert.True(inited, "Configure doesnot initialize");
            model.Value = 1;
            Assert.False(tracked, "Configure setup tracked on init-only mode.");
        }

        [Fact()]
        public void Configure_Bidirection_AsExpecterd()
        {
            var inited = false;
            string? propName = null;
            void Initializer(SampleModel model, ISettings settings) => inited = true;
            void Tracker(ISettings settings, SampleModel model, string? prop) => propName = prop;
            ConfigureSettingsModel<SampleModel> configure = new(Initializer, Tracker);
            FakeSettings settings = new();
            SampleModel model = new();
            configure.Configure(settings, model, bidirection: true);
            Assert.True(inited, "Configure doesnot initialize");
            model.Value = 1;
            Assert.Equal(nameof(SampleModel.Value), propName);
        }
    }
}
