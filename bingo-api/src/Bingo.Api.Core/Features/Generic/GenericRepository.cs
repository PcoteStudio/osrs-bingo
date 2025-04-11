using Bingo.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.Generic;

public interface IGenericRepository<T> where T : class
{
    ValueTask<T?> GetByIdAsync(int id);
    Task<T> GetRequiredByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    void Add(T entity);
    void Add(IEnumerable<T> entities);
    void Remove(T entity);
    void Remove(IEnumerable<T> entities);
}

public class GenericRepository<T>(ApplicationDbContext dbContext) : IGenericRepository<T>
    where T : class
{
    protected readonly ApplicationDbContext DbContext = dbContext;

    public void Add(T entity)
    {
        DbContext.Set<T>().Add(entity);
    }

    public void Add(IEnumerable<T> entities)
    {
        DbContext.Set<T>().AddRange(entities);
    }

    public Task<List<T>> GetAllAsync()
    {
        return DbContext.Set<T>().ToListAsync();
    }

    public ValueTask<T?> GetByIdAsync(int id)
    {
        return DbContext.Set<T>().FindAsync(id);
    }

    public async Task<T> GetRequiredByIdAsync(int id)
    {
        return await DbContext.Set<T>().FindAsync(id) ??
               throw new Exception($"Required entity of type {typeof(T)} not found for id {id}");
    }

    public void Remove(T entity)
    {
        DbContext.Set<T>().Remove(entity);
    }

    public void Remove(IEnumerable<T> entities)
    {
        DbContext.Set<T>().RemoveRange(entities);
    }
}