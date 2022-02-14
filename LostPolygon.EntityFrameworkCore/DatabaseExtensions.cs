using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace LostPolygon.EntityFrameworkCore; 

public static class DatabaseExtensions {
    public static void AddInMemorySqliteDbContext<T>(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder>? dbContextOptionsConfigureAction = null
    ) where T : DbContext {
        AddSqliteDbContext<T>(services, "DataSource=:memory:", true, dbContextOptionsConfigureAction);
    }

    public static void AddSqliteDbContext<T>(
        this IServiceCollection services,
        string connectionString,
        Action<DbContextOptionsBuilder>? dbContextOptionsConfigureAction = null
    ) where T : DbContext {
        AddSqliteDbContext<T>(services, connectionString, false, dbContextOptionsConfigureAction);
    }

    private static void AddSqliteDbContext<T>(
        this IServiceCollection services,
        string connectionString,
        bool singleConnection,
        Action<DbContextOptionsBuilder>? dbContextOptionsConfigureAction = null
    ) where T : DbContext {
        SqliteConnection connection = null!;
        if (singleConnection) {
            connection = new SqliteConnection(connectionString);
            connection.Open();
        }

        services.AddDbContext<T>(options => {
            if (singleConnection) {
                options.UseSqlite(connection!);
            }
            else {
                options.UseSqlite(connectionString);
            }

            dbContextOptionsConfigureAction?.Invoke(options);
        });
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