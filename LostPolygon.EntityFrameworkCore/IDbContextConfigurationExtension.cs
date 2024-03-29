using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LostPolygon.EntityFrameworkCore; 

public interface IDbContextConfigurationExtension : IDbContextOptionsExtension {
    void OnModelCreating(DbContext dbContext, ModelBuilder builder);
    void OnConfiguring(DbContext dbContext);
}