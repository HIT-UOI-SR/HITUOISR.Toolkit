using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace HITUOISR.Toolkit.Settings.Model.Tests
{
    public class DummySettingsModelTests
    {
        private class SampleModel : ObservableObject, IEquatable<SampleModel>
        {
            public double Value
            {
                get => _Value;
                set => SetProperty(ref _Value, value);
            }
            private double _Value = double.NaN;

            public bool Equals([AllowNull] SampleModel other) => other != null && Value.CompareTo(other.Value) == 0;
            public override bool Equals(object? obj) => Equals(obj as SampleModel);
            public override int GetHashCode() => HashCode.Combine(Value);
        }

        private static TheoryData<INotifyPropertyChanged> ModelsData() => new()
        {
            new SampleModel(),
            new SampleModel { Value = double.Epsilon },
        };

        [Fact()]
        public void Ctor_New_AsExpected()
        {
            DummySettingsModel<SampleModel> settingsModel = new();
            Assert.Equal(new SampleModel(), settingsModel.Value);
        }

        [Theory]
        [MemberData(nameof(ModelsData))]
        public void Ctor_SameValue_AsExpected<TModel>(TModel model) where TModel : class, INotifyPropertyChanged, new()
        {
            DummySettingsModel<TModel> settingsModel = new(model);
            Assert.Same(model, settingsModel.Value);
        }
    }
}
