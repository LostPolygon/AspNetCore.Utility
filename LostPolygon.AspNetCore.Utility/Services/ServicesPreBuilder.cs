using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LostPolygon.AspNetCore.Utility {
    public class ServicesPreBuilderService : IHostedService {
        private readonly IServiceCollection _services;
        private readonly IServiceProvider _serviceProvider;

        public ServicesPreBuilderService(IServiceCollection services, IServiceProvider serviceProvider) {
            _services = services;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken) {
            IEnumerable<Type> types = GetSingletons();
            foreach (Type type in types) {
                if (type == typeof(DbContextOptions))
                    continue;

                _serviceProvider.GetService(type);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private IEnumerable<Type> GetSingletons() {
            return _services
                .Where(descriptor => descriptor.Lifetime == ServiceLifetime.Singleton)
                .Where(descriptor => descriptor.ImplementationType != GetType())
                .Where(descriptor => descriptor.ServiceType.ContainsGenericParameters == false)
                .Select(descriptor => descriptor.ServiceType)
                .Distinct();
        }
    }
}
