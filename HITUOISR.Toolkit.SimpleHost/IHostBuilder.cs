using HITUOISR.Toolkit.Settings;
using HITUOISR.Toolkit.SimpleHost.PluginTools;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HITUOISR.Toolkit.SimpleHost
{
    /// <summary>
    /// 主机构造器接口。
    /// </summary>
    public interface IHostBuilder
    {
        /// <summary>
        /// 构造主机。该函数只能调用一次。
        /// </summary>
        /// <returns>初始化了的主机对象。</returns>
        IHost Build();

        /// <summary>
        /// 配置服务。
        /// </summary>
        /// <param name="configureAction">配置的操作。</param>
        /// <returns>该对象自身。</returns>
        IHostBuilder ConfigureServices(Action<IServiceCollection> configureAction);

        /// <summary>
        /// 配置设置。
        /// </summary>
        /// <param name="configureAction">配置的操作。</param>
        /// <returns>该对象自身。</returns>
        IHostBuilder ConfigureSettings(Action<ISettingsBuilder> configureAction);

        /// <summary>
        /// 配置插件。
        /// </summary>
        /// <param name="configureAction">配置的操作。</param>
        /// <returns>该对象自身。</returns>
        IHostBuilder ConfigurePlugins(Action<IPluginBuilder> configureAction);
    }
}
