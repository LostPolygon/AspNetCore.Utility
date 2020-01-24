using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ballast.Atlantis {
    public static class DatabaseUtility {
        public static T AddInMemoryDbContext<T>(
            IServiceCollection services,
            Action<DbContextOptionsBuilder>? dbContextOptionsConfigureAction = null
            ) where T : DbContext {
            return AddSqliteDbContext<T>(services, "DataSource=:memory:", dbContextOptionsConfigureAction);
        }

        public static T AddSqliteDbContext<T>(
            IServiceCollection services,
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
    }
}
