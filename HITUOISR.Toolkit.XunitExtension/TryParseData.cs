using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace HITUOISR.Toolkit.XunitExtension
{
    /// <summary>
    /// 解析测试数据。
    /// </summary>
    /// <remarks>
    /// 对应于：结果、源、是否成功。
    /// </remarks>
    /// <typeparam name="TResult">解析结果类型。</typeparam>
    /// <typeparam name="TSource">解析源类型。</typeparam>
    public class TryParseData<TResult, TSource> : TheoryData<TResult?, TSource, bool>
    {
        /// <summary>
        /// 有效的数据。
        /// </summary>
        /// <remarks>
        /// 对应于：结果、源
        /// </remarks>
        public TheoryData<TResult, TSource> ValidData { get; } = new();

        /// <summary>
        /// 无效的数据。
        /// </summary>
        /// <remarks>
        /// 对应于：源
        /// </remarks>
        public TheoryData<TSource> InvalidData { get; } = new();

        /// <summary>
        /// 添加测试数据项。
        /// </summary>
        /// <remarks>
        /// <paramref name="result"/> 为 <see langword="null"/> 时，为无效数据，否则为有效数据。
        /// </remarks>
        /// <param name="result">预期结果。</param>
        /// <param name="source">输入源。</param>
        public void Add([AllowNull] TResult result, TSource source)
        {
            if (result is null)
            {
                base.Add(result, source, false);
                InvalidData.Add(source);
            }
            else
            {
                base.Add(result, source, true);
                ValidData.Add(result, source);
            }
        }

        /// <summary/>
        [Obsolete("Do not add success flag manully.")]
        public new void Add([AllowNull] TResult p1, TSource p2, bool p3) => base.Add(p1, p2, p3);

        /// <summary>
        /// 初始化解析测试数据。
        /// </summary>
        public TryParseData() { }

        /// <summary>
        /// 初始化解析测试数据。
        /// </summary>
        /// <param name="template">测试数据样板。</param>
        public TryParseData(IEnumerable<(TResult, TSource)> template)
        {
            foreach (var (result, source) in template)
            {
                Add(result, source);
            }
        }
    }
}
