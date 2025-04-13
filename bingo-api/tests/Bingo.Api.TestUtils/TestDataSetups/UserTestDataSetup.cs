using Bingo.Api.Core.Features.Authentication.Arguments;
using Bingo.Api.Data.Constants;
using Bingo.Api.Data.Entities;
using Bingo.Api.TestUtils.TestDataGenerators;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;

namespace Bingo.Api.TestUtils.TestDataSetups;

public partial class TestDataSetup
{
    public TestDataSetup CreateRole(Roles role)
    {
        roleManager.Should().NotBeNull();
        if (!roleManager.RoleExistsAsync(role.ToString()).Result)
        {
            var createRoleResult = roleManager.CreateAsync(new IdentityRole(role.ToString())).Result;
            createRoleResult.Succeeded.Should().BeTrue();
        }

        return this;
    }

    private void AddRoleToUser(UserEntity user, Roles role)
    {
        CreateRole(role);
        var addRoleToUserResult = userManager.AddToRoleAsync(user, role.ToString()).Result;
        addRoleToUserResult.Succeeded.Should().BeTrue();
    }

    public void AddRole(Roles role = Roles.User)
    {
        var user = GetRequiredLast<UserEntity>();
        AddRoleToUser(user, role);
        dbContext.SaveChanges();
    }

    public TestDataSetup AddUser(out UserWithSecrets userWithSecrets)
    {
        userWithSecrets = new UserWithSecrets
        {
            User = new UserEntity
            {
                UserName = TestDataGenerator.GenerateUserName(),
                Email = TestDataGenerator.GenerateUserName(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            },
            Password = TestDataGenerator.GenerateUserPassword()
        };
        var createUserResult = userManager.CreateAsync(userWithSecrets.User, userWithSecrets.Password).Result;
        createUserResult.Succeeded.Should().BeTrue();
        _allEntities.Add(userWithSecrets.User);
        dbContext.SaveChanges();

        // Login user (with low iteration count configured)
        var tokens = authService.LoginAsync(new AuthLoginArguments
        {
            Username = userWithSecrets.User.UserName,
            Password = userWithSecrets.Password
        }).Result;
        userWithSecrets.AccessToken = tokens.AccessToken;
        userWithSecrets.RefreshToken = tokens.RefreshToken;

        return this;
    }

    public TestDataSetup AddUser()
    {
        return AddUser(out _);
    }
}

public class UserWithSecrets
{
    public required UserEntity User { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}