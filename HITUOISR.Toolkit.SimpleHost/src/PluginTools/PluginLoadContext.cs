using System;
using System.Reflection;
using System.Runtime.Loader;

namespace HITUOISR.Toolkit.SimpleHost.PluginTools
{
    /// <summary>
    /// 插件加载上下文。
    /// </summary>
    /// <remarks>
    /// 参见：https://docs.microsoft.com/zh-cn/dotnet/core/tutorials/creating-app-with-plugin-support
    /// </remarks>
    public class PluginLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="pluginPath">插件路径。</param>
        public PluginLoadContext(string pluginPath) => _resolver = new(pluginPath);

        /// <inheritdoc/>
        protected override Assembly? Load(AssemblyName assemblyName)
        {
            string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }
            return null;
        }

        /// <inheritdoc/>
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string? libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }
            return IntPtr.Zero;
        }
    }
}
