using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Xunit;

namespace HITUOISR.Toolkit.XunitExtension
{
    /// <summary>
    /// 表示通过两个数据序列合并创建的数据矩阵。
    /// </summary>
    /// <typeparam name="T1">第一维的数据元素类型。</typeparam>
    /// <typeparam name="T2">第二维的数据元素类型。</typeparam>
    public class MatrixTheoryData<T1, T2> : TheoryData<T1, T2>
    {
        /// <summary>
        /// 初始化 <see cref="MatrixTheoryData{T1, T2}"/>（1×1）。
        /// </summary>
        /// <param name="srcdim1">第一维的数据。</param>
        /// <param name="srcdim2">第二维的数据。</param>
        public MatrixTheoryData(IEnumerable<T1> srcdim1, IEnumerable<T2> srcdim2)
        {
            Contract.Requires(srcdim1.Any());
            Contract.Requires(srcdim2.Any());
            foreach (var t1 in srcdim1)
            {
                foreach (var t2 in srcdim2)
                {
                    Add(t1, t2);
                }
            }
        }

        /// <summary>
        /// 初始化 <see cref="MatrixTheoryData{T1, T2}"/>（1×1）。
        /// </summary>
        /// <param name="srcdim1">第一维的数据。</param>
        /// <param name="srcdim2">第二维的数据。</param>
        /// <param name="filter">数据过滤条件。</param>
        public MatrixTheoryData(IEnumerable<T1> srcdim1, IEnumerable<T2> srcdim2, Func<T1, T2, bool> filter)
        {
            Contract.Requires(srcdim1.Any());
            Contract.Requires(srcdim2.Any());
            foreach (var t1 in srcdim1)
            {
                foreach (var t2 in srcdim2)
                {
                    if (filter(t1, t2))
                        Add(t1, t2);
                }
            }
        }

        /// <summary>
        /// 初始化 <see cref="MatrixTheoryData{T1, T2}"/>（1×1）。
        /// </summary>
        /// <param name="srcdim1">第一维的数据。</param>
        /// <param name="srcdim2">第二维的数据。</param>
        public MatrixTheoryData(TheoryData<T1> srcdim1, TheoryData<T2> srcdim2)
        {
            Contract.Requires(srcdim1.Any());
            Contract.Requires(srcdim2.Any());
            foreach (var t1 in srcdim1.AsValueTuples())
            {
                foreach (var t2 in srcdim2.AsValueTuples())
                {
                    Add(t1, t2);
                }
            }
        }
    }

    /// <summary>
    /// 表示通过三个数据序列合并创建的数据矩阵。
    /// </summary>
    /// <typeparam name="T1">第一维的数据元素类型。</typeparam>
    /// <typeparam name="T2">第二维的数据元素类型。</typeparam>
    /// <typeparam name="T3">第三维的数据元素类型。</typeparam>
    public class MatrixTheoryData<T1, T2, T3> : TheoryData<T1, T2, T3>
    {
        /// <summary>
        /// 初始化 <see cref="MatrixTheoryData{T1, T2, T3}"/>（1×1×1）。
        /// </summary>
        /// <param name="srcdim1">第一维的数据。</param>
        /// <param name="srcdim2">第二维的数据。</param>
        /// <param name="srcdim3">第三维的数据。</param>
        public MatrixTheoryData(IEnumerable<T1> srcdim1, IEnumerable<T2> srcdim2, IEnumerable<T3> srcdim3)
        {
            Contract.Requires(srcdim1.Any());
            Contract.Requires(srcdim2.Any());
            Contract.Requires(srcdim3.Any());
            foreach (var t1 in srcdim1)
            {
                foreach (var t2 in srcdim2)
                {
                    foreach (var t3 in srcdim3)
                    {
                        Add(t1, t2, t3);
                    }
                }
            }
        }

        /// <summary>
        /// 初始化 <see cref="MatrixTheoryData{T1, T2, T3}"/>（1×1×1）。
        /// </summary>
        /// <param name="srcdim1">第一维的数据。</param>
        /// <param name="srcdim2">第二维的数据。</param>
        /// <param name="srcdim3">第三维的数据。</param>
        /// <param name="filter">数据过滤条件。</param>
        public MatrixTheoryData(IEnumerable<T1> srcdim1, IEnumerable<T2> srcdim2, IEnumerable<T3> srcdim3, Func<T1, T2, T3, bool> filter)
        {
            Contract.Requires(srcdim1.Any());
            Contract.Requires(srcdim2.Any());
            Contract.Requires(srcdim3.Any());
            foreach (var t1 in srcdim1)
            {
                foreach (var t2 in srcdim2)
                {
                    foreach (var t3 in srcdim3)
                    {
                        if (filter(t1, t2, t3))
                            Add(t1, t2, t3);
                    }
                }
            }
        }

        /// <summary>
        /// 初始化 <see cref="MatrixTheoryData{T1, T2, T3}"/>（1×1×1）。
        /// </summary>
        /// <param name="srcdim1">第一维的数据。</param>
        /// <param name="srcdim2">第二维的数据。</param>
        /// <param name="srcdim3">第三维的数据。</param>
        public MatrixTheoryData(TheoryData<T1> srcdim1, TheoryData<T2> srcdim2, TheoryData<T3> srcdim3)
        {
            Contract.Requires(srcdim1.Any());
            Contract.Requires(srcdim2.Any());
            Contract.Requires(srcdim3.Any());
            foreach (var t1 in srcdim1.AsValueTuples())
            {
                foreach (var t2 in srcdim2.AsValueTuples())
                {
                    foreach (var t3 in srcdim3.AsValueTuples())
                    {
                        Add(t1, t2, t3);
                    }
                }
            }
        }

        /// <summary>
        /// 初始化 <see cref="MatrixTheoryData{T1, T2, T3}"/>（2×1）。
        /// </summary>
        /// <param name="srcdim12">第一、二维的数据。</param>
        /// <param name="srcdim3">第三维的数据。</param>
        public MatrixTheoryData(TheoryData<T1, T2> srcdim12, TheoryData<T3> srcdim3)
        {
            Contract.Requires(srcdim12.Any());
            Contract.Requires(srcdim3.Any());
            foreach (var (t1, t2) in srcdim12.AsValueTuples())
            {
                foreach (var t3 in srcdim3.AsValueTuples())
                {
                    Add(t1, t2, t3);
                }
            }
        }

        /// <summary>
        /// 初始化 <see cref="MatrixTheoryData{T1, T2, T3}"/>（1×2）。
        /// </summary>
        /// <param name="srcdim1">第一维的数据。</param>
        /// <param name="srcdim23">第二、三维的数据。</param>
        public MatrixTheoryData(TheoryData<T1> srcdim1, TheoryData<T2, T3> srcdim23)
        {
            Contract.Requires(srcdim1.Any());
            Contract.Requires(srcdim23.Any());
            foreach (var t1 in srcdim1.AsValueTuples())
            {
                foreach (var (t2, t3) in srcdim23.AsValueTuples())
                {
                    Add(t1, t2, t3);
                }
            }
        }
    }
}
