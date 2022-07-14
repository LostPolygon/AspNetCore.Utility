using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace LostPolygon.AspNetCore.DataSeeding;

public class DataSeedingService : IHostedService {
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<DataSeedingConfiguration> _dataSeedingConfiguration;

    public DataSeedingService(IServiceProvider serviceProvider, IOptions<DataSeedingConfiguration> dataSeedingConfiguration) {
        _serviceProvider = serviceProvider;
        _dataSeedingConfiguration = dataSeedingConfiguration;
    }

    public async Task StartAsync(CancellationToken cancellationToken) {
        if (_dataSeedingConfiguration.Value.DataSeeders == null)
            return;

        using IServiceScope serviceScope = _serviceProvider.CreateScope();
        foreach (string dataSeederTypeName in _dataSeedingConfiguration.Value.DataSeeders) {
            if (String.IsNullOrWhiteSpace(dataSeederTypeName))
                continue;

            Type? dataSeederType = Type.GetType(dataSeederTypeName);
            if (dataSeederType == null)
                throw new OptionsValidationException(
                    nameof(DataSeedingConfiguration.DataSeeders),
                    typeof(DataSeedingConfiguration),
                    new[] { $"Data seeder '{dataSeederTypeName}' not found" }
                );

            if (!typeof(IDataSeeder).IsAssignableFrom(dataSeederType))
                throw new OptionsValidationException(
                    nameof(DataSeedingConfiguration.DataSeeders),
                    typeof(DataSeedingConfiguration),
                    new[] { $"Data seeder '{dataSeederTypeName}' must implement {nameof(IDataSeeder)}" }
                );

            IDataSeeder dataSeeder = (IDataSeeder) ActivatorUtilities.CreateInstance(serviceScope.ServiceProvider, dataSeederType);
            await dataSeeder.Seed(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
