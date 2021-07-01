using System;
using System.Runtime.Serialization;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// 请求的设置类型与实际不匹配时触发。
    /// </summary>
    public class SettingsTypeMismatchException : Exception
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        public SettingsTypeMismatchException()
        {
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="message">消息。</param>
        public SettingsTypeMismatchException(string? message) : base(message)
        {
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="message">消息。</param>
        /// <param name="innerException">来源的异常。</param>
        public SettingsTypeMismatchException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// 用于反序列化的初始化。
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected SettingsTypeMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
