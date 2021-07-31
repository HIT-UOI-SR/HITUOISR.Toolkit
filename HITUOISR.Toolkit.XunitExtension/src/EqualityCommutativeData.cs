using System.Collections.Generic;
using Xunit;

namespace HITUOISR.Toolkit.XunitExtension
{
    /// <summary>
    /// 相等性可对易数据。
    /// </summary>
    /// <typeparam name="T">元素类型。</typeparam>
    public class EqualityCommutativeData<T> : TheoryData<bool, T, T>
    {
        /// <summary>
        /// 初始化相等性可对易数据。
        /// </summary>
        public EqualityCommutativeData() { }

        /// <summary>
        /// 初始化相等性可对易数据。
        /// </summary>
        /// <param name="source">数据源。</param>
        public EqualityCommutativeData(IEnumerable<(bool, T, T)> source)
        {
            foreach (var (eq, lhs, rhs) in source)
            {
                Add(eq, lhs, rhs);
            }
        }

        /// <summary>
        /// 添加测试数据。
        /// </summary>
        /// <param name="eq">相等性。</param>
        /// <param name="lhs">左侧数据。</param>
        /// <param name="rhs">右侧数据。</param>
        public new void Add(bool eq, T lhs, T rhs)
        {
            base.Add(eq, lhs, rhs);
            if (!ReferenceEquals(lhs, rhs))
            {
                base.Add(eq, rhs, lhs);
            }
        }
    }
}
