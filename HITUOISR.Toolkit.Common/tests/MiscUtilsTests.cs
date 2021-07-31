using System;
using System.Threading.Tasks;
using Xunit;

namespace HITUOISR.Toolkit.Common.Tests
{
    public class MiscUtilsTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(TypeCode.Decimal)]
        [InlineData("lorem ipsum")]
        public void Identity_AsExpected<T>(T value) => Assert.Equal(value, MiscUtils.Identity(value));

        [Theory]
        [MemberData(nameof(DisposableTestData))]
        internal async Task DisposeAsyncTest(IDisposableTest disposable)
        {
            Assert.False(disposable.Disposed, $"{disposable} should not be disposed before.");
            await MiscUtils.DisposeAsync(disposable);
            Assert.True(disposable.Disposed, $"{disposable} should be disposed.");
        }

        private static TheoryData<IDisposableTest> DisposableTestData() => new()
        {
            new Disposable(),
            new AsyncDisposable(),
        };

        internal interface IDisposableTest
        {
            bool Disposed { get; }
        }

        private sealed class Disposable : IDisposable, IDisposableTest
        {
            public bool Disposed { get; private set; }

            public void Dispose() => Disposed = true;
        }

        private sealed class AsyncDisposable : IDisposable, IAsyncDisposable, IDisposableTest
        {
            public bool Disposed { get; private set; }

            public void Dispose() => DisposeAsync().AsTask().GetAwaiter().GetResult();

            public async ValueTask DisposeAsync()
            {
                await Task.Delay(TimeSpan.FromMilliseconds(10));
                Disposed = true;
            }
        }
    }
}
