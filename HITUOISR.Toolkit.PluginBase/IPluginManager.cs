using System;
using System.Collections.Generic;

namespace HITUOISR.Toolkit.PluginBase
{
    /// <summary>
    /// 插件管理器接口。
    /// </summary>
    public interface IPluginManager : IServiceProvider, IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// 枚举插件。
        /// </summary>
        IEnumerable<IPlugin> Plugins { get; }
    }
}
