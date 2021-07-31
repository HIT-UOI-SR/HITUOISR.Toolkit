using System.Collections.Generic;
using Xunit;

namespace HITUOISR.Toolkit.XunitExtension
{
    /// <summary>
    /// 用于比较的交换数据。
    /// </summary>
    /// <typeparam name="T">比较对象类型。</typeparam>
    public class ComparationCommutativeData<T> : TheoryData<int, T, T>
    {
        /// <summary>
        /// 初始化用于比较的交换数据。
        /// </summary>
        public ComparationCommutativeData() { }

        /// <summary>
        /// 初始化用于比较的交换数据。
        /// </summary>
        /// <param name="source">源数据。</param>
        public ComparationCommutativeData(IEnumerable<(int, T, T)> source)
        {
            foreach (var (order, lhs, rhs) in source)
            {
                Add(order, lhs, rhs);
            }
        }

        /// <summary>
        /// 添加测试数据。
        /// </summary>
        /// <param name="order">顺序。</param>
        /// <param name="lhs">左侧参数。</param>
        /// <param name="rhs">右侧参数。</param>
        public new void Add(int order, T lhs, T rhs)
        {
            base.Add(order, lhs, rhs);
            if (order != 0)
            {
                base.Add(-order, rhs, lhs);
            }
        }
    }

    /// <summary>
    /// 用于比较的交换数据。
    /// </summary>
    /// <typeparam name="T">比较对象类型。</typeparam>
    /// <typeparam name="TExtra">（常用于初始化比较器的）额外参数的类型。</typeparam>
    public class ComparationCommutativeData<T, TExtra> : TheoryData<int, T, T, TExtra>
    {
        /// <summary>
        /// 初始化用于比较的交换数据。
        /// </summary>
        public ComparationCommutativeData() { }

        /// <summary>
        /// 初始化用于比较的交换数据。
        /// </summary>
        /// <param name="source">源数据。</param>
        public ComparationCommutativeData(IEnumerable<(int, T, T, TExtra)> source)
        {
            foreach (var (order, lhs, rhs, extra) in source)
            {
                Add(order, lhs, rhs, extra);
            }
        }

        /// <summary>
        /// 添加测试数据。
        /// </summary>
        /// <param name="order">顺序。</param>
        /// <param name="lhs">左侧参数。</param>
        /// <param name="rhs">右侧参数。</param>
        /// <param name="extra">额外参数。</param>
        public new void Add(int order, T lhs, T rhs, TExtra extra)
        {
            base.Add(order, lhs, rhs, extra);
            if (order != 0)
            {
                base.Add(-order, rhs, lhs, extra);
            }
        }
    }
}
