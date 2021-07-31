using HITUOISR.Toolkit.Settings;
using HITUOISR.Toolkit.Settings.Model;
using HITUOISR.Toolkit.SimpleHost.PluginTools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace HITUOISR.Toolkit.SimpleHost
{
    /// <summary>
    /// 主机构造器。
    /// </summary>
    public class HostBuilder : IHostBuilder
    {
        private readonly List<Action<IServiceCollection>> _configureServicesActions = new();
        private readonly List<Action<ISettingsBuilder>> _configureSettingsActions = new();
        private readonly List<Action<IPluginBuilder>> _configurePluginsActions = new();

        /// <summary>
        /// 初始化。
        /// </summary>
        public HostBuilder() { }

        /// <inheritdoc/>
        public IHost Build()
        {
            IServiceCollection services = SetupServices();
            BuildPlugins(services);
            BuildSettings(services);
            var sp = BuildServiceProvider(services);
            return sp.GetRequiredService<IHost>();
        }

        /// <inheritdoc/>
        public IHostBuilder ConfigureServices(Action<IServiceCollection> configureAction)
        {
            _configureServicesActions.Add(configureAction);
            return this;
        }

        /// <inheritdoc/>
        public IHostBuilder ConfigureSettings(Action<ISettingsBuilder> configureAction)
        {
            _configureSettingsActions.Add(configureAction);
            return this;
        }

        /// <inheritdoc/>
        public IHostBuilder ConfigurePlugins(Action<IPluginBuilder> configureAction)
        {
            _configurePluginsActions.Add(configureAction);
            return this;
        }

        private IServiceCollection SetupServices()
        {
            ServiceCollection services = new();
            services.AddLogging();
            return services;
        }

        private void BuildPlugins(IServiceCollection services)
        {
            PluginBuilder builder = new();
            foreach (var action in _configurePluginsActions)
            {
                action(builder);
            }
            services.AddSingleton(builder.Build(this));
        }

        private void BuildSettings(IServiceCollection services)
        {
            SettingsBuilder builder = new();
            builder.SetServices(services);
            foreach (var action in _configureSettingsActions)
            {
                action(builder);
            }
            ISettings settings = builder.Build();
            services.AddSingleton(settings);
            services.AddSingleton(typeof(ISettingsModel<>), typeof(SettingsModel<>));
            services.AddScoped(typeof(ISettingsModelSnapshot<>), typeof(ReadOnlySettingsModel<>));
        }

        private IServiceProvider BuildServiceProvider(IServiceCollection services)
        {
            services.AddSingleton<IHost>(sp => new Host(
                sp,
                sp.GetRequiredService<ILogger<Host>>()));
            foreach (var action in _configureServicesActions)
            {
                action(services);
            }
            var sp = services.BuildServiceProvider();
            _ = sp.GetService<ISettings>();
            return sp;
        }
    }
}
