using System;
using System.Runtime.Serialization;

namespace HITUOISR.Toolkit.Settings
{
    /// <summary>
    /// 尝试修改只读设置时触发。
    /// </summary>
    public class SettingsReadOnlyExcepetion : Exception
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        public SettingsReadOnlyExcepetion()
        {
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="message">消息。</param>
        public SettingsReadOnlyExcepetion(string? message) : base(message)
        {
        }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="message">消息。</param>
        /// <param name="innerException">来源的异常。</param>
        public SettingsReadOnlyExcepetion(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// 用于反序列化的初始化。
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected SettingsReadOnlyExcepetion(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
