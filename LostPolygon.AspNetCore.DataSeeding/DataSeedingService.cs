using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace LostPolygon.AspNetCore.DataSeeding; 

public class DataSeedingService : IHostedService {
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<DataSeedingOptions> _dataSeedingOptions;

    public DataSeedingService(IServiceProvider serviceProvider, IOptions<DataSeedingOptions> dataSeedingOptions) {
        _serviceProvider = serviceProvider;
        _dataSeedingOptions = dataSeedingOptions;
    }

    public async Task StartAsync(CancellationToken cancellationToken) {
        using IServiceScope serviceScope = _serviceProvider.CreateScope();
        foreach (string dataSeederTypeName in _dataSeedingOptions.Value.DataSeeders) {
            if (String.IsNullOrWhiteSpace(dataSeederTypeName))
                continue;

            Type? dataSeederType = Type.GetType(dataSeederTypeName);
            if (dataSeederType == null)
                throw new OptionsValidationException(
                    nameof(DataSeedingOptions.DataSeeders),
                    typeof(DataSeedingOptions),
                    new[] { $"Data seeder '{dataSeederTypeName}' not found" }
                );

            if (!typeof(IDataSeeder).IsAssignableFrom(dataSeederType))
                throw new OptionsValidationException(
                    nameof(DataSeedingOptions.DataSeeders),
                    typeof(DataSeedingOptions),
                    new[] { $"Data seeder '{dataSeederTypeName}' must implement {nameof(IDataSeeder)}" }
                );

            IDataSeeder dataSeeder = (IDataSeeder) ActivatorUtilities.CreateInstance(serviceScope.ServiceProvider, dataSeederType);
            await dataSeeder.Seed(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}