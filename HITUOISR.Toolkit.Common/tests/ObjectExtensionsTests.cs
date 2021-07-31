using System;
using System.Collections.Generic;
using Xunit;

namespace HITUOISR.Toolkit.Common.Tests
{
    public class ObjectExtensionsTests
    {
        [Theory]
        [InlineData(1, typeof(int))]
        [InlineData(double.NaN, typeof(double?))] // T to Nullable<T>
        [InlineData("lorem ipsum", typeof(string))]
        [InlineData(TypeCode.Byte, typeof(TypeCode))]
        [InlineData('A', typeof(ValueType))] // to 基类
        [InlineData(TypeCode.Empty, typeof(Enum))]
        [InlineData("lorem ipsum", typeof(IEnumerable<char>))] // to 接口
        [InlineData(null, typeof(string))]
        [InlineData(null, typeof(float[]))] // null to 数组
        [InlineData(null, typeof(int?))] // null to Nullable<T>
        [InlineData(null, typeof(void*))] // null to 指针
        [InlineData(null, typeof(Type))] // null to 抽象类
        [InlineData(null, typeof(IDisposable))] // null to 接口
        public void IsAssignableToType_True(object? obj, Type type) => Assert.True(obj.IsAssignableToType(type), $"{obj} should be assignable to {type}.");

        [Theory]
        [InlineData(-1, typeof(uint))]
        [InlineData(0, typeof(double))] // 隐式转换不行
        [InlineData('?', typeof(string))]
        [InlineData("lorem ipsum", typeof(ReadOnlySpan<char>))] // 隐式转换不行
        [InlineData(null, typeof(byte))]
        public void IsAssignableToType_False(object? obj, Type type) => Assert.False(obj.IsAssignableToType(type), $"{obj} should not be assignable to {type}.");

        [Theory]
        [InlineData(1)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(TypeCode.Decimal)]
        [InlineData("lorem ipsum")]
        public void RestoreTo_SameType_AsExpected<T>(T expected)
        {
            object? obj = expected;
            Assert.Equal(expected, obj.RestoreTo<T>());
        }

        [Fact]
        public void RestoreTo_Nullable_AsExpected()
        {
            object? obj = null;
            var v = obj.RestoreTo<int?>();
            Assert.Null(v);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(TypeCode.Decimal)]
        public void RestoreTo_BaseType_AsExpected<T>(T expected) where T : struct
        {
            object? obj = expected;
            Assert.Equal(expected, obj.RestoreTo<ValueType>());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(TypeCode.Decimal)]
        [InlineData("lorem ipsum")]
        public void RestoreTo_Interface_AsExpected<T>(T expected) where T : IComparable
        {
            object? obj = expected;
            Assert.Equal(expected, obj.RestoreTo<IComparable>());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(TypeCode.Decimal)]
        [InlineData("lorem ipsum")]
        public void RestoreTo_DifferentType_ShouldThrow<T>(T expected)
        {
            object? obj = expected;
            Assert.Throws<InvalidCastException>(() => obj.RestoreTo<Delegate>());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(TypeCode.Decimal)]
        [InlineData("lorem ipsum")]
        public void RestoreToOrElse_SameType_AsExpected<T>(T expected)
        {
            object? obj = expected;
            Assert.Equal(expected, obj.RestoreToOrElse(() => default(T)));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(TypeCode.Decimal)]
        public void RestoreToOrElse_BaseType_AsExpected<T>(T expected) where T : struct
        {
            object? obj = expected;
            Assert.Equal(expected, obj.RestoreToOrElse<ValueType>(() => default(T)));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(TypeCode.Decimal)]
        [InlineData("lorem ipsum")]
        public void RestoreToOrElse_Interface_AsExpected<T>(T expected) where T : IComparable
        {
            object? obj = expected;
            Assert.Equal(expected, obj.RestoreToOrElse<IComparable?>(() => default(T)));
        }

        [Theory]
        [InlineData(1, double.NaN)]
        [InlineData(double.PositiveInfinity, "")]
        [InlineData(TypeCode.Decimal, ConsoleColor.Black)]
        [InlineData("lorem ipsum", 0)]
        public void RestoreToOrElse_DifferentType_Fallback<TDef>(object? origin, TDef def) => Assert.Equal(def, origin.RestoreToOrElse(() => def));
    }
}
