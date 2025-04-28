using Bingo.Api.Data.Entities;
using Bingo.Api.TestUtils.TestDataGenerators;

namespace Bingo.Api.TestUtils.TestDataSetups;

public partial class TestDataSetup
{
    public TestDataSetup AddUser(out UserWithSecrets userWithSecrets)
    {
        userWithSecrets = new UserWithSecrets
        {
            User = TestDataGenerator.GenerateUserEntity(),
            Password = TestDataGenerator.GenerateUserPassword()
        };
        userWithSecrets.User.HashedPassword = passwordHasher.HashPassword(
            userWithSecrets.User, userWithSecrets.Password
        );
        SaveEntity(userWithSecrets.User);

        return this;
    }

    public TestDataSetup AddUser()
    {
        return AddUser(out _);
    }

    public TestDataSetup AddPermissions(params string[] permissions)
    {
        var user = GetRequiredLast<UserEntity>();
        user.Permissions.AddRange(permissions);
        dbContext.SaveChanges();
        return this;
    }
}

public class UserWithSecrets
{
    public required UserEntity User { get; set; }
    public string Password { get; set; } = string.Empty;
}