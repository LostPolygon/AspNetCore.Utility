using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace LostPolygon.EntityFrameworkCore {
    public static class DatabaseExtensions {
        public static T AddInMemoryDbContext<T>(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder>? dbContextOptionsConfigureAction = null
            ) where T : DbContext {
            return AddSqliteDbContext<T>(services, "DataSource=:memory:", dbContextOptionsConfigureAction);
        }

        public static T AddSqliteDbContext<T>(
            this IServiceCollection services,
            string connectionString,
            Action<DbContextOptionsBuilder>? dbContextOptionsConfigureAction = null
            ) where T : DbContext {
            SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();
            services.AddDbContext<T>(options => {
                options.UseSqlite(connection);
                dbContextOptionsConfigureAction?.Invoke(options);
            });
            T dbContext = services.BuildServiceProvider().GetService<T>();
            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        public static void AddOrUpdateExtension(this DbContextOptionsBuilder optionsBuilder, IDbContextOptionsExtension extension) {
            ((IDbContextOptionsBuilderInfrastructure) optionsBuilder).AddOrUpdateExtension(extension);
        }

        public static void DetachAllEntries(this DbContext context) {
            context.SaveChanges();
            List<EntityEntry> changedEntriesCopy = context.ChangeTracker.Entries().ToList();
            foreach (EntityEntry entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }
    }
}
