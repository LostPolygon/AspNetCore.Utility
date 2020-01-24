using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Ballast.Atlantis {
    public interface IDbContextConfigurationExtension : IDbContextOptionsExtension {
        void OnModelCreating(DbContext dbContext, ModelBuilder builder);
        void OnConfiguring(DbContext dbContext);
    }
}
