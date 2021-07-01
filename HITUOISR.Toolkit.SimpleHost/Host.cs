using HITUOISR.Toolkit.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HITUOISR.Toolkit.SimpleHost
{
    internal sealed class Host : IHost, IAsyncDisposable
    {
        private readonly ILogger<Host> _logger;

        public Host(IServiceProvider services, ILogger<Host> logger)
        {
            Services = services;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        public void Dispose() => DisposeAsync().AsTask().GetAwaiter().GetResult();

        public async ValueTask DisposeAsync() => await MiscUtils.DisposeAsync(Services).ConfigureAwait(false);
    }
}
