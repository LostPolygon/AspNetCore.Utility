using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace LostPolygon.EntityFrameworkCore; 

public abstract class DbContextConfigurationExtensionBase : IDbContextConfigurationExtension {
    protected DbContextConfigurationExtensionBase() {
    }

    public abstract void OnModelCreating(DbContext dbContext, ModelBuilder builder);

    public abstract void OnConfiguring(DbContext dbContext);

    public void ApplyServices(IServiceCollection services) {
    }

    public void Validate(IDbContextOptions options) {
    }

    public DbContextOptionsExtensionInfo Info => new DbContextOptionsExtensionInfoInternal(this);

    private class DbContextOptionsExtensionInfoInternal : DbContextOptionsExtensionInfo {
        public DbContextOptionsExtensionInfoInternal(IDbContextOptionsExtension extension) : base(extension) {
        }

        public override int GetServiceProviderHashCode() {
            return 0;
        }

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) {
            return true;
        }

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo) {
        }

        public override bool IsDatabaseProvider => false;

        public override string LogFragment => nameof(DbContextConfigurationExtensionBase);
    }
}