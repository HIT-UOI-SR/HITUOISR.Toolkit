using Microsoft.Extensions.DependencyInjection;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// <seealso cref="ISettingsBuilder"/> 扩展方法。
    /// </summary>
    public static class SettingsBuilderExtensions
    {
        private static readonly string ServicesKey = "Services";

        /// <summary>
        /// 设置关联的服务集合。
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="services">关联的服务集合。</param>
        /// <returns>该构造器。</returns>
        public static ISettingsBuilder SetServices(this ISettingsBuilder builder, IServiceCollection services)
        {
            builder.Properties[ServicesKey] = services;
            return builder;
        }

        /// <summary>
        /// 获取关联的服务集合。
        /// </summary>
        /// <param name="builder"></param>
        /// <returns>关联的服务集合，或<see langword="null"/>。</returns>
        public static IServiceCollection? GetServices(this ISettingsBuilder builder)
        {
            return builder.Properties.TryGetValue(ServicesKey, out object? value) ? value as IServiceCollection : null;
        }

        /// <summary>
        /// 注册设置键。
        /// </summary>
        /// <typeparam name="T">对应设置值的类型。</typeparam>
        /// <param name="builder"></param>
        /// <param name="path">设置的键路径。</param>
        /// <param name="defaultValue">设置的默认值。</param>
        /// <param name="isReadonly">指示该设置项是否只读。</param>
        /// <returns>该构造器。</returns>
        public static ISettingsBuilder RegisterKey<T>(this ISettingsBuilder builder, string path, T defaultValue, bool isReadonly = false)
        {
            return builder.RegisterKey(new SettingsKeyInfo(path, typeof(T))
            {
                DefaultValue = defaultValue,
                IsReadOnly = isReadonly,
            });
        }
    }
}
