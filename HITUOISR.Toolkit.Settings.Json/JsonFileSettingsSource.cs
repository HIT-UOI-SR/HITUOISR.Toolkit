using HITUOISR.Toolkit.Settings.FileBase;

namespace HITUOISR.Toolkit.Settings.Json
{
    /// <summary>
    /// JSON文件设置源。
    /// </summary>
    public sealed class JsonFileSettingsSource : FileSettingsSource
    {
        /// <summary>
        /// 初始化JSON文件设置源。
        /// </summary>
        /// <param name="file">文件路径。</param>
        public JsonFileSettingsSource(string file) : base(file) { }

        /// <inheritdoc/>
        public override ISettingsProvider Build(ISettingsBuilder builder)
        {
            EnsureDefaults(builder);
            return new JsonFileSettingsProvider(this);
        }
    }
}
