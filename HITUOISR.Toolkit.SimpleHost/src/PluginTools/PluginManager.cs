using HITUOISR.Toolkit.Common;
using HITUOISR.Toolkit.PluginBase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HITUOISR.Toolkit.SimpleHost.PluginTools
{
    /// <summary>
    /// 插件管理器。
    /// </summary>
    internal sealed class PluginManager : IPluginManager
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<PluginManager> _logger;

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="logger"></param>
        public PluginManager(IServiceProvider services, ILogger<PluginManager> logger)
        {
            _services = services;
            _logger = logger;
        }

        /// <summary>
        /// 恢复待进行日志记录操作。
        /// </summary>
        /// <param name="logAtions"></param>
        /// <returns></returns>
        public PluginManager RestoreLazyLogger(Queue<Action<ILogger>> logAtions)
        {
            while (logAtions.TryDequeue(out var action))
            {
                action(_logger);
            }
            return this;
        }

        /// <inheritdoc/>
        public IEnumerable<IPlugin> Plugins => _services.GetServices<IPlugin>();

        /// <inheritdoc/>
        public void Dispose() => DisposeAsync().AsTask().GetAwaiter().GetResult();

        /// <inheritdoc/>
        public async ValueTask DisposeAsync() => await MiscUtils.DisposeAsync(_services);

        /// <inheritdoc/>
        public object? GetService(Type serviceType) => _services.GetService(serviceType);
    }
}
