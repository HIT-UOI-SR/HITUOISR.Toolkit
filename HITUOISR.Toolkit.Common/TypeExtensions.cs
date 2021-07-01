using System;

namespace HITUOISR.Toolkit.Common
{
    /// <summary>
    /// <seealso cref="Type"/> 扩展方法。
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 检查类型是否为可为空的类型。
        /// </summary>
        /// <param name="type"></param>
        /// <returns>
        /// 如果类型为引用类型或指针类型或<see cref="Nullable{T}"/>，则为<see langword="true"/>；
        /// 否则为<see langword="false"/>。
        /// </returns>
        public static bool IsNullable(this Type type) =>
            !type.IsValueType || type.IsPointer || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        /// <summary>
        /// 获取类型的默认值。
        /// </summary>
        /// <param name="type"></param>
        /// <returns>对于值类型，调用无参构造函数创建实例；对于引用类型，为<see langword="null"/>。</returns>
        public static object? GetLanguageDefault(this Type type) => type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}
