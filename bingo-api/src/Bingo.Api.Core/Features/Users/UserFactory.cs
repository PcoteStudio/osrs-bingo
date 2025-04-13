using Bingo.Api.Core.Features.Authentication.Arguments;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Users;

public interface IUserFactory
{
    UserEntity Create(AuthSignupArguments args);
}

public class UserFactory : IUserFactory
{
    public UserEntity Create(AuthSignupArguments args)
    {
        return new UserEntity
        {
            Email = args.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = args.Username,
            EmailConfirmed = true
        };
    }
}