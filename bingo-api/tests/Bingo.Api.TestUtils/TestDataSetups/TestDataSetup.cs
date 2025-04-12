using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Bingo.Api.TestUtils.TestDataSetups;

public partial class TestDataSetup(
    ApplicationDbContext dbContext,
    UserManager<UserEntity> userManager,
    RoleManager<IdentityRole> roleManager,
    IAuthService authService)
{
    private readonly List<object> _allEntities = [];

    public T GetRequiredLast<T>() where T : class
    {
        var last = _allEntities.OfType<T>().LastOrDefault();
        if (last == null)
            throw new Exception($"Unable to find an element of type {typeof(T).Name}. " +
                                $"Did you call Add{typeof(T).Name.Replace("Entity", "")}() first?");
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