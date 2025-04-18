using Bingo.Api.Core.Features.Authentication.Arguments;
using Bingo.Api.Core.Features.Users;
using Bingo.Api.Core.Features.Users.Exceptions;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Bingo.Api.Core.Features.Authentication;

public interface IAuthService
{
    Task<UserEntity> LoginAsync(AuthLoginArguments args);
}

public class AuthService(
    IUserRepository userRepository,
    IPasswordHasher<UserEntity> passwordHasher,
    ApplicationDbContext dbContext) : IAuthService
{
    public async Task<UserEntity> LoginAsync(AuthLoginArguments args)
    {
        var user = await userRepository.GetCompleteByNameAsync(args.Username);
        if (user is null) throw new InvalidCredentialsException();

        var verifyPasswordResults = passwordHasher.VerifyHashedPassword(user, user.HashedPassword, args.Password);
        if (verifyPasswordResults == PasswordVerificationResult.Failed) throw new InvalidCredentialsException();

        if (verifyPasswordResults == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.HashedPassword = passwordHasher.HashPassword(user, args.Password);
            dbContext.Users.Update(user);
        }

        return user;
    }
}