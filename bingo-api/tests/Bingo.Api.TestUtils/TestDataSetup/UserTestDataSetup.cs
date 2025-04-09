using Bingo.Api.Data.Constants;
using Bingo.Api.Data.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;

namespace Bingo.Api.TestUtils.TestDataSetup;

public partial class TestDataSetup
{
    public class AddUserArguments
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<Roles> Roles { get; set; } = [];
    }
    
    public TestDataSetup AddRole(Roles role)
    {
        roleManager.Should().NotBeNull();
        if (roleManager.RoleExistsAsync(role.ToString()).Result)
        {
            var createRoleResult = roleManager.CreateAsync(new IdentityRole(role.ToString())).Result;
            createRoleResult.Succeeded.Should().BeTrue();
        }
        return this;
    }
    
    private void AddRoleToUser(UserEntity user, Roles role)
    {
        userManager.Should().NotBeNull();
        var addRoleToUserResult = userManager.AddToRoleAsync(user, role.ToString()).Result;
        addRoleToUserResult.Succeeded.Should().BeTrue();
    }
    
    public TestDataSetup AddUser(out UserEntity user, AddUserArguments args)
    {
        userManager.Should().NotBeNull();
        user = new UserEntity
        {
            Name = args.Name,
            UserName = args.Email,
            Email = args.Email,
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        var createUserResult = userManager.CreateAsync(user, args.Password).Result;
        createUserResult.Succeeded.Should().BeTrue();
        foreach (var role in args.Roles)
            AddRoleToUser(user, role);
        dbContext.SaveChanges();
        return this;
    }

    
    public TestDataSetup AddUser()
    {
        return AddUser(out _);
    }
    
    public TestDataSetup AddUser(out UserEntity user)
    {
        var args = GenerateAddUserArguments();
        args.Roles.Add(Roles.User);
        return AddUser(out user, args);
    }

    public static AddUserArguments GenerateAddUserArguments()
    {
        return new AddUserArguments()
        {
            Name = GenerateUserName(),
            Email = GenerateUserEmail(),
            Password = GenerateUserPassword(),
        };
    }

    private static string GenerateUserPassword()
    {
        return RandomUtil.GetPrefixedRandomHexString("!Passw0rd", Random.Shared.Next(20, 30));
    }
    
    private static string GenerateUserName()
    {
        return RandomUtil.GetPrefixedRandomHexString("UName_", Random.Shared.Next(5, 25));
    }

    private static string GenerateUserEmail()
    {
        return RandomUtil.GetPrefixedRandomHexString("UMail_", Random.Shared.Next(5, 25)) + "@local.host";
    }
}