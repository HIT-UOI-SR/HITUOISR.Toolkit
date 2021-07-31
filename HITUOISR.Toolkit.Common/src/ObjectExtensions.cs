using System;

namespace HITUOISR.Toolkit.Common
{
    /// <summary>
    /// <see cref="object"/> 扩展方法。
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 检查对象是否可分配给指定类型。
        /// </summary>
        /// <param name="obj">要检查的对象。</param>
        /// <param name="type">指定的类型。</param>
        /// <returns>
        /// 如果满足下列任一条件，则为 <see langword="true"/>：
        /// <list type="bullet">
        /// <item><paramref name="obj"/>为<see langword="null"/>，且<paramref name="type"/>是可以为空的类型；</item>
        /// <item><paramref name="obj"/>的类型和<paramref name="type"/>相同；</item>
        /// <item><paramref name="obj"/>的类型直接或间接派生于<paramref name="type"/>；</item>
        /// <item><paramref name="obj"/>是接口<paramref name="type"/>的实现；</item>
        /// <item><paramref name="obj"/>具有值类型<c>T</c>，且<paramref name="type"/>为<see cref="Nullable{T}"/>。</item>
        /// </list>
        /// 否则为<see langword="false"/>。
        /// </returns>
        public static bool IsAssignableToType(this object? obj, Type type) =>
            obj is null ? type.IsNullable() : type.IsAssignableFrom(obj.GetType());

        /// <summary>
        /// 恢复到类型。
        /// </summary>
        /// <remarks>
        /// 只有对象原本就属于该类型时，转换才能成功。
        /// </remarks>
        /// <typeparam name="TResult">目标类型。</typeparam>
        /// <param name="src">源对象。</param>
        /// <returns>转换后的对象。</returns>
        public static TResult RestoreTo<TResult>(this object? src)
        {
            if (Nullable.GetUnderlyingType(typeof(TResult)) != null && src == null)
                return (TResult)src!;
            return src is TResult res ? res : throw new InvalidCastException($"Cannot restore {src} to {typeof(TResult)}.");
        }

        /// <summary>
        /// 恢复到类型，或者使用指定的默认值。。
        /// </summary>
        /// <typeparam name="TResult">目标类型。</typeparam>
        /// <param name="src">源对象。</param>
        /// <param name="defaultGetter">获取默认值的委托。</param>
        /// <returns>转换后的对象。</returns>
        public static TResult RestoreToOrElse<TResult>(this object? src, Func<TResult> defaultGetter)
        {
            if (Nullable.GetUnderlyingType(typeof(TResult)) != null && src == null)
                return (TResult)src!;
            return src is TResult res ? res : defaultGetter();
        }
    }
}
