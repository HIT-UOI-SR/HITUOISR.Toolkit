using System.Collections.Generic;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// 表示用于生成应用设置的构造器。
    /// </summary>
    public interface ISettingsBuilder
    {
        /// <summary>
        /// 构造中使用的键值数据。
        /// </summary>
        IDictionary<string, object> Properties { get; }

        /// <summary>
        /// 获取设置源。
        /// </summary>
        IList<ISettingsSource> Sources { get; }

        /// <summary>
        /// 获取设置键信息。
        /// </summary>
        IEnumerable<ISettingsKeyInfo> SettingsKeys { get; }

        /// <summary>
        /// 根据设置源等信息生成设置。
        /// </summary>
        ISettingsRoot Build();

        /// <summary>
        /// 注册设置键。
        /// </summary>
        /// <param name="keyInfo">设置键信息。</param>
        /// <returns>该构造器。</returns>
        /// <exception cref="SettingsKeyExistsException">设置键已存在，无法再次注册。</exception>
        ISettingsBuilder RegisterKey(ISettingsKeyInfo keyInfo);
    }
}
