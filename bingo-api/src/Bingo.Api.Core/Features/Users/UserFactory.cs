using Bingo.Api.Core.Features.Authentication.Arguments;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Users;

public interface IUserFactory
{
    UserEntity Create(AuthSignupArguments args);
}

public class UserFactory(IUserUtil userUtil) : IUserFactory
{
    public UserEntity Create(AuthSignupArguments args)
    {
        return new UserEntity
        {
            Email = args.Email,
            EmailNormalized = userUtil.NormalizeEmail(args.Email),
            Username = args.Username,
            UsernameNormalized = userUtil.NormalizeEmail(args.Username),
            EmailConfirmed = true,
            Permissions = []
        };
    }
}