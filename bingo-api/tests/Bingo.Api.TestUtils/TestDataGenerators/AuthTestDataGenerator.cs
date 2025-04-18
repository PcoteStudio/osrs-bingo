using Bingo.Api.Core.Features.Authentication.Arguments;

namespace Bingo.Api.TestUtils.TestDataGenerators;

public static partial class TestDataGenerator
{
    public static AuthSignupArguments GenerateAuthSignupArguments(Action<AuthSignupArguments>? customizer = null)
    {
        var args = new AuthSignupArguments
        {
            Email = GenerateUserEmail(),
            Password = GenerateUserPassword(),
            Username = GenerateUserName()
        };
        customizer?.Invoke(args);
        return args;
    }
}