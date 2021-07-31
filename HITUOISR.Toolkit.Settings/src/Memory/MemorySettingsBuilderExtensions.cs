using System.Collections.Generic;

namespace HITUOISR.Toolkit.Settings.Memory
{
    /// <summary>
    /// 用于 <seealso cref="MemorySettingsProvider"/> 的 <seealso cref="ISettingsBuilder"/> 扩展方法。
    /// </summary>
    public static class MemorySettingsBuilderExtensions
    {
        /// <summary>
        /// 添加内存中的设置数据。
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="data">要添加的设置数据。</param>
        /// <returns></returns>
        public static ISettingsBuilder AddInMemoryData(this ISettingsBuilder builder, IDictionary<string, object?> data)
        {
            builder.Sources.Add(new MemorySettingsSource(data));
            return builder;
        }
    }
}
