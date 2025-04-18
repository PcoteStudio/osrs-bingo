using Bingo.Api.Core.Features.Authentication.Arguments;
using Bingo.Api.Core.Features.Users.Exceptions;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Bingo.Api.Core.Features.Users;

public interface IUserService
{
    Task<UserEntity?> GetCompleteUserByIdAsync(int userId);
    Task<UserEntity> SignupUserAsync(AuthSignupArguments args);
}

public class UserService(
    IUserRepository userRepository,
    IUserFactory userFactory,
    IPasswordHasher<UserEntity> passwordHasher,
    ApplicationDbContext dbContext) : IUserService
{
    public async Task<UserEntity> SignupUserAsync(AuthSignupArguments args)
    {
        var existingUser = await userRepository.GetByEmailAsync(args.Email);
        if (existingUser is not null) throw new EmailAlreadyInUseException(args.Email);
        existingUser = await userRepository.GetByUsernameAsync(args.Username);
        if (existingUser is not null) throw new UsernameAlreadyInUseException(args.Username);

        var user = userFactory.Create(args);
        user.HashedPassword = passwordHasher.HashPassword(user, args.Password);
        userRepository.Add(user);
        await dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<UserEntity?> GetCompleteUserByIdAsync(int userId)
    {
        return await userRepository.GetCompleteByIdAsync(userId);
    }
}