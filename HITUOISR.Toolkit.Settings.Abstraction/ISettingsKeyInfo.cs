using System;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// 表示设置键的信息。
    /// </summary>
    public interface ISettingsKeyInfo
    {
        /// <summary>
        /// 获取该键的完整路径。
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// 获取该键的设置类型。
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// 表示该设置键是否只读。
        /// </summary>
        public bool IsReadOnly { get; }

        /// <summary>
        /// 获取该键对应的默认值。
        /// </summary>
        public object? DefaultValue { get; }
    }
}
