using System.Linq.Expressions;

namespace Quark.Infrastructure.Repositories;

public class RepositoryAsync<T, TId> : IRepositoryAsync<T, TId> where T : AuditableEntity<TId>
{
    private readonly LibraryDbContext _dbContext;

    public RepositoryAsync(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<T> Entities => _dbContext.Set<T>();

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public async Task<int> CountAsync()
    {
        return await _dbContext.Set<T>().CountAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<List<TModel>> GetAllAsync<TModel>(Expression<Func<T, TModel>> map)
    {
        return await _dbContext.Set<T>().Select(map).ToListAsync();
    }

    public async Task<T> GetByIdAsync(TId id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize)
    {
        return await _dbContext.Set<T>().Skip((pageNumber - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        T old = await _dbContext.Set<T>().FindAsync(entity.Id);
        _dbContext.Entry(old).CurrentValues.SetValues(entity);
        await Task.CompletedTask;
    }
}