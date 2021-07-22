using Xunit;

namespace HITUOISR.Toolkit.XunitExtension.Tests
{
    public class MatrixTheoryDataTests
    {
        [Fact()]
        public void Ctor_1x1()
        {
            var v1 = new[] { 1, 2 };
            var v2 = new[] { 2, 3 };
            MatrixTheoryData<int, int> m = new(v1, v2);
            Assert.Equal(new[] { (1, 2), (1, 3), (2, 2), (2, 3) }, m.AsValueTuples());
        }

        [Fact()]
        public void Ctor_Theory_1x1()
        {
            TheoryData<int> v1 = new() { 1, 2 };
            TheoryData<int> v2 = new() { 2, 3 };
            MatrixTheoryData<int, int> m = new(v1, v2);
            Assert.Equal(new[] { (1, 2), (1, 3), (2, 2), (2, 3) }, m.AsValueTuples());
        }

        [Fact()]
        public void Ctor_1x1_Filter()
        {
            var v1 = new[] { 1, 2 };
            var v2 = new[] { 2, 3 };
            MatrixTheoryData<int, int> m = new(v1, v2, (x1, x2) => x1 != x2);
            Assert.Equal(new[] { (1, 2), (1, 3), (2, 3) }, m.AsValueTuples());
        }

        [Fact()]
        public void Ctor_1x1x1()
        {
            var v1 = new[] { 1, 2 };
            var v2 = new[] { 2, 3 };
            var v3 = new[] { 3, 4 };
            MatrixTheoryData<int, int, int> m = new(v1, v2, v3);
            Assert.Equal(new[] { 
                (1, 2, 3), (1, 2, 4),
                (1, 3, 3), (1, 3, 4),
                (2, 2, 3), (2, 2, 4),
                (2, 3, 3), (2, 3, 4) }, 
                m.AsValueTuples());
        }

        [Fact()]
        public void Ctor_1x1x1_Filter()
        {
            var v1 = new[] { 1, 2 };
            var v2 = new[] { 2, 3 };
            var v3 = new[] { 3, 4 };
            MatrixTheoryData<int, int, int> m = new(v1, v2, v3, (x1, x2, x3) => x1 + x2 != x3);
            Assert.Equal(new[] {
                           (1, 2, 4),
                (1, 3, 3), 
                (2, 2, 3), 
                (2, 3, 3), (2, 3, 4) },
                m.AsValueTuples());
        }

        [Fact()]
        public void Ctor_Theory_1x1x1()
        {
            TheoryData<int> v1 = new() { 1, 2 };
            TheoryData<int> v2 = new() { 2, 3 };
            TheoryData<int> v3 = new() { 3, 4 };
            MatrixTheoryData<int, int, int> m = new(v1, v2, v3);
            Assert.Equal(new[] {
                (1, 2, 3), (1, 2, 4),
                (1, 3, 3), (1, 3, 4),
                (2, 2, 3), (2, 2, 4),
                (2, 3, 3), (2, 3, 4) },
                m.AsValueTuples());
        }

        [Fact()]
        public void Ctor_Theory_1x2()
        {
            TheoryData<int> v1 = new() { 1, 2 };
            TheoryData<int, int> v2 = new() { { 2, 2 }, { 2, 3 } };
            MatrixTheoryData<int, int, int> m = new(v1, v2);
            Assert.Equal(new[] { (1, 2, 2), (1, 2, 3), (2, 2, 2), (2, 2, 3) }, m.AsValueTuples());
        }

        [Fact()]
        public void Ctor_Theory_2x1()
        {
            TheoryData<int, int> v1 = new() { { 1, 1 }, { 1, 2 } };
            TheoryData<int> v2 = new() { 2, 3 };
            MatrixTheoryData<int, int, int> m = new(v1, v2);
            Assert.Equal(new[] { (1, 1, 2), (1, 1, 3), (1, 2, 2), (1, 2, 3) }, m.AsValueTuples());
        }
    }
}
