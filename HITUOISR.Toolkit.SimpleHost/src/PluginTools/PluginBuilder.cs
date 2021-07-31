using HITUOISR.Toolkit.PluginBase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

namespace HITUOISR.Toolkit.SimpleHost.PluginTools
{
    /// <summary>
    /// 插件构造器。
    /// </summary>
    internal class PluginBuilder : IPluginBuilder
    {
        private readonly List<string> searchPaths = new();
        private readonly List<string> searchPatterns = new();
        private readonly List<IPlugin> extraPlugins = new();

        /// <inheritdoc/>
        public IPluginBuilder AddPlugin(IPlugin plugin)
        {
            extraPlugins.Add(plugin);
            return this;
        }

        /// <inheritdoc/>
        public IPluginBuilder AddSearchPath(string path)
        {
            searchPaths.Add(path);
            return this;
        }

        /// <inheritdoc/>
        public IPluginBuilder AddSearchPattern(string pattern)
        {
            searchPatterns.Add(pattern);
            return this;
        }

        /// <inheritdoc/>
        public Func<IServiceProvider, IPluginManager> Build(IHostBuilder builder)
        {
            Queue<Action<ILogger>> logActions = new();
            IServiceProvider services = LoadPlugins(logActions).BuildServiceProvider();
            try
            {
                foreach (var plugin in services.GetServices<IPlugin>().Concat(extraPlugins))
                {
                    try
                    {
                        builder.ConfigureServices(plugin.ConfigureServices);
                        builder.ConfigureSettings(plugin.ConfigureSettings);
                    }
                    catch (Exception ex)
                    {
                        logActions.Enqueue(logger => logger.LogError(ex, "无法配置插件 {name}。", plugin.Name));
                    }
                }
            }
            catch (Exception ex)
            {
                logActions.Enqueue(logger => logger.LogError(ex, "无法解决插件依赖。"));
            }
            return sp =>
            new PluginManager(
                services,
                sp.GetRequiredService<ILogger<PluginManager>>())
            .RestoreLazyLogger(logActions);
        }

        private IServiceCollection LoadPlugins(Queue<Action<ILogger>> logActions)
        {
            ServiceCollection services = new();
            foreach (var file in
                from path in searchPaths
                where Directory.Exists(path)
                from patt in searchPatterns
                from file in Directory.EnumerateFiles(path, patt, SearchOption.AllDirectories)
                select file)
            {
                try
                {
                    var type = GetPlugin(LoadPluginAssembly(file));
                    if (type == null)
                    {
                        logActions.Enqueue(logger => logger.LogError("无法从 {file} 中获取插件接口。", file));
                        continue;
                    }
                    services.AddSingleton(typeof(IPlugin), type);
                }
                catch (BadImageFormatException ex)
                {
                    logActions.Enqueue(logger => logger.LogError(ex, "{file} 不是有效的 .NET 程序集，无法加载。", file));
                    continue;
                }
                catch (FileNotFoundException ex)
                {
                    logActions.Enqueue(logger => logger.LogDebug(ex, "{file} 未找到，无法加载。", file));
                    continue;
                }
                catch (FileLoadException ex)
                {
                    logActions.Enqueue(logger => logger.LogDebug(ex, "{file} 已经加载，无法再次加载。", file));
                    continue;
                }
                catch (Exception ex)
                {
                    logActions.Enqueue(logger => logger.LogError(ex, "无法加载插件 {file}", file));
                    continue;
                }
            }
            return services;
        }

        private static Assembly LoadPluginAssembly(string path)
        {
            PluginLoadContext loadContext = new(path);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(path)));
        }

        private static Type? GetPlugin(Assembly assembly)
        {
            return assembly
                .GetExportedTypes()
                .FirstOrDefault(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        }
    }
}
