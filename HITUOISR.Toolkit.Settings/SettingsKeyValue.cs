using HITUOISR.Toolkit.Common;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// <seealso cref="ISettingsKeyValue"/> 的实现。
    /// </summary>
    public class SettingsKeyValue : ISettingsKeyValue
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="info"></param>
        public SettingsKeyValue(ISettingsKeyInfo info) => KeyInfo = info;

        /// <inheritdoc/>
        public ISettingsKeyInfo KeyInfo { get; }

        /// <inheritdoc/>
        public object? Value
        {
            get => Provider != null && Provider.TryGet(KeyInfo.Path, out object? value) ? value : KeyInfo.DefaultValue;
            set
            {
                if (value.IsAssignableToType(KeyInfo.Type))
                {
                    if (Provider is null)
                    {
                        throw new MissingSettingsProviderException($"There is no avaliable settings provider for {KeyInfo}");
                    }
                    IsModified = Provider.Set(KeyInfo.Path, value);
                }
                else
                {
                    throw new SettingsTypeMismatchException($"{value} cannot assign to settings {KeyInfo}.");
                }
            }
        }

        /// <inheritdoc/>
        public ISettingsProvider? Provider { get; set; }

        /// <inheritdoc/>
        public bool IsModified { get; private set; }

        /// <inheritdoc/>
        public bool UsingDefaultValue => Provider is null || !Provider.TryGet(KeyInfo.Path, out _);
    }
}
