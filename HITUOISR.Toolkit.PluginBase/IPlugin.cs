using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace HITUOISR.Toolkit.PluginBase
{
    /// <summary>
    /// 插件接口。
    /// </summary>
    public interface IPlugin : IDisposable
    {
        /// <summary>
        /// 配置插件服务。
        /// </summary>
        /// <param name="services">用于添加的服务集合。</param>
        void ConfigureServices(IServiceCollection services);

        /// <summary>
        /// 插件名称。
        /// </summary>
        /// <remarks>
        /// 默认实现为所在程序集名称。
        /// </remarks>
        string? Name => Assembly.GetExecutingAssembly().FullName;
    }
}
