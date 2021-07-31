using System;

namespace HITUOISR.Toolkit.Settings.FileBase
{
    /// <summary>
    /// 文件设置提供器异常上下文。
    /// </summary>
    public class FileExceptionContext
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="exception"></param>
        public FileExceptionContext(FileSettingsProvider provider, Exception exception)
        {
            Provider = provider;
            Exception = exception;
        }

        /// <summary>
        /// 发生异常的提供器。
        /// </summary>
        public FileSettingsProvider Provider { get; set; }

        /// <summary>
        /// 对应的异常。
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 是否可忽略（不会重新抛出）。
        /// </summary>
        public bool Ignore { get; set; }
    }
}
