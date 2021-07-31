using System;
using System.Diagnostics.Contracts;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// 表示设置的一个子节。
    /// </summary>
    public interface ISettingsSection : ISettings
    {
        /// <summary>
        /// 该子节在父级对应的键。
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// 该子节的完整路径。
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// 为该设置指定一个值。
        /// </summary>
        /// <typeparam name="T">指定的对象的类型。</typeparam>
        /// <param name="value">指派的值。</param>
        /// <exception cref="MissingSettingsProviderException">缺少可用的提供器。</exception>
        /// <exception cref="SettingsKeyNotFoundException">找不到对应的设置键。</exception>
        /// <exception cref="SettingsReadOnlyExcepetion">设置键只读。</exception>
        /// <exception cref="SettingsTypeMismatchException">设置类型不匹配。</exception>
        public void SetValue<T>(T value);

        /// <summary>
        /// 获取该设置对应的设置值。
        /// </summary>
        /// <typeparam name="T">获取的对象的类型。</typeparam>
        /// <returns>获取的设置值。</returns>
        /// <exception cref="SettingsKeyNotFoundException">找不到对应的设置键。</exception>
        /// <exception cref="SettingsTypeMismatchException">设置类型不匹配。</exception>
        [Pure]
        T GetValue<T>();

        /// <summary>
        /// 获取该设置对应的设置值。
        /// </summary>
        /// <typeparam name="T">获取的对象的类型。</typeparam>
        /// <param name="defaultGetter">获取默认值的委托。</param>
        /// <returns>获取的设置值。</returns>
        [Pure]
        T GetValueOrElse<T>(Func<T> defaultGetter);
    }
}
