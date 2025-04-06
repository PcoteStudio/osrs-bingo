using BingoBackend.Data.Constants;
using BingoBackend.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BingoBackend.Data;

public class DbSeeder(
    UserManager<UserEntity> userManager,
    RoleManager<IdentityRole> roleManager,
    IWebHostEnvironment environment,
    ILogger logger)
{
    private async Task CreateRoleIfNotExistsAsync(string role)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var createRoleResult = await roleManager.CreateAsync(new IdentityRole(role));
            if (!createRoleResult.Succeeded)
                throw new Exception($"Failed to create {role} role. Errors: {string.Join(",",
                    createRoleResult.Errors.Select(e => e.Description))}");
        }
    }

    private async Task AddRoleToUserAsync(UserEntity user, string role)
    {
        var addRoleToUserResult = await userManager.AddToRoleAsync(user, role);
        if (!addRoleToUserResult.Succeeded)
            throw new Exception($"Failed to add admin role to user. Errors: {string.Join(",",
                addRoleToUserResult.Errors.Select(e => e.Description))}");
    }

    private async Task<UserEntity?> CreateDevelopmentUser(string role, string email, string name, string password)
    {
        if (!environment.IsDevelopment()) return null;
        var user = new UserEntity
        {
            Name = name,
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        var createUserResult = await userManager.CreateAsync(user, password);
        if (!createUserResult.Succeeded)
            throw new Exception($"Failed to create {name} user. Errors: {string.Join(",",
                createUserResult.Errors.Select(e => e.Description))}");
        await AddRoleToUserAsync(user, role);
        logger.LogInformation($"Created {name} user with {role} role.");
        return user;
    }

    private async Task CreateRoles()
    {
        await CreateRoleIfNotExistsAsync(Roles.Admin);
        await CreateRoleIfNotExistsAsync(Roles.User);
    }

    public async Task Seed()
    {
        await CreateRoles();

        if (!userManager.Users.Any())
        {
            await CreateDevelopmentUser(Roles.Admin, "admin@local.host", "admin@local.host", "Password1!");
            await CreateDevelopmentUser(Roles.User, "user@local.host", "user@local.host", "Password1!");
        }
    }
}