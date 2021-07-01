using System.Collections.Generic;

namespace HITUOISR.Toolkit.Settings.Memory
{
    /// <summary>
    /// 基于内存数据的设置源。
    /// </summary>
    public class MemorySettingsSource : ISettingsSource
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="data">设置数据。</param>
        public MemorySettingsSource(IDictionary<string, object?> data) =>
            Source = data;

        /// <summary>
        /// 数据源。
        /// </summary>
        public IDictionary<string, object?> Source { get; init; }

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public ISettingsProvider Build(ISettingsBuilder builder) => new MemorySettingsProvider(this);
    }
}
