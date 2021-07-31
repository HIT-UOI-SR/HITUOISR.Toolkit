using Xunit;
using System;
using System.Diagnostics.Contracts;

namespace HITUOISR.Toolkit.Settings.Tests
{
    public class SettingsDefaultValueMemberAttributeTests
    {
        public static int[] TestOtherClassPropertyDefault => new[] { 1, 5, 5, 1 };

        private class TestClass
        {
            public static int[] TestPropertyDefault => new[] { 1, 0, 2, 4 };

            public static readonly int[] TestFieldDefault = new[] { -2, 5, 6 };

            public static int[] TestMethodDefault() => new[] { 2, 3, 3, 3 };

            [SettingsDefaultValueMember(nameof(TestPropertyDefault))]
            public int[] TestProperty { get; set; } = TestPropertyDefault;

            [SettingsDefaultValueMember(nameof(TestFieldDefault))]
            public int[] TestField { get; set; } = TestFieldDefault;

            [SettingsDefaultValueMember(nameof(TestMethodDefault))]
            public int[] TestMethod { get; set; } = TestMethodDefault();

            [SettingsDefaultValueMember(nameof(TestOtherClassPropertyDefault), MemberDeclaringType = typeof(SettingsDefaultValueMemberAttributeTests))]
            public int[] TestOtherClassProperty { get; set; } = TestOtherClassPropertyDefault;
        }

        private static TheoryData<string, object> DefaultValueData() => new()
        {
            { nameof(TestClass.TestProperty), TestClass.TestPropertyDefault },
            { nameof(TestClass.TestField), TestClass.TestFieldDefault },
            { nameof(TestClass.TestMethod), TestClass.TestMethodDefault() },
            { nameof(TestClass.TestOtherClassProperty), TestOtherClassPropertyDefault },
        };

        [Theory]
        [MemberData(nameof(DefaultValueData))]
        public void GetDefaultValue_AsExpected<T>(string propertyName, T expected)
        {
            var type = typeof(TestClass);
            var property = type.GetProperty(propertyName);
            Assert.NotNull(property);
            Contract.Assume(property != null);
            var attr = (SettingsDefaultValueMemberAttribute?)Attribute.GetCustomAttribute(property, typeof(SettingsDefaultValueMemberAttribute));
            Assert.NotNull(attr);
            Contract.Assume(attr != null);
            var actual = attr.GetDefaultValue(type);
            Assert.IsType<T>(actual);
            Contract.Assume(actual is T);
            Assert.Equal(expected, (T)actual);
        }
    }
}
