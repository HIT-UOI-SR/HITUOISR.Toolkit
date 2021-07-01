namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// 设置的键值。
    /// </summary>
    internal interface ISettingsKeyValue
    {
        /// <summary>
        /// 关联的键信息。
        /// </summary>
        ISettingsKeyInfo KeyInfo { get; }

        /// <summary>
        /// 获取或设置该键值关联的设置值。
        /// </summary>
        object? Value { get; set; }

        /// <summary>
        /// 该键值关联的设置提供器。
        /// </summary>
        ISettingsProvider? Provider { get; set; }

        /// <summary>
        /// 指示该键是否已修改。
        /// </summary>
        bool IsModified { get; }

        /// <summary>
        /// 指示该键是否需要使用默认的设置值。
        /// </summary>
        bool UsingDefaultValue { get; }
    }
}
