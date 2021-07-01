namespace HITUOISR.Toolkit.Settings.Json
{
    /// <summary>
    /// <seealso cref="ISettingsBuilder"/> 扩展方法。
    /// </summary>
    public static class JsonSettingsBuilderExtensions
    {
        /// <summary>
        /// 向设置构造器添加JSON文件。
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="file">要添加的JSON文件。</param>
        /// <param name="autoCreate">该文件在缺失时是否会自动创建。</param>
        /// <param name="isReadonly">该文件是否只读。</param>
        /// <returns></returns>
        public static ISettingsBuilder AddJsonFile(this ISettingsBuilder builder, string file, bool autoCreate = true, bool isReadonly = false)
        {
            JsonFileSettingsSource source = new(file)
            {
                AutoCreate = autoCreate,
                IsReadOnly = isReadonly,
            };
            builder.Sources.Add(source);
            return builder;
        }
    }
}
