using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace HITUOISR.Toolkit.Settings.Model
{
    /// <summary>
    /// <seealso cref="ISettingsBuilder"/> 扩展方法。
    /// </summary>
    public static class SettingsBuilderExtensions
    {
        /// <summary>
        /// 添加设置模型。
        /// </summary>
        /// <typeparam name="TModel">设置模型类型。</typeparam>
        /// <param name="builder"></param>
        /// <returns>该构造器。</returns>
        public static ISettingsBuilder AddModel<TModel>(this ISettingsBuilder builder) where TModel : class, INotifyPropertyChanged, new()
        {
            var propkeys = ModelKeyGenerator.GetPropKeys<TModel>();
            foreach ((_, ISettingsKeyInfo key) in propkeys)
            {
                builder.RegisterKey(key);
            }
            IConfigureSettingsModel<TModel> configure = new ConfigureSettingsModel<TModel>(
                ModelKeyGenerator.GenerateModelInitializer<TModel>(propkeys),
                ModelKeyGenerator.GenerateModelSettingsTracker<TModel>(propkeys));
            builder.GetServices()?.AddSingleton(configure);
            return builder;
        }

        /// <summary>
        /// 添加设置模型。
        /// </summary>
        /// <typeparam name="TModel">设置模型类型。</typeparam>
        /// <param name="builder"></param>
        /// <param name="root">使用的根键路径。</param>
        /// <returns>该构造器。</returns>
        public static ISettingsBuilder AddModel<TModel>(this ISettingsBuilder builder, string root) where TModel : class, INotifyPropertyChanged, new()
        {
            var propkeys = ModelKeyGenerator.GetPropKeys<TModel>(root);
            foreach ((_, ISettingsKeyInfo key) in propkeys)
            {
                builder.RegisterKey(key);
            }
            IConfigureSettingsModel<TModel> configure = new ConfigureSettingsModel<TModel>(
                ModelKeyGenerator.GenerateModelInitializer<TModel>(propkeys),
                ModelKeyGenerator.GenerateModelSettingsTracker<TModel>(propkeys));
            builder.GetServices()?.AddSingleton(configure);
            return builder;
        }
    }
}
