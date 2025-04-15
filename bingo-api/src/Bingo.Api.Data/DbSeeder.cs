using Bingo.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bingo.Api.Data;

public class DbSeeder(
    DbContext dbContext,
    IPasswordHasher<UserEntity> passwordHasher,
    IHostEnvironment environment,
    ILogger logger)
{
    private async Task<UserEntity?> CreateDevelopmentUserAsync(string email, string username,
        string password, List<string> permissions)
    {
        if (!environment.IsDevelopment()) return null;
        var user = new UserEntity
        {
            Username = username,
            Email = email,
            EmailConfirmed = true,
            Permissions = permissions
        };
        user.HashedPassword = passwordHasher.HashPassword(user, password);
        await dbContext.Set<UserEntity>().AddAsync(user);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Created {username} user with following permissions: {permissions}.",
            username,
            string.Join(", ", permissions)
        );
        return user;
    }

    public async Task SeedAsync()
    {
        if (!await dbContext.Set<UserEntity>().AnyAsync())
        {
            await CreateDevelopmentUserAsync("admin@local.host", "Admin", "Password1!", ["*"]);
            await CreateDevelopmentUserAsync("user@local.host", "User", "Password1!", []);
        }
    }
}