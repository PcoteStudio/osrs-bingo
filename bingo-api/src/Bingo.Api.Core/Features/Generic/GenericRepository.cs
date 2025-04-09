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

public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T>
    where T : class
{
    protected readonly ApplicationDbContext Context = context;

    public void Add(T entity)
    {
        Context.Set<T>().Add(entity);
    }

    public void Add(IEnumerable<T> entities)
    {
        Context.Set<T>().AddRange(entities);
    }

    public Task<List<T>> GetAllAsync()
    {
        return Context.Set<T>().ToListAsync();
    }

    public ValueTask<T?> GetByIdAsync(int id)
    {
        return Context.Set<T>().FindAsync(id);
    }

    public async Task<T> GetRequiredByIdAsync(int id)
    {
        return await Context.Set<T>().FindAsync(id) ??
               throw new Exception($"Required entity of type {typeof(T)} not found for id {id}");
    }

    public void Remove(T entity)
    {
        Context.Set<T>().Remove(entity);
    }

    public void Remove(IEnumerable<T> entities)
    {
        Context.Set<T>().RemoveRange(entities);
    }
}