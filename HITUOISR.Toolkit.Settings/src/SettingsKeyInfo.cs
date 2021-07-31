using HITUOISR.Toolkit.Common;
using System;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// <seealso cref="ISettingsKeyInfo"/> 的实现。
    /// </summary>
    public class SettingsKeyInfo : ISettingsKeyInfo
    {
        /// <summary>
        /// 初始化设置键信息。
        /// </summary>
        /// <param name="path">设置键路径。</param>
        /// <param name="type">设置键类型。</param>
        public SettingsKeyInfo(string path, Type type)
        {
            Path = path;
            Type = type;
            DefaultValue = type.GetLanguageDefault();
        }

        /// <inheritdoc/>
        public string Path { get; init; }

        /// <inheritdoc/>
        public Type Type { get; set; }

        /// <inheritdoc/>
        public bool IsReadOnly { get; set; }

        /// <inheritdoc/>
        public object? DefaultValue { get; set; }

        /// <inheritdoc/>
        public override string ToString() => Path;
    }
}
