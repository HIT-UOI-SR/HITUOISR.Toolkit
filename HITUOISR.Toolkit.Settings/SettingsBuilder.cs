using HITUOISR.Toolkit.Common;
using System;
using System.Collections.Generic;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// <seealso cref="ISettingsBuilder"/> 的实现。
    /// </summary>
    public sealed class SettingsBuilder : ISettingsBuilder
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        public SettingsBuilder() { }

        /// <inheritdoc/>
        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        /// <inheritdoc/>
        public IList<ISettingsSource> Sources { get; } = new List<ISettingsSource>();

        /// <inheritdoc/>
        public IEnumerable<ISettingsKeyInfo> SettingsKeys => settingsKeys;
        private readonly SortedSet<ISettingsKeyInfo> settingsKeys =
            new(ComparerUtils.FromSelector((ISettingsKeyInfo key) => key.Path, StringComparer.OrdinalIgnoreCase));

        /// <inheritdoc/>
        public ISettingsBuilder RegisterKey(ISettingsKeyInfo keyInfo)
        {
            if (!settingsKeys.Add(keyInfo))
            {
                throw new SettingsKeyExistsException($"设置键 {keyInfo.Path} 已存在，无法再次注册。");
            }
            return this;
        }

        /// <inheritdoc/>
        public ISettingsRoot Build()
        {
            List<ISettingsProvider> providers = new();
            foreach (var source in Sources)
            {
                providers.Add(source.Build(this));
            }
            return new SettingsRoot(providers, settingsKeys);
        }
    }
}
