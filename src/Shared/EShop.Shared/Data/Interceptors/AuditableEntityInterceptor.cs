using EShop.Shared.DDD;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Shared.Data.Interceptors
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(DbContext? context)
        {
            if (context is null) return;

            foreach (var entry in context.ChangeTracker.Entries<IEntity>())
            {
                if (entry.State is EntityState.Added)
                {
                    entry.Entity.CreatedBy = "Ben";
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }

                if (entry is { State: EntityState.Added or EntityState.Modified }
                   || entry.HasChangedOwnedEntities())
                {
                    entry.Entity.LastModifiedBy = "Ben";
                    entry.Entity.LastModified = DateTime.UtcNow;
                }
            }
        }
    }
    public static class Extensions
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entityEntry)
        {
            return entityEntry.References.Any(r =>
            r.TargetEntry is EntityEntry tr
            && tr.Metadata.IsOwned()
            && tr is { State: EntityState.Added or EntityState.Modified });
        }
    }
}
