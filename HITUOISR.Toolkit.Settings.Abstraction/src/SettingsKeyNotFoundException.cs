using System;
using System.Runtime.Serialization;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// 试图使用不存在的设置键时触发的异常。
    /// </summary>
    public class SettingsKeyNotFoundException : Exception
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        public SettingsKeyNotFoundException()
        {
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="message">消息。</param>
        public SettingsKeyNotFoundException(string? message) : base(message)
        {
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="message">消息。</param>
        /// <param name="innerException">来源的异常。</param>
        public SettingsKeyNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// 用于反序列化的初始化。
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected SettingsKeyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
