using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ballast.Atlantis.Utility {
    public abstract class ScopedBackgroundService : BackgroundService {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        protected ScopedBackgroundService(IServiceScopeFactory serviceScopeFactory) {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;
            GetServices(serviceProvider);
            await DoWork(stoppingToken);
        }

        protected abstract void GetServices(IServiceProvider serviceProvider);

        protected abstract Task DoWork(CancellationToken stoppingToken);
    }
}
