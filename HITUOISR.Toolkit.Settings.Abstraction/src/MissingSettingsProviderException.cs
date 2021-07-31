using System;
using System.Runtime.Serialization;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// 缺少可用的设置提供器时触发。
    /// </summary>
    public class MissingSettingsProviderException : Exception
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        public MissingSettingsProviderException()
        {
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="message">消息。</param>
        public MissingSettingsProviderException(string? message) : base(message)
        {
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="message">消息。</param>
        /// <param name="innerException">来源的异常。</param>
        public MissingSettingsProviderException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// 用于反序列化的初始化。
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected MissingSettingsProviderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
