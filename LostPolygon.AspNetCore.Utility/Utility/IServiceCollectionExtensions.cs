using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LostPolygon.AspNetCore.Utility {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddOptionsAndValidate<TOptions, TValidator>(this IServiceCollection services, IConfiguration configuration)
            where TOptions : class, new()
            where TValidator : class, IValidateOptions<TOptions>, new() {

            services
                .AddOptions<TOptions>()
                .Bind(configuration);

            services.AddSingleton<IValidateOptions<TOptions>, TValidator>();

            // Hack to enable eager validation in cooperation with service pre-building
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<TOptions>>().Value);

            return services;
        }
    }
}
