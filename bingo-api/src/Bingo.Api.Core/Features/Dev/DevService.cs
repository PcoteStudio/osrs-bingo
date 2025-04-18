using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.Dev;

public interface IDevService
{
    Task<EventEntity> SeedEventAsync();
    Task DropDatabaseAsync();
}

public class DevService(
    IDbSeeder dbSeeder,
    ApplicationDbContext dbContext
) : IDevService
{
    public Task<EventEntity> SeedEventAsync()
    {
        throw new NotImplementedException();
    }

    public async Task DropDatabaseAsync()
    {
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.MigrateAsync();
        dbContext.ChangeTracker.Clear();

        await dbSeeder.SeedDevUsersAsync();
    }
}