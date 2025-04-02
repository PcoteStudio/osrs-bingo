using System.Linq.Expressions;
using BingoBackend.Data;
using Microsoft.EntityFrameworkCore;

namespace BingoBackend.Core.Features.Generic;

public interface IGenericRepository<T> where T : class
{
    ValueTask<T?> GetById(int id);
    T GetRequiredById(int id);
    Task<List<T>> GetAll();
    Task<List<T>> Find(Expression<Func<T, bool>> expression);
    void Add(T entity);
    void AddMultiple(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveMultiple(IEnumerable<T> entities);
}

public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T>
    where T : class
{
    protected readonly ApplicationDbContext Context = context;

    public void Add(T entity)
    {
        Context.Set<T>().Add(entity);
    }

    public void AddMultiple(IEnumerable<T> entities)
    {
        Context.Set<T>().AddRange(entities);
    }

    public Task<List<T>> Find(Expression<Func<T, bool>> expression)
    {
        return Context.Set<T>().Where(expression).ToListAsync();
    }

    public Task<List<T>> GetAll()
    {
        return Context.Set<T>().ToListAsync();
    }

    public ValueTask<T?> GetById(int id)
    {
        return Context.Set<T>().FindAsync(id);
    }

    public T GetRequiredById(int id)
    {
        return Context.Set<T>().Find(id) ??
               throw new Exception($"Required entity of type {typeof(T)} not found for id {id}");
    }

    public void Remove(T entity)
    {
        Context.Set<T>().Remove(entity);
    }

    public void RemoveMultiple(IEnumerable<T> entities)
    {
        Context.Set<T>().RemoveRange(entities);
    }
}