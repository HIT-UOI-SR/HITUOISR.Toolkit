using HITUOISR.Toolkit.PluginBase;
using System;

namespace HITUOISR.Toolkit.SimpleHost.PluginTools
{
    /// <summary>
    /// 插件构造器。
    /// </summary>
    public interface IPluginBuilder
    {
        /// <summary>
        /// 添加插件搜索模式。
        /// </summary>
        /// <param name="pattern">插件搜索模式。</param>
        IPluginBuilder AddSearchPattern(string pattern);

        /// <summary>
        /// 添加插件搜索路径。
        /// </summary>
        /// <param name="path">插件搜索路径。</param>
        IPluginBuilder AddSearchPath(string path);

        /// <summary>
        /// 添加插件。
        /// </summary>
        /// <param name="plugin">插件。</param>
        IPluginBuilder AddPlugin(IPlugin plugin);

        /// <summary>
        /// 构造插件和插件管理器。
        /// </summary>
        /// <param name="builder">关联的主机构造器。</param>
        /// <returns>插件管理器工厂方法。</returns>
        Func<IServiceProvider, IPluginManager> Build(IHostBuilder builder);
    }
}
