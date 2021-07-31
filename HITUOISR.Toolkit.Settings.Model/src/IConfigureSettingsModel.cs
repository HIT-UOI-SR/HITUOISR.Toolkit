namespace HITUOISR.Toolkit.Settings.Model
{
    /// <summary>
    /// 表示对模型的配置过程。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    public interface IConfigureSettingsModel<in TModel> where TModel : class
    {
        /// <summary>
        /// 调用以配置指定模型实例。
        /// </summary>
        /// <param name="settings">依赖的设置。</param>
        /// <param name="model">要配置的模型实例。</param>
        /// <param name="bidirection">指示是否双向绑定。</param>
        void Configure(ISettings settings, TModel model, bool bidirection = true);
    }
}
