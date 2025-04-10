using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Bingo.Api.TestUtils.TestDataSetups;

public partial class TestDataSetup(
    ApplicationDbContext dbContext,
    UserManager<UserEntity>? userManager = null,
    RoleManager<IdentityRole>? roleManager = null)
{
    private readonly List<object> _allEntities = [];

    public T GetRequiredLast<T>()
    {
        var last = _allEntities.OfType<T>().LastOrDefault();
        if (last == null)
            throw new Exception($"Unable to find an element of type {typeof(T).Name}. " +
                                $"Did you call Add{typeof(T).Name}() first?");
        return last;
    }

    public T? GetLast<T>()
    {
        return _allEntities.OfType<T>().LastOrDefault();
    }

    private TestDataSetup SaveEntity<TEntity>(TEntity entity, Action<TEntity>? customizer = null) where TEntity : class
    {
        customizer?.Invoke(entity);
        _allEntities.Add(entity);
        dbContext.Add(entity);
        dbContext.SaveChanges();
        return this;
    }
}