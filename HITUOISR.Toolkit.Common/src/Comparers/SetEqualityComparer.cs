using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace HITUOISR.Toolkit.Common.Comparers
{
    /// <summary>
    /// 集合的相等性比较器。
    /// </summary>
    /// <remarks>
    /// 基于 <seealso cref="ISet{T}.SetEquals(IEnumerable{T})"/> 实现。
    /// </remarks>
    /// <typeparam name="T">集合元素类型。</typeparam>
    public class SetEqualityComparer<T> : EqualityComparer<ISet<T>>
    {
        /// <inheritdoc/>
        public override bool Equals(ISet<T>? x, ISet<T>? y) =>
            y is null ?
            x is null :
            x?.SetEquals(y) ?? y is null;

        /// <inheritdoc/>
        public override int GetHashCode([DisallowNull] ISet<T> obj) => obj
            .Select(v => v?.GetHashCode() ?? 0)
            .Aggregate(0, (v1, v2) => v1 ^ v2);
    }
}
