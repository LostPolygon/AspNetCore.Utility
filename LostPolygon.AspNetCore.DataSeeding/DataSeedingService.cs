using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LostPolygon.AspNetCore.DataSeeding;

public class DataSeedingService(
    IServiceProvider serviceProvider,
    ILogger<DataSeedingService> logger,
    IOptions<DataSeedingConfiguration> dataSeedingConfiguration
) : IHostedService {
    public async Task StartAsync(CancellationToken cancellationToken) {
        if (dataSeedingConfiguration.Value.DataSeeders == null)
            return;

        using IServiceScope serviceScope = serviceProvider.CreateScope();
        foreach (string dataSeederTypeName in dataSeedingConfiguration.Value.DataSeeders) {
            if (String.IsNullOrWhiteSpace(dataSeederTypeName))
                continue;

            Type? dataSeederType = Type.GetType(dataSeederTypeName);
            if (dataSeederType == null)
                throw new OptionsValidationException(
                    nameof(DataSeedingConfiguration.DataSeeders),
                    typeof(DataSeedingConfiguration),
                    [$"Data seeder '{dataSeederTypeName}' not found"]
                );

            if (!typeof(IDataSeeder).IsAssignableFrom(dataSeederType))
                throw new OptionsValidationException(
                    nameof(DataSeedingConfiguration.DataSeeders),
                    typeof(DataSeedingConfiguration),
                    [$"Data seeder '{dataSeederTypeName}' must implement {nameof(IDataSeeder)}"]
                );

            logger.LogInformation("Running data seeder '{DataSeederTypeName}'.", dataSeederTypeName);
            IDataSeeder dataSeeder = (IDataSeeder) ActivatorUtilities.CreateInstance(serviceScope.ServiceProvider, dataSeederType);
            await dataSeeder.Seed(cancellationToken);
            logger.LogInformation("Completed data seeder '{DataSeederTypeName}'.", dataSeederTypeName);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
