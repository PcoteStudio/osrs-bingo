using BingoBackend.Core.Features.Authentication.Arguments;
using BingoBackend.Data.Entities;

namespace BingoBackend.Core.Features.Users;

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
            UserName = args.Email,
            Name = args.Name,
            EmailConfirmed = true
        };
    }
}