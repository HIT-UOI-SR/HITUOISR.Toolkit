using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// 设置接口。
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// 获取具有指定键的设置子节。
        /// </summary>
        /// <param name="key">子节的键。</param>
        /// <returns>对应的子节。</returns>
        ISettingsSection GetSection(string key);

        /// <summary>
        /// 获取直接后代设置子节。
        /// </summary>
        /// <returns>子节列表。</returns>
        IEnumerable<ISettingsSection> GetChildren();

        /// <summary>
        /// 为设置键指定一个值。
        /// </summary>
        /// <typeparam name="T">指定给键的对象的类型。</typeparam>
        /// <param name="key">待设置的键。</param>
        /// <param name="value">指派给键的值。</param>
        /// <exception cref="MissingSettingsProviderException">缺少可用的提供器。</exception>
        /// <exception cref="SettingsKeyNotFoundException">找不到对应的设置键。</exception>
        /// <exception cref="SettingsReadOnlyExcepetion">设置键只读。</exception>
        /// <exception cref="SettingsTypeMismatchException">设置类型不匹配。</exception>
        void SetValue<T>(string key, T value);

        /// <summary>
        /// 获取指定键对应的设置值。
        /// </summary>
        /// <typeparam name="T">获取的对象的类型。</typeparam>
        /// <param name="key">关联与请求获取的对象的键。</param>
        /// <returns>获取的设置值。</returns>
        /// <exception cref="SettingsKeyNotFoundException">找不到对应的设置键。</exception>
        /// <exception cref="SettingsTypeMismatchException">设置类型不匹配。</exception>
        [Pure]
        T GetValue<T>(string key);

        /// <summary>
        /// 获取指定键对应的设置值。
        /// </summary>
        /// <typeparam name="T">获取的对象的类型。</typeparam>
        /// <param name="key">关联与请求获取的对象的键。</param>
        /// <param name="defaultGetter">获取默认值的委托。</param>
        /// <returns>获取的设置值。</returns>
        [Pure]
        T GetValueOrElse<T>(string key, Func<T> defaultGetter);
    }
}
