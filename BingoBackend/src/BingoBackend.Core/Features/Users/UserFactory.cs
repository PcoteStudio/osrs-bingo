using BingoBackend.Core.Features.Users.Arguments;
using BingoBackend.Data.Entities;

namespace BingoBackend.Core.Features.Users;

public interface IUserFactory
{
    UserEntity Create(UserSignupArguments args);
}

public class UserFactory : IUserFactory
{
    public UserEntity Create(UserSignupArguments args)
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