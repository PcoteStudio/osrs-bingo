using System.Linq.Expressions;

namespace BingoBackend.Core.Features.Generic;

public interface IGenericRepository<T> where T : class
{
    T? GetById(int id);
    T GetRequiredById(int id);
    IEnumerable<T> GetAll();
    IEnumerable<T> Find(Expression<Func<T, bool>> expression);
    T Add(T entity);
    void AddMultiple(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveMultiple(IEnumerable<T> entities);
}