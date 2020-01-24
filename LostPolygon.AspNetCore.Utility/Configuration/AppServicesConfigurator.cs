using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LostPolygon.AspNetCore.Utility {
    public abstract class AppServicesConfigurator<T> where T : ITuple {
        protected IServiceCollection Services { get; }
        protected IConfiguration Configuration { get; }

        protected AppServicesConfigurator(IServiceCollection services, IConfiguration configuration) {
            Services = services;
            Configuration = configuration;
        }

        public abstract T Configure();
    }
}
