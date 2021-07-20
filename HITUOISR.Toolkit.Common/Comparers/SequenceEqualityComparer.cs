using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace HITUOISR.Toolkit.Common.Comparers
{
    /// <summary>
    /// 序列相等性比较器。
    /// </summary>
    /// <typeparam name="TElement">元素类型。</typeparam>
    /// <typeparam name="TSequence">序列类型。</typeparam>
    public class SequenceEqualityComparer<TElement, TSequence> : EqualityComparer<TSequence>
        where TSequence : IEnumerable<TElement>
    {
        private readonly IEqualityComparer<TElement> ElementComparer;

        /// <summary>
        /// 初始化序列相等性比较器。
        /// </summary>
        public SequenceEqualityComparer() : this(EqualityComparer<TElement>.Default) { }

        /// <summary>
        /// 初始化序列相等性比较器。
        /// </summary>
        /// <param name="elementComparer">元素比较器。</param>
        public SequenceEqualityComparer(IEqualityComparer<TElement> elementComparer) => ElementComparer = elementComparer;

        /// <inheritdoc/>
        public override bool Equals(TSequence? x, TSequence? y) =>
            ReferenceEquals(x, y) || y != null && x != null && x.SequenceEqual(y, ElementComparer);

        /// <inheritdoc/>
        public override int GetHashCode([DisallowNull] TSequence obj)
        {
            HashCode hash = new();
            foreach (TElement item in obj)
            {
                hash.Add(item, ElementComparer);
            }
            return hash.ToHashCode();
        }
    }

    /// <summary>
    /// 序列相等性比较器。
    /// </summary>
    /// <typeparam name="TElement">元素类型。</typeparam>
    public class SequenceEqualityComparer<TElement> : SequenceEqualityComparer<TElement, IEnumerable<TElement>>
    {
        /// <summary>
        /// 初始化序列相等性比较器。
        /// </summary>
        public SequenceEqualityComparer() { }

        /// <summary>
        /// 初始化序列相等性比较器。
        /// </summary>
        /// <param name="elementComparer">元素比较器。</param>
        public SequenceEqualityComparer(IEqualityComparer<TElement> elementComparer) : base(elementComparer) { }
    }
}
