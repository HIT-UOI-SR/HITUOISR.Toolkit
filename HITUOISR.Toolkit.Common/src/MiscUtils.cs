using System;
using System.Threading.Tasks;

namespace HITUOISR.Toolkit.Common
{
    /// <summary>
    /// 杂项工具。
    /// </summary>
    public static class MiscUtils
    {
        /// <summary>
        /// 恒等变换。
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="arg">参数。</param>
        /// <returns>与参数一样的值。</returns>
        public static T Identity<T>(T arg) => arg;

        /// <summary>
        /// 自动根据实现类型选择释放资源的方法(同步/异步)。
        /// </summary>
        /// <param name="obj">要释放的对象。</param>
        /// <returns>异步释放任务。</returns>
        public static async ValueTask DisposeAsync(object? obj)
        {
            switch (obj)
            {
                case IAsyncDisposable asyncDisposable:
                    await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                    break;
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
                default:
                    break;
            }
        }
    }
}
