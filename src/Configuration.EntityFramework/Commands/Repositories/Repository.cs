using Configuration.EntityFramework.DbContexts.Configuration;
using Enterprise.DomainDrivenDesign.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Configuration.EntityFramework.Commands.Repositories;

// NOTE: A constraint could be added so that this only works with domain entity base abstractions.
// The choice depends on if the db context models are being kept separate from the domain models.

internal abstract class Repository<TEntity, TEntityId> where TEntity : Entity<TEntityId>
    where TEntityId : class, IEquatable<TEntityId>
{
    protected readonly ConfigurationContext DbContext;

    protected Repository(ConfigurationContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public void Add(TEntity entity)
    {
        DbContext.Add(entity);
    }
}