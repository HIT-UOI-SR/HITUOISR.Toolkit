using System;
using System.Reflection;
using Xunit;

namespace HITUOISR.Toolkit.Common.Tests
{
    public class TypeExtensionsTests
    {
        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(void*))] // 指针
        [InlineData(typeof(uint*))]
        [InlineData(typeof(double?))] // Nullable<T>
        [InlineData(typeof(int[]))] // 数组
        [InlineData(typeof(float[,]))]
        [InlineData(typeof(Type))] // 抽象类
        [InlineData(typeof(IDisposable))] // 接口
        public void IsNullable_True(Type type) => Assert.True(type.IsNullable(), $"{type} should be nullable.");

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(TypeCode))] // 枚举
        [InlineData(typeof(TimeSpan))] // 结构体
        public void IsNullable_False(Type type) => Assert.False(type.IsNullable(), $"{type} should not be nullable.");

        [Theory]
        [InlineData(typeof(bool))]
        [InlineData(typeof(byte))]
        [InlineData(typeof(char))]
        [InlineData(typeof(int))]
        [InlineData(typeof(uint))]
        [InlineData(typeof(float))]
        [InlineData(typeof(double))]
        [InlineData(typeof(decimal))]
        [InlineData(typeof(TypeCode))]
        [InlineData(typeof(TimeSpan))]
        [InlineData(typeof(bool?))]
        [InlineData(typeof(int?))]
        [InlineData(typeof(double?))]
        [InlineData(typeof(TypeCode?))]
        [InlineData(typeof(TimeSpan?))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int[]))]
        [InlineData(typeof(Type[,]))]
        public void GetLanguageDefault_AsExpected(Type type)
        {
            var @default = typeof(TypeExtensionsTests)
                .GetMethod(nameof(Default), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(type)
                .Invoke(null, null);
            Assert.Equal(@default, type.GetLanguageDefault());
        }

        private static T? Default<T>() => default;
    }
}
