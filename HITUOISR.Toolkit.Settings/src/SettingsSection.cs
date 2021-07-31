using System;
using System.Collections.Generic;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// <seealso cref="ISettingsSection"/> 实现。
    /// </summary>
    public class SettingsSection : ISettingsSection
    {
        private readonly ISettingsRoot _root;
        private readonly string _path;

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="root">关联的设置根。</param>
        /// <param name="path">使用的路径。</param>
        public SettingsSection(ISettingsRoot root, string path)
        {
            _root = root;
            _path = path;
        }

        /// <inheritdoc/>
        public string Key => SettingsPath.GetSectionKey(_path);

        /// <inheritdoc/>
        public string Path => _path;

        /// <inheritdoc/>
        public ISettingsSection GetSection(string key) => new SettingsSection(_root, SettingsPath.Combine(_path, key));

        /// <inheritdoc/>
        public IEnumerable<ISettingsSection> GetChildren() => _root.GetChildrenImpl(Path);

        /// <inheritdoc/>
        public T GetValue<T>() => _root.GetValue<T>(_path);

        /// <inheritdoc/>
        public T GetValue<T>(string key) => _root.GetValue<T>(SettingsPath.Combine(_path, key));

        /// <inheritdoc/>
        public T GetValueOrElse<T>(string key, Func<T> defaultGetter) => _root.GetValueOrElse(SettingsPath.Combine(_path, key), defaultGetter);

        /// <inheritdoc/>
        public T GetValueOrElse<T>(Func<T> defaultGetter) => _root.GetValueOrElse(_path, defaultGetter);

        /// <inheritdoc/>
        public void SetValue<T>(T value) => _root.SetValue(_path, value);

        /// <inheritdoc/>
        public void SetValue<T>(string key, T value) => _root.SetValue(SettingsPath.Combine(_path, key), value);
    }
}
