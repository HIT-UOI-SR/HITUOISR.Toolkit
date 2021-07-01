namespace HITUOISR.Toolkit.Settings.Model
{
    /// <summary>
    /// 设置模型快照接口。
    /// </summary>
    /// <typeparam name="TModel">关联的模型类型。</typeparam>
    public interface ISettingsModelSnapshot<out TModel> where TModel : class, new()
    {
        /// <summary>
        /// 获取设置模型的值。
        /// </summary>
        public TModel Value { get; }
    }
}
