namespace HITUOISR.Toolkit.Settings.Model
{
    /// <summary>
    /// 只读设置模型。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    public class ReadOnlySettingsModel<TModel> : ISettingsModelSnapshot<TModel> where TModel : class, new()
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="settings">依赖的设置。</param>
        /// <param name="configure">模型配置操作。</param>
        public ReadOnlySettingsModel(ISettings settings, IConfigureSettingsModel<TModel> configure)
        {
            Value = new();
            configure.Configure(settings, Value, bidirection: false);
        }

        /// <inheritdoc/>
        public TModel Value { get; }
    }
}
