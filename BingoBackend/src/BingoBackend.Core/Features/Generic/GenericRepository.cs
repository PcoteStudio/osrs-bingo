using System.Linq.Expressions;
using BingoBackend.Data;

namespace BingoBackend.Core.Features.Generic;

public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T>
    where T : class
{
    protected readonly ApplicationDbContext Context = context;

    public T Add(T entity)
    {
        return Context.Set<T>().Add(entity).Entity;
    }

    public void AddMultiple(IEnumerable<T> entities)
    {
        Context.Set<T>().AddRange(entities);
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return Context.Set<T>().Where(expression);
    }

    public IEnumerable<T> GetAll()
    {
        return Context.Set<T>().ToList();
    }

    public T? GetById(int id)
    {
        return Context.Set<T>().Find(id);
    }

    public T GetRequiredById(int id)
    {
        return Context.Set<T>().Find(id) ?? throw new Exception();
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