using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LostPolygon.AspNetCore.Utility;

public abstract class ScopedBackgroundService : BackgroundService {
    protected IServiceScopeFactory ServiceScopeFactory { get; }

    protected ScopedBackgroundService(IServiceScopeFactory serviceScopeFactory) {
        ServiceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        using IServiceScope scope = ServiceScopeFactory.CreateScope();
        IServiceProvider serviceProvider = scope.ServiceProvider;
        GetServices(serviceProvider);
        await DoWork(stoppingToken);
    }

    protected abstract void GetServices(IServiceProvider serviceProvider);

    protected abstract Task DoWork(CancellationToken stoppingToken);
}
