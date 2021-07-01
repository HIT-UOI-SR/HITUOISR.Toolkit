using System;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// 指定要绑定到设置中的键。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false)]
    public class SettingsKeyAttribute : Attribute
    {
        /// <summary>
        /// 绑定的键路径（相对路径）。
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="key">绑定的键路径。</param>
        public SettingsKeyAttribute(string key) => Key = key;

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="keySegments">绑定的键路径节。</param>
        public SettingsKeyAttribute(params string[] keySegments) => Key = SettingsPath.Combine(keySegments);
    }
}
