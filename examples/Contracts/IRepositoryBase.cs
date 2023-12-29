using System.Linq.Expressions;

namespace Contracts;

// NOTE: This has some leaked abstraction with "trackChanges".
// Also, not everything is going to support IQueryable<T>, which in itself exposes too much power to the client.
// It allows for data access / queries to be made outside the repository reference and implementation.
// Do not use this in real solutions.

public interface IRepositoryBase<T>
{
    IQueryable<T> FindAll(bool trackChanges);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}