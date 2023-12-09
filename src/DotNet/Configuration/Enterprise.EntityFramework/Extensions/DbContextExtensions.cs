using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Enterprise.EntityFramework.Extensions;

public static class DbContextExtensions
{
    public static T? GetShadowPropertyValue<T>(this DbContext dbContext, object entity, string propertyName) where T : IConvertible
    {
        object? value = GetShadowPropertyValue(dbContext, entity, propertyName);

        T? result = value != null ?
            (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture) :
            default;

        return result;
    }

    public static object? GetShadowPropertyValue(this DbContext dbContext, object entity, string propertyName)
    {
        object? value;

        if (entity is EntityEntry entry)
        {
            value = entry.Property(propertyName).CurrentValue;
        }
        else
        {
            value = dbContext.Entry(entity).Property(propertyName).CurrentValue;
        }

        return value;
    }

    public static void UpdateAuditShadowProperties(this DbContext dbContext)
    {
        foreach (EntityEntry entry in dbContext.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added && entry.Properties.Any(x => x.Metadata.Name == "DateCreated"))
            {
                DateTime currentDateCreatedValue = dbContext.GetShadowPropertyValue<DateTime>(entry, "DateCreated");
                PropertyEntry dateCreatedPropertyEntry = entry.Property("DateCreated");
                dateCreatedPropertyEntry.CurrentValue ??= DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified && entry.Properties.Any(x => x.Metadata.Name == "DateModified"))
            {
                object? currentDateModifiedValue = dbContext.GetShadowPropertyValue(entry, "DateModified");
                PropertyEntry dateModifiedPropertyEntry = entry.Property("DateModified");
                dateModifiedPropertyEntry.CurrentValue ??= DateTime.UtcNow;
            }
        }
    }
}