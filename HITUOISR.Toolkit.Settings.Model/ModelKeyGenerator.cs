using HITUOISR.Toolkit.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace HITUOISR.Toolkit.Settings.Model
{
    internal static class ModelKeyGenerator
    {
        /// <summary>
        /// 生成设置模型属性初始化器。
        /// </summary>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="propkeys">属性-设置键序列。</param>
        /// <returns>用于初始化模型属性的委托函数（参数：模型，设置）。</returns>
        public static Action<TModel, ISettings> GenerateModelInitializer<TModel>(IEnumerable<(PropertyInfo prop, ISettingsKeyInfo key)> propkeys) where TModel : class
        {
            var model = Expression.Parameter(typeof(TModel), "model");
            var settings = Expression.Parameter(typeof(ISettings), "settings");
            var body = Expression.Block(
                from pk in propkeys
                select Expression.Assign(
                    Expression.Property(model, pk.prop),
                    Expression.Call(
                        settings,
                        typeof(ISettings).GetMethod(nameof(ISettings.GetValue))!.MakeGenericMethod(pk.key.Type),
                        Expression.Constant(pk.key.Path)
                        )
                    )
                );
            var lambda = Expression.Lambda<Action<TModel, ISettings>>(body, model, settings);
            return lambda.Compile();
        }

        /// <summary>
        /// 生成设置模型-设置跟踪器。
        /// </summary>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="propkeys">属性-设置键序列。</param>
        /// <returns>生成用于根据属性名更新设置的委托函数（参数：设置，模型，属性名）。</returns>
        public static Action<ISettings, TModel, string?> GenerateModelSettingsTracker<TModel>(IEnumerable<(PropertyInfo prop, ISettingsKeyInfo key)> propkeys) where TModel : class
        {
            var settings = Expression.Parameter(typeof(ISettings), "settings");
            var model = Expression.Parameter(typeof(TModel), "model");
            var propertyName = Expression.Parameter(typeof(string), "propertyName");
            var body = Expression.Switch(
                switchValue: propertyName,
                cases: (from pk in propkeys
                        where !pk.key.IsReadOnly // 只读设置不能更新
                        select Expression.SwitchCase(
                            testValues: Expression.Constant(pk.prop.Name),
                            body: Expression.Call(
                                instance: settings,
                                method: typeof(ISettings).GetMethod(nameof(ISettings.SetValue))!.MakeGenericMethod(pk.prop.PropertyType),
                                arg0: Expression.Constant(pk.key.Path),
                                arg1: Expression.Property(model, pk.prop)
                                )
                            )).ToArray(),
                defaultBody: Expression.Throw(
                    Expression.New(
                        constructor: typeof(ArgumentException).GetConstructor(new[] { typeof(string), typeof(string) })!,
                        Expression.Call(
                            method: typeof(string).GetMethod(nameof(string.Format), new[] { typeof(string), typeof(object) })!,
                            arg0: Expression.Constant("Unknown property name {0}."),
                            arg1: propertyName),
                        Expression.Constant(propertyName.Name)
                        )
                    )
                );
            var lambda = Expression.Lambda<Action<ISettings, TModel, string?>>(body, settings, model, propertyName);
            return lambda.Compile();
        }

        public static IEnumerable<(PropertyInfo, ISettingsKeyInfo)> GetPropKeys<T>() => GetPropKeys(typeof(T));

        public static IEnumerable<(PropertyInfo, ISettingsKeyInfo)> GetPropKeys<T>(string root) => GetPropKeys(typeof(T), root);

        public static IEnumerable<(PropertyInfo, ISettingsKeyInfo)> GetPropKeys(Type type) => GetPropKeys(type, GetKey(type));

        public static IEnumerable<(PropertyInfo, ISettingsKeyInfo)> GetPropKeys(Type type, string root) =>
            GetKeys(
                type.GetProperties(BindingFlags.Public | BindingFlags.Instance), // 只搜索公有的实例属性
                root);

        public static IEnumerable<(PropertyInfo, ISettingsKeyInfo)> GetKeys(PropertyInfo[] properties, string root)
        {
            foreach (var prop in properties)
            {
                if (!prop.CanRead || !prop.CanWrite)
                    continue;
                string key = GetKey(prop);
                string path = SettingsPath.Combine(root, key);
                bool readOnly = prop.IsInitOnly();
                object? defaultValue = GetDefaultValue(prop);
                yield return ValueTuple.Create(prop,
                    new SettingsKeyInfo(path, prop.PropertyType)
                    {
                        IsReadOnly = readOnly,
                        DefaultValue = defaultValue,
                    } as ISettingsKeyInfo);
            }
        }

        internal static string GetKey(MemberInfo info) =>
            Attribute.GetCustomAttribute(info, typeof(SettingsKeyAttribute)) is SettingsKeyAttribute settingsKeyAttr ?
            settingsKeyAttr.Key :
            info.Name;

        internal static object? GetDefaultValue(PropertyInfo property)
        {
            Type ptype = property.PropertyType;
            if (Attribute.GetCustomAttribute(property, typeof(SettingsDefaultValueMemberAttribute)) is SettingsDefaultValueMemberAttribute propDefaultValueMemberAttr)
            {
                return propDefaultValueMemberAttr.GetDefaultValue(property.DeclaringType!);
            }
            else if (Attribute.GetCustomAttribute(property, typeof(DefaultValueAttribute)) is DefaultValueAttribute propDefaultValueAttr)
            {
                return propDefaultValueAttr.Value;
            }
            else if (Attribute.GetCustomAttribute(ptype, typeof(DefaultValueAttribute)) is DefaultValueAttribute typeDefaultValueAttr)
            {
                return typeDefaultValueAttr.Value;
            }
            else
            {
                return ptype.GetLanguageDefault();
            }
        }
    }
}
