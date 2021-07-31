using System;
using System.Collections.Generic;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// 辅助实现 <seealso cref="ISettingsProvider"/> 的基类。
    /// </summary>
    public abstract class SettingsProvider : ISettingsProvider
    {
        /// <summary>
        /// 缓存的数据。
        /// </summary>
        protected IDictionary<string, object?> Data { get; set; }

        /// <summary>
        /// 设置源。
        /// </summary>
        protected ISettingsSource Source { get; init; }

        /// <inheritdoc/>
        public virtual bool IsReadOnly => Source.IsReadOnly;

        /// <summary>
        /// 初始化。
        /// </summary>
        protected SettingsProvider(ISettingsSource source)
        {
            Source = source;
            Data = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        }

        /// <inheritdoc/>
        public virtual bool TryLoad(ISettingsKeyInfo key, bool reloadRequest = false) => false;

        /// <inheritdoc/>
        public virtual bool Save() => false;

        /// <inheritdoc/>
        public virtual bool TryGet(string key, out object? value) => Data.TryGetValue(key, out value);

        /// <inheritdoc/>
        public virtual bool Set(string key, object? value)
        {
            if (!IsReadOnly) Data[key] = value;
            return !IsReadOnly;
        }

        /// <inheritdoc/>
        public override string ToString() => GetType().Name;
    }
}
