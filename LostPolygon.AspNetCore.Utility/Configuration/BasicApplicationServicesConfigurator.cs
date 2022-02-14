using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LostPolygon.AspNetCore.Utility;

public abstract class BasicApplicationServicesConfigurator<TConfigureResult> {
    protected IServiceCollection Services { get; }
    protected IConfiguration Configuration { get; }

    protected BasicApplicationServicesConfigurator(IServiceCollection services, IConfiguration configuration) {
        Services = services;
        Configuration = configuration;
    }

    public abstract TConfigureResult Configure();
}
