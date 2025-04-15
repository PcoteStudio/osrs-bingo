using Bingo.Api.Core.Features.Authentication.Arguments;
using Bingo.Api.Data.Entities;
using Bingo.Api.TestUtils.TestDataGenerators;

namespace Bingo.Api.TestUtils.TestDataSetups;

public partial class TestDataSetup
{
    public TestDataSetup AddUser(out UserWithSecrets userWithSecrets)
    {
        userWithSecrets = new UserWithSecrets
        {
            User = new UserEntity
            {
                Username = TestDataGenerator.GenerateUserName(),
                Email = TestDataGenerator.GenerateUserName(),
                EmailConfirmed = true
            },
            Password = TestDataGenerator.GenerateUserPassword()
        };
        userWithSecrets.User.HashedPassword = passwordHasher.HashPassword(
            userWithSecrets.User, userWithSecrets.Password
        );
        SaveEntity(userWithSecrets.User);

        // Login user (with low iteration count configured)
        var TODO = authService.LoginAsync(new AuthLoginArguments
        {
            Username = userWithSecrets.User.Username,
            Password = userWithSecrets.Password
        }).Result;

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
    public string Password { get; set; } = string.Empty;
}