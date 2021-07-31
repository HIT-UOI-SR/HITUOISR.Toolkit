using System;

namespace HITUOISR.Toolkit.PluginBase
{
    /// <summary>
    /// 具名扩展接口。
    /// </summary>
    public interface INamedExtension
    {
        /// <summary>
        /// 扩展名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 唯一标识。
        /// </summary>
        Guid Guid { get; }
    }
}
