using System;
using System.Reflection;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// 指定获取设置默认值的成员。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingsDefaultValueMemberAttribute : Attribute
    {
        /// <summary>
        /// 用于获取默认值的成员名称。
        /// </summary>
        public string MemberName { get; }

        /// <summary>
        /// 成员所在的类型。
        /// </summary>
        public Type? MemberDeclaringType { get; set; }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="memberName">用于获取默认值的成员名称。</param>
        public SettingsDefaultValueMemberAttribute(string memberName) => MemberName = memberName;

        /// <summary>
        /// 获取默认值。
        /// </summary>
        /// <param name="fallbackType">当成员所在类型未指定时，使用的类型。</param>
        /// <returns>得到的默认值。</returns>
        public object? GetDefaultValue(Type fallbackType)
        {
            Type type = MemberDeclaringType ?? fallbackType;
            var accessor = GetPropertyAccessor(type) ?? GetFieldAccessor(type) ?? GetMethodAccessor(type);
            if (accessor is null)
            {
                throw new ArgumentException($"Could not find public static member (property, field, or method) named '{MemberName}' on {type.FullName}.");
            }
            return accessor();
        }

        private Func<object?>? GetFieldAccessor(Type? type)
        {
            FieldInfo? fieldInfo = null;
            for (var reflectionType = type; reflectionType != null; reflectionType = reflectionType.BaseType)
            {
                fieldInfo = reflectionType.GetRuntimeField(MemberName);
                if (fieldInfo != null)
                    break;
            }

            if (fieldInfo == null || !fieldInfo.IsStatic)
                return null;

            return () => fieldInfo.GetValue(null);
        }

        private Func<object?>? GetPropertyAccessor(Type? type)
        {
            PropertyInfo? propInfo = null;
            for (var reflectionType = type; reflectionType != null; reflectionType = reflectionType.BaseType)
            {
                propInfo = reflectionType.GetRuntimeProperty(MemberName);
                if (propInfo != null)
                    break;
            }

            if (propInfo == null || propInfo.GetMethod == null || !propInfo.GetMethod.IsStatic)
                return null;

            return () => propInfo.GetValue(null);
        }

        private Func<object?>? GetMethodAccessor(Type? type)
        {
            MethodInfo? methodInfo = null;
            for (var reflectionType = type; reflectionType != null; reflectionType = reflectionType.BaseType)
            {
                methodInfo = reflectionType.GetRuntimeMethod(MemberName, Array.Empty<Type>());
                if (methodInfo != null)
                    break;
            }

            if (methodInfo == null || !methodInfo.IsStatic)
                return null;

            return () => methodInfo.Invoke(null, null);
        }
    }
}
