namespace Enterprise.DesignPatterns.UnitOfWork;

public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}