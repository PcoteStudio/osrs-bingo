using Bingo.Api.Data.Entities;
using Bingo.Api.Shared;

namespace Bingo.Api.TestUtils.TestDataGenerators;

public static partial class TestDataGenerator
{
    public static string GenerateUserName()
    {
        return RandomUtil.GetPrefixedRandomHexString("UName_", 5, 20);
    }

    public static string GenerateUserPassword()
    {
        return RandomUtil.GetPrefixedRandomHexString("Passw0rd!", 20, 30);
    }

    public static string GenerateUserEmail()
    {
        return RandomUtil.GetPrefixedRandomHexString("UMail_", 5, 25) + "@local.host";
    }

    public static UserEntity GenerateUserEntity()
    {
        var user = new UserEntity
        {
            Id = Random.Shared.Next(),
            Username = GenerateUserName(),
            Email = GenerateUserEmail(),
            EmailConfirmed = true,
            Permissions = []
        };
        user.UsernameNormalized = user.Username.ToLower();
        user.EmailNormalized = user.Email.ToLower();
        return user;
    }
}