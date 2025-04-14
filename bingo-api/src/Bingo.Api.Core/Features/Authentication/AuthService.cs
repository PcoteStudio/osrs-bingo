using Bingo.Api.Core.Features.Authentication.Arguments;
using Bingo.Api.Core.Features.Users.Exceptions;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Bingo.Api.Core.Features.Authentication;

public interface IAuthService
{
    Task<string> LoginAsync(AuthLoginArguments args);
}

public class AuthService(
    UserManager<UserEntity> userManager,
    ApplicationDbContext dbContext) : IAuthService
{
    public async Task<string> LoginAsync(AuthLoginArguments args)
    {
        var user = await userManager.FindByNameAsync(args.Username);
        if (user is null) throw new UserNotFoundException(args.Username);

        var isPasswordValid = await userManager.CheckPasswordAsync(user, args.Password);
        if (!isPasswordValid) throw new InvalidCredentialsException();

        return args.Username;
    }
}