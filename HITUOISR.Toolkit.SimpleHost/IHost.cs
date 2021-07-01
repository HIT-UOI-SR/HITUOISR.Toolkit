using System;

namespace HITUOISR.Toolkit.SimpleHost
{
    /// <summary>
    /// 用于控制反转的通用主机接口。
    /// </summary>
    public interface IHost : IDisposable
    {
        /// <summary>
        /// 程序配置的服务。
        /// </summary>
        public IServiceProvider Services { get; }
    }
}
