namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// 表示程序的设置源。
    /// </summary>
    public interface ISettingsSource
    {
        /// <summary>
        /// 指示该设置源是否只读。
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// 生成该源对应的提供器。
        /// </summary>
        /// <param name="builder"></param>
        /// <returns>对应的提供器。</returns>
        ISettingsProvider Build(ISettingsBuilder builder);
    }
}
