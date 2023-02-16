using System;
using Microsoft.Extensions.DependencyInjection;

namespace LostPolygon.AspNetCore.DataSeeding; 

public static class DataSeedingExtensions {
    public static void AddDataSeeding(this IServiceCollection services, Action<DataSeedingConfiguration> configureOptions) {
        services.AddOptions<DataSeedingConfiguration>().Configure(configureOptions);
        services.AddHostedService<DataSeedingService>();
    }
}