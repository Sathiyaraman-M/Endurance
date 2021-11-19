namespace Quark.Core.Interfaces.Repositories;

public interface IRepositoryAsync<T, in TId> where T : class, IEntity<TId>
{
    IQueryable<T> Entities { get; }

    Task<T> GetByIdAsync(TId id);

    Task<List<T>> GetAllAsync();

    Task<List<TModel>> GetAllAsync<TModel>(Expression<Func<T, TModel>> map);

    Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize);

    Task<T> AddAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);

    Task<int> CountAsync();
}