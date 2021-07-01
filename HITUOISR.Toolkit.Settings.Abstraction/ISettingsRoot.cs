using System;
using System.Collections.Generic;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// 表示设置的根层次。
    /// </summary>
    public interface ISettingsRoot : ISettings
    {
        /// <summary>
        /// 与该设置关联的提供器。
        /// </summary>
        public IEnumerable<ISettingsProvider> Providers { get; }

        /// <summary>
        /// 所有可用的设置键。
        /// </summary>
        public IEnumerable<ISettingsKeyInfo> Keys { get; }

        /// <summary>
        /// 强制重新加载设置。
        /// </summary>
        void Reload();

        /// <summary>
        /// 保存设置。
        /// </summary>
        /// <exception cref="NotSupportedException">不支持保存的平台。</exception>
        void Save();
    }
}
