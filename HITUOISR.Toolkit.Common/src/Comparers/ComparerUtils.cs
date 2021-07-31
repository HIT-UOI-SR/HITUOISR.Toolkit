using System;
using System.Collections.Generic;

namespace HITUOISR.Toolkit.Common.Comparers
{
    /// <summary>
    /// 比较器工具。
    /// </summary>
    public static class ComparerUtils
    {
        /// <summary>
        /// 从选择器中创建比较器。
        /// </summary>
        /// <typeparam name="T">要应用比较的类型。</typeparam>
        /// <typeparam name="TResult">要进行实际比较操作的结果对象类型。</typeparam>
        /// <param name="selector">属性选择器。</param>
        /// <returns>根据选择器创建的比较器。</returns>
        public static IComparer<T> FromSelector<T, TResult>(Func<T, TResult> selector) where TResult : IComparable<TResult> =>
            Comparer<T>.Create((x, y) => Comparer<TResult>.Default.Compare(selector(x), selector(y)));

        /// <summary>
        /// 从选择器中创建比较器。
        /// </summary>
        /// <typeparam name="T">要应用比较的类型。</typeparam>
        /// <typeparam name="TResult">要进行实际比较操作的结果对象类型。</typeparam>
        /// <param name="selector">属性选择器。</param>
        /// <param name="comparer">结果对象的比较器。</param>
        /// <returns>根据选择器创建的比较器。</returns>
        public static IComparer<T> FromSelector<T, TResult>(Func<T, TResult> selector, IComparer<TResult> comparer) =>
            Comparer<T>.Create((x, y) => comparer.Compare(selector(x), selector(y)));
    }
}
