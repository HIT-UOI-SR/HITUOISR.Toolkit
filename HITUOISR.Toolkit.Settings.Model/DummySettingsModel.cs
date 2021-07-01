using System.ComponentModel;

namespace HITUOISR.Toolkit.Settings.Model
{
    /// <summary>
    /// 伪设置模型（不与设置关联）。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class DummySettingsModel<TModel> : ISettingsModel<TModel>, ISettingsModelSnapshot<TModel>
        where TModel : class, INotifyPropertyChanged, new()
    {
        /// <summary>
        /// 初始化伪设置模型。
        /// </summary>
        public DummySettingsModel() => Value = new();

        /// <summary>
        /// 从指定的模型数据中初始化伪设置模型。
        /// </summary>
        /// <param name="model">关联的模型对象。</param>
        public DummySettingsModel(TModel model) => Value = model;

        /// <inheritdoc/>
        public TModel Value { get; }
    }
}
