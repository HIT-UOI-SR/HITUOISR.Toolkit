using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace HITUOISR.Toolkit.SimpleHost.Tests
{
    public class HostTests
    {
        [Fact()]
        public void Ctor_Init()
        {
            using NullLoggerFactory loggerFactory = new();
            ServiceCollection services = new();
            var sp = services.BuildServiceProvider();
            using Host host = new(sp, loggerFactory.CreateLogger<Host>());
            Assert.Same(sp, host.Services);
        }

        [Fact()]
        internal async Task DisposeAsync_AsExpected()
        {
            using NullLoggerFactory loggerFactory = new();
            FakeDisposableServiceProvider sp = new();
            Host host = new(sp, loggerFactory.CreateLogger<Host>());
            Assert.False(sp.Disposed, "ServiceProvider has already been disposed.");
            await host.DisposeAsync();
            Assert.True(sp.Disposed, "ServiceProvider hasn't been disposed.");
        }

        [Fact()]
        internal void Dispose_AsExpected()
        {
            using NullLoggerFactory loggerFactory = new();
            FakeDisposableServiceProvider sp = new();
            Host host = new(sp, loggerFactory.CreateLogger<Host>());
            Assert.False(sp.Disposed, "ServiceProvider has already been disposed.");
            host.Dispose();
            Assert.True(sp.Disposed, "ServiceProvider hasn't been disposed.");
        }

        internal class FakeDisposableServiceProvider : IServiceProvider, IDisposable
        {
            public bool Disposed { get; private set; }

            public void Dispose() => Disposed = true;

            public object? GetService(Type serviceType) => null;
        }
    }
}
