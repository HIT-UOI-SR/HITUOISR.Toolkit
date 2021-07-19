using HITUOISR.Toolkit.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// <seealso cref="ISettingsRoot"/> 的实现。
    /// </summary>
    public class SettingsRoot : ISettingsRoot, IDisposable
    {
        private readonly IList<ISettingsProvider> _providers;
        private readonly IEnumerable<ISettingsKeyInfo> _keys;
        private readonly Dictionary<string, ISettingsKeyValue> _keyvaluesmap;
        private bool disposedValue = false;

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="providers">使用的设置提供服务。</param>
        /// <param name="settingsKeys">设置键信息。</param>
        public SettingsRoot(IList<ISettingsProvider> providers, IEnumerable<ISettingsKeyInfo> settingsKeys)
        {
            _providers = providers;
            _keys = settingsKeys;
            _keyvaluesmap = _keys.ToDictionary(k => k.Path, k => new SettingsKeyValue(k) as ISettingsKeyValue, StringComparer.OrdinalIgnoreCase);

            foreach (var key in _keys)
            {
                var kv = _keyvaluesmap[key.Path];
                foreach (var provider in _providers)
                {
                    if (provider.TryLoad(key))
                    {
                        kv.Provider = provider;
                    }
                }
                kv.Provider ??= key.IsReadOnly ? _providers.FirstOrDefault() : _providers.FirstOrDefault(p => !p.IsReadOnly);
            }
        }

        /// <summary>
        /// 终结器。
        /// </summary>
        ~SettingsRoot() => Dispose(disposing: false);

        /// <inheritdoc/>
        public IEnumerable<ISettingsProvider> Providers => _providers;

        /// <inheritdoc/>
        public IEnumerable<ISettingsKeyInfo> Keys => _keys;

        /// <inheritdoc/>
        public ISettingsSection GetSection(string key) => new SettingsSection(this, key);

        /// <inheritdoc/>
        public IEnumerable<ISettingsSection> GetChildren() => this.GetChildrenImpl(null);

        /// <inheritdoc/>
        public T GetValue<T>(string key)
        {
            try
            {
                return _keyvaluesmap[key].Value.RestoreTo<T>();
            }
            catch (KeyNotFoundException ex)
            {
                throw new SettingsKeyNotFoundException($"Unknown settings key {key}.", ex);
            }
            catch (InvalidCastException ex)
            {
                throw new SettingsTypeMismatchException($"{typeof(T)} is not an valid type for settings {key}.", ex);
            }
        }

        /// <inheritdoc/>
        public T GetValueOrElse<T>(string key, Func<T> defaultGetter)
        {
            if (_keyvaluesmap.TryGetValue(key, out ISettingsKeyValue? kv))
            {
                return kv.UsingDefaultValue ? defaultGetter() : kv.Value.RestoreToOrElse(defaultGetter);
            }
            else
            {
                return defaultGetter();
            }
        }

        /// <inheritdoc/>
        public void Reload()
        {
            var reloadRequest = true;
            foreach (var key in _keys)
            {
                foreach (var provider in _providers)
                {
                    if (provider.TryLoad(key, reloadRequest))
                    {
                        _keyvaluesmap[key.Path].Provider = provider;
                        reloadRequest = false;
                    }
                }
            }
        }

        /// <inheritdoc/>
        public void Save()
        {
            foreach (var provider in _providers)
            {
                provider.Save();
            }
        }

        /// <inheritdoc/>
        public void SetValue<T>(string key, T value)
        {
            try
            {
                var kv = _keyvaluesmap[key];
                if (kv.KeyInfo.IsReadOnly)
                {
                    throw new SettingsReadOnlyExcepetion($"Settings {key} is read-only, and cannot be modified.");
                }
                kv.Value = value;
            }
            catch (KeyNotFoundException ex)
            {
                throw new SettingsKeyNotFoundException($"Unknown settings key {key}.", ex);
            }
        }

        /// <summary>
        /// 释放托管与非托管对象。
        /// </summary>
        /// <param name="disposing">表示是否释放。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var provider in _providers)
                    {
                        (provider as IDisposable)?.Dispose();
                    }
                }
                _keyvaluesmap.Clear();
                disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
