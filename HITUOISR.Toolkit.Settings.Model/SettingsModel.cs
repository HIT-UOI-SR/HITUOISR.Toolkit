using System.ComponentModel;

namespace HITUOISR.Toolkit.Settings.Model
{
    /// <summary>
    /// 设置模型。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    public class SettingsModel<TModel> : ISettingsModel<TModel> where TModel : class, INotifyPropertyChanged, new()
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="settings">依赖的设置。</param>
        /// <param name="configure">模型配置操作。</param>
        public SettingsModel(ISettings settings, IConfigureSettingsModel<TModel> configure)
        {
            Value = new();
            configure.Configure(settings, Value);
        }

        /// <inheritdoc/>
        public TModel Value { get; }
    }
}
