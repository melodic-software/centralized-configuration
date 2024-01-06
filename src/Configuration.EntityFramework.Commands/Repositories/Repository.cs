using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configuration.EntityFramework.DbContexts.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Configuration.EntityFramework.Commands.Repositories
{
    // NOTE: A constraint could be added so that this only works with domain entity base abstractions.
    // The choice depends on if the db context models are being kept separate from the domain models.

    internal abstract class Repository<T>(ConfigurationContext dbContext)
        where T : class
    {
        protected readonly ConfigurationContext DbContext = dbContext;

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
            //return await DbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public void Add(T entity)
        {
            DbContext.Add(entity);
        }
    }
}
