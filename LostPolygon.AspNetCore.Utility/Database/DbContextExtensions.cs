using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Ballast.Atlantis {
    public static class DbContextExtensions {
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
