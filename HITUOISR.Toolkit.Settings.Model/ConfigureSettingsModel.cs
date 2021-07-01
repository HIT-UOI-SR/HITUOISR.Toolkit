using System;
using System.ComponentModel;

namespace HITUOISR.Toolkit.Settings.Model
{
    /// <summary>
    /// 配置设置模型。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    public class ConfigureSettingsModel<TModel> : IConfigureSettingsModel<TModel>
        where TModel : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initializer"></param>
        /// <param name="tracker"></param>
        public ConfigureSettingsModel(Action<TModel, ISettings> initializer, Action<ISettings, TModel, string?> tracker)
        {
            Initializer = initializer;
            Tracker = tracker;
        }

        private Action<TModel, ISettings> Initializer { get; }

        private Action<ISettings, TModel, string?> Tracker { get; }

        /// <inheritdoc/>
        public void Configure(ISettings settings, TModel model, bool bidirection = true)
        {
            Initializer(model, settings);
            if (bidirection && model is INotifyPropertyChanged notify)
            {
                var tracker = Tracker;
                notify.PropertyChanged += (o, e) => tracker(settings, model, e.PropertyName);
            }
        }
    }
}
