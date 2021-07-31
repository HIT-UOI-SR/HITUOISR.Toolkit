using System.ComponentModel;

namespace HITUOISR.Toolkit.Settings.Model
{
    /// <summary>
    /// 设置模型接口。
    /// </summary>
    /// <typeparam name="TModel">关联的模型类型。</typeparam>
    public interface ISettingsModel<out TModel> where TModel : class, INotifyPropertyChanged, new()
    {
        /// <summary>
        /// 获取设置模型的值。
        /// </summary>
        public TModel Value { get; }
    }
}
