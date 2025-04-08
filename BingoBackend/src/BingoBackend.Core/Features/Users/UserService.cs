using BingoBackend.Core.Features.Authentication.Arguments;
using BingoBackend.Core.Features.Users.Exceptions;
using BingoBackend.Data;
using BingoBackend.Data.Constants;
using BingoBackend.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace BingoBackend.Core.Features.Users;

public interface IUserService
{
    Task<UserEntity> SignupUser(UserSignupArguments args);
}

public class UserService(
    UserManager<UserEntity> userManager,
    IUserFactory userFactory,
    ApplicationDbContext dbContext
) : IUserService
{
    public async Task<UserEntity> SignupUser(UserSignupArguments args)
    {
        var existingUser = await userManager.FindByNameAsync(args.Email);
        if (existingUser is not null) throw new EmailAlreadyInUseException(args.Email);

        var user = userFactory.Create(args);
        user = await CreateUser(user, args.Password);
        user = await AddRoleToUser(user, Roles.User);
        return user;
    }

    private async Task<UserEntity> CreateUser(UserEntity user, string password)
    {
        var createUserResult = await userManager.CreateAsync(user, password);
        if (createUserResult.Succeeded) return user;

        throw new Exception($"Failed to create user ${user.Email}. Errors: {string.Join(", ",
            createUserResult.Errors.Select(e => e.Description))}");
    }

    private async Task<UserEntity> AddRoleToUser(UserEntity user, Roles role)
    {
        if (await userManager.IsInRoleAsync(user, role.ToString())) return user;

        var addRoleToUserResult = await userManager.AddToRoleAsync(user, Roles.User.ToString());
        if (addRoleToUserResult.Succeeded) return user;

        throw new Exception($"Failed to add the ${role} role to the user ${user.Email}. Errors : {string.Join(",",
            addRoleToUserResult.Errors.Select(e => e.Description))}");
    }
}