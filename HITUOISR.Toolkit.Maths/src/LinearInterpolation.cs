using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CovRNADetect.Maths
{
    /// <summary>
    /// 线性插值。
    /// </summary>
    public readonly struct LinearInterpolation
    {
        private readonly ImmutableArray<(double x, double y)> _points;

        /// <summary>
        /// 初始化线性插值函数。
        /// </summary>
        /// <param name="points">插值点。</param>
        public LinearInterpolation(IEnumerable<(double x, double y)> points)
        {
            if (points.Count() <= 1)
            {
                throw new ArgumentException("Too few points", nameof(points));
            }
            _points = points.OrderBy(p => p.x).ToImmutableArray();
        }

        /// <summary>
        /// 计算插值值。
        /// </summary>
        /// <param name="x">自变量值。</param>
        /// <returns>因变量值。</returns>
        public double Apply(double x) => ApplyImplRecv(_points, 0, _points.Length - 1, x);

        private static double ApplyImplRecv(ImmutableArray<(double x, double y)> points, int left, int right, double x)
        {
            (double x0, double y0) = points[left];
            (double x1, double y1) = points[right];
            // 插值：
            // 外插的情况也自动隐含进来了：搜索外插点时会向对应的边缘收缩直到定位到最边缘的区间，此时计算就是边缘直线的延伸
            if (right - left == 1)
            {
                return ((x - x0) * y1 + (x1 - x) * y0) / (x1 - x0);
            }
            // 区间递归二分收缩
            int mid = (left + right) / 2;
            double xm = points[mid].x;
            return x >= xm ? ApplyImplRecv(points, mid, right, x) : ApplyImplRecv(points, left, mid, x);
        }

        /// <summary>
        /// 隐式转换为函数。
        /// </summary>
        /// <param name="interpolation">线性插值。</param>
        public static implicit operator Func<double, double>(LinearInterpolation interpolation) => x => interpolation.Apply(x);
    }
}
