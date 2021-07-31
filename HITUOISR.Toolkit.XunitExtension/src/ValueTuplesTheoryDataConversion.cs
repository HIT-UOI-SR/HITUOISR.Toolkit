using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace HITUOISR.Toolkit.XunitExtension
{
    /// <summary>
    /// 元组序列与 <seealso cref="TheoryData"/> 的辅助转换方法。
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ValueTuplesTheoryDataConversion
    {
        /// <summary>
        /// 从单参数值序列中创建 <see cref="TheoryData{T}"/>。
        /// </summary>
        /// <typeparam name="T">参数类型。</typeparam>
        /// <param name="vs">参数值序列。</param>
        /// <returns>对应的测试数据。</returns>
        public static TheoryData<T> ToTheoryData<T>(this IEnumerable<T> vs)
        {
            TheoryData<T> data = new();
            foreach (var v in vs)
            {
                data.Add(v);
            }
            return data;
        }

        /// <summary>
        /// 从双参数值序列中创建 <see cref="TheoryData{T1, T2}"/>。
        /// </summary>
        /// <typeparam name="T1">第一个参数的类型。</typeparam>
        /// <typeparam name="T2">第二个参数的类型。</typeparam>
        /// <param name="vs">参数值序列。</param>
        /// <returns>对应的测试数据。</returns>
        public static TheoryData<T1, T2> ToTheoryData<T1, T2>(this IEnumerable<(T1, T2)> vs)
        {
            TheoryData<T1, T2> data = new();
            foreach (var (v1, v2) in vs)
            {
                data.Add(v1, v2);
            }
            return data;
        }

        /// <summary>
        /// 从三参数值序列中创建 <see cref="TheoryData{T1, T2, T3}"/>。
        /// </summary>
        /// <typeparam name="T1">第一个参数的类型。</typeparam>
        /// <typeparam name="T2">第二个参数的类型。</typeparam>
        /// <typeparam name="T3">第三个参数的类型。</typeparam>
        /// <param name="vs">参数值序列。</param>
        /// <returns>对应的测试数据。</returns>
        public static TheoryData<T1, T2, T3> ToTheoryData<T1, T2, T3>(this IEnumerable<(T1, T2, T3)> vs)
        {
            TheoryData<T1, T2, T3> data = new();
            foreach (var (v1, v2, v3) in vs)
            {
                data.Add(v1, v2, v3);
            }
            return data;
        }

        /// <summary>
        /// 从四参数值序列中创建 <see cref="TheoryData{T1, T2, T3, T4}"/>。
        /// </summary>
        /// <typeparam name="T1">第一个参数的类型。</typeparam>
        /// <typeparam name="T2">第二个参数的类型。</typeparam>
        /// <typeparam name="T3">第三个参数的类型。</typeparam>
        /// <typeparam name="T4">第四个参数的类型。</typeparam>
        /// <param name="vs">参数值序列。</param>
        /// <returns>对应的测试数据。</returns>
        public static TheoryData<T1, T2, T3, T4> ToTheoryData<T1, T2, T3, T4>(this IEnumerable<(T1, T2, T3, T4)> vs)
        {
            TheoryData<T1, T2, T3, T4> data = new();
            foreach (var (v1, v2, v3, v4) in vs)
            {
                data.Add(v1, v2, v3, v4);
            }
            return data;
        }

        /// <summary>
        /// 从五参数值序列中创建 <see cref="TheoryData{T1, T2, T3, T4, T5}"/>。
        /// </summary>
        /// <typeparam name="T1">第一个参数的类型。</typeparam>
        /// <typeparam name="T2">第二个参数的类型。</typeparam>
        /// <typeparam name="T3">第三个参数的类型。</typeparam>
        /// <typeparam name="T4">第四个参数的类型。</typeparam>
        /// <typeparam name="T5">第五个参数的类型。</typeparam>
        /// <param name="vs">参数值序列。</param>
        /// <returns>对应的测试数据。</returns>
        public static TheoryData<T1, T2, T3, T4, T5> ToTheoryData<T1, T2, T3, T4, T5>(this IEnumerable<(T1, T2, T3, T4, T5)> vs)
        {
            TheoryData<T1, T2, T3, T4, T5> data = new();
            foreach (var (v1, v2, v3, v4, v5) in vs)
            {
                data.Add(v1, v2, v3, v4, v5);
            }
            return data;
        }

        /// <summary>
        /// 从六参数值序列中创建 <see cref="TheoryData{T1, T2, T3, T4, T5, T6}"/>。
        /// </summary>
        /// <typeparam name="T1">第一个参数的类型。</typeparam>
        /// <typeparam name="T2">第二个参数的类型。</typeparam>
        /// <typeparam name="T3">第三个参数的类型。</typeparam>
        /// <typeparam name="T4">第四个参数的类型。</typeparam>
        /// <typeparam name="T5">第五个参数的类型。</typeparam>
        /// <typeparam name="T6">第六个参数的类型。</typeparam>
        /// <param name="vs">参数值序列。</param>
        /// <returns>对应的测试数据。</returns>
        public static TheoryData<T1, T2, T3, T4, T5, T6> ToTheoryData<T1, T2, T3, T4, T5, T6>(this IEnumerable<(T1, T2, T3, T4, T5, T6)> vs)
        {
            TheoryData<T1, T2, T3, T4, T5, T6> data = new();
            foreach (var (v1, v2, v3, v4, v5, v6) in vs)
            {
                data.Add(v1, v2, v3, v4, v5, v6);
            }
            return data;
        }

        /// <summary>
        /// 从 <see cref="TheoryData{T}"/> 中创建序列。
        /// </summary>
        /// <typeparam name="T">参数类型。</typeparam>
        /// <param name="data">谓词数据。</param>
        /// <returns>对应的数据序列。</returns>
        public static IEnumerable<T> AsValueTuples<T>(this TheoryData<T> data) =>
            from a in data select (T)a[0];

        /// <summary>
        /// 从 <see cref="TheoryData{T1, T2}"/> 中创建序列。
        /// </summary>
        /// <typeparam name="T1">第一个参数的类型。</typeparam>
        /// <typeparam name="T2">第二个参数的类型。</typeparam>
        /// <param name="data">谓词数据。</param>
        /// <returns>对应的数据序列。</returns>
        public static IEnumerable<(T1, T2)> AsValueTuples<T1, T2>(this TheoryData<T1, T2> data) =>
            from a in data select ((T1)a[0], (T2)a[1]);

        /// <summary>
        /// 从 <see cref="TheoryData{T1, T2, T3}"/> 中创建序列。
        /// </summary>
        /// <typeparam name="T1">第一个参数的类型。</typeparam>
        /// <typeparam name="T2">第二个参数的类型。</typeparam>
        /// <typeparam name="T3">第三个参数的类型。</typeparam>
        /// <param name="data">谓词数据。</param>
        /// <returns>对应的数据序列。</returns>
        public static IEnumerable<(T1, T2, T3)> AsValueTuples<T1, T2, T3>(this TheoryData<T1, T2, T3> data) =>
            from a in data select ((T1)a[0], (T2)a[1], (T3)a[2]);

        /// <summary>
        /// 从 <see cref="TheoryData{T1, T2, T3, T4}"/> 中创建序列。
        /// </summary>
        /// <typeparam name="T1">第一个参数的类型。</typeparam>
        /// <typeparam name="T2">第二个参数的类型。</typeparam>
        /// <typeparam name="T3">第三个参数的类型。</typeparam>
        /// <typeparam name="T4">第四个参数的类型。</typeparam>
        /// <param name="data">谓词数据。</param>
        /// <returns>对应的数据序列。</returns>
        public static IEnumerable<(T1, T2, T3, T4)> AsValueTuples<T1, T2, T3, T4>(this TheoryData<T1, T2, T3, T4> data) =>
            from a in data select ((T1)a[0], (T2)a[1], (T3)a[2], (T4)a[3]);

        /// <summary>
        /// 从 <see cref="TheoryData{T1, T2, T3, T4, T5}"/> 中创建序列。
        /// </summary>
        /// <typeparam name="T1">第一个参数的类型。</typeparam>
        /// <typeparam name="T2">第二个参数的类型。</typeparam>
        /// <typeparam name="T3">第三个参数的类型。</typeparam>
        /// <typeparam name="T4">第四个参数的类型。</typeparam>
        /// <typeparam name="T5">第五个参数的类型。</typeparam>
        /// <param name="data">谓词数据。</param>
        /// <returns>对应的数据序列。</returns>
        public static IEnumerable<(T1, T2, T3, T4, T5)> AsValueTuples<T1, T2, T3, T4, T5>(this TheoryData<T1, T2, T3, T4, T5> data) =>
            from a in data select ((T1)a[0], (T2)a[1], (T3)a[2], (T4)a[3], (T5)a[4]);

        /// <summary>
        /// 从 <see cref="TheoryData{T1, T2, T3, T4, T5, T6}"/> 中创建序列。
        /// </summary>
        /// <typeparam name="T1">第一个参数的类型。</typeparam>
        /// <typeparam name="T2">第二个参数的类型。</typeparam>
        /// <typeparam name="T3">第三个参数的类型。</typeparam>
        /// <typeparam name="T4">第四个参数的类型。</typeparam>
        /// <typeparam name="T5">第五个参数的类型。</typeparam>
        /// <typeparam name="T6">第六个参数的类型。</typeparam>
        /// <param name="data">谓词数据。</param>
        /// <returns>对应的数据序列。</returns>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6)> AsValueTuples<T1, T2, T3, T4, T5, T6>(this TheoryData<T1, T2, T3, T4, T5, T6> data) =>
            from a in data select ((T1)a[0], (T2)a[1], (T3)a[2], (T4)a[3], (T5)a[4], (T6)a[5]);
    }
}
