using HITUOISR.Toolkit.Common;

namespace HITUOISR.Toolkit.Settings.Memory
{
    /// <summary>
    /// 基于内存数据的设置提供器。
    /// </summary>
    public class MemorySettingsProvider : SettingsProvider
    {
        private new MemorySettingsSource Source => (MemorySettingsSource)base.Source;

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="source"></param>
        internal MemorySettingsProvider(MemorySettingsSource source) : base(source) { }

        /// <inheritdoc/>
        public override bool TryLoad(ISettingsKeyInfo key, bool reloadRequest = false)
        {
            if (Source.Source.TryGetValue(key.Path, out object? value) && value.IsAssignableToType(key.Type))
            {
                Data[key.Path] = value;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public override bool Save()
        {
            foreach (var (k, v) in Data)
            {
                Source.Source[k] = v;
            }
            return true;
        }
    }
}
