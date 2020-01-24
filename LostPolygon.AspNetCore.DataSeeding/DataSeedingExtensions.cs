using System;
using Microsoft.Extensions.DependencyInjection;

namespace LostPolygon.AspNetCore.DataSeeding {
    public static class DataSeedingExtensions {
        public static void AddDataSeeding(this IServiceCollection services, Action<DataSeedingOptions> configureOptions) {
            services.AddOptions<DataSeedingOptions>().Configure(configureOptions);
            services.AddHostedService<DataSeedingService>();
        }
    }
}
