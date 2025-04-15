using Bingo.Api.Data.Entities;
using Bingo.Api.Shared;

namespace Bingo.Api.TestUtils.TestDataGenerators;

public static partial class TestDataGenerator
{
    public static string GenerateUserName()
    {
        return RandomUtil.GetPrefixedRandomHexString("UName_", Random.Shared.Next(5, 25));
    }

    public static string GenerateUserPassword()
    {
        return RandomUtil.GetPrefixedRandomHexString("Passw0rd!", Random.Shared.Next(20, 30));
    }

    public static string GenerateUserEmail()
    {
        return RandomUtil.GetPrefixedRandomHexString("UMail_", Random.Shared.Next(5, 25)) + "@local.host";
    }

    public static UserEntity GenerateUserEntity()
    {
        return new UserEntity
        {
            Id = Guid.NewGuid().ToString(),
            UserName = GenerateUserName(),
            Email = GenerateUserEmail()
        };
    }
}