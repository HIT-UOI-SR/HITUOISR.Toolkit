using System;
using System.Runtime.Serialization;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// 设置键已存在，无法再次注册时触发。
    /// </summary>
    public class SettingsKeyExistsException : Exception
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        public SettingsKeyExistsException()
        {
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="message">消息。</param>
        public SettingsKeyExistsException(string? message) : base(message)
        {
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="message">消息。</param>
        /// <param name="innerException">来源的异常。</param>
        public SettingsKeyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// 用于反序列化的初始化。
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected SettingsKeyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
