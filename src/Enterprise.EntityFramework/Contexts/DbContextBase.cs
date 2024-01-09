using Enterprise.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.EntityFramework.Contexts;

public abstract class DbContextBase : DbContext
{
    protected DbContextBase(DbContextOptions options) : base(options)
    {
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", ex);
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            int result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", ex);
        }
    }

    public override int SaveChanges()
    {
        try
        {
            int result = base.SaveChanges();

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", ex);
        }
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        try
        {
            int result = base.SaveChanges(acceptAllChangesOnSuccess);

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", ex);
        }
    }
}