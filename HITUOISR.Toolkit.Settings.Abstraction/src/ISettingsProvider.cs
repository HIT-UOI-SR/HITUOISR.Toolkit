namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// 设置提供服务。
    /// </summary>
    public interface ISettingsProvider
    {
        /// <summary>
        /// 指示该设置提供服务是否只读。
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// 尝试从源中加载指定设置。
        /// </summary>
        /// <param name="key">要加载的设置键信息。</param>
        /// <param name="reloadRequest">请求重新加载。</param>
        /// <returns>如果源中存在对应设置，则为<see langword="true"/>；否则为<see langword="false"/></returns>
        bool TryLoad(ISettingsKeyInfo key, bool reloadRequest = false);

        /// <summary>
        /// 保存设置到源。
        /// </summary>
        /// <returns>指示保存是否成功。</returns>
        bool Save();

        /// <summary>
        /// 尝试获取设置值。
        /// </summary>
        /// <param name="key">关联的键信息。</param>
        /// <param name="value">对应的键值。</param>
        /// <returns>指示提供源中是否存在该键。</returns>
        bool TryGet(string key, out object? value);

        /// <summary>
        /// 设置键。
        /// </summary>
        /// <param name="key">要设置的键。</param>
        /// <param name="value">要设置的值。</param>
        /// <returns>指示设置是否成功且修改了值。</returns>
        bool Set(string key, object? value);
    }
}
