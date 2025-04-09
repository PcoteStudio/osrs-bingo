using System.Security.Claims;
using Bingo.Api.Core.Features.Authentication.Arguments;
using Bingo.Api.Core.Features.Users.Exceptions;
using Bingo.Api.Data;
using Bingo.Api.Data.Constants;
using Bingo.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Bingo.Api.Core.Features.Users;

public interface IUserService
{
    Task<UserEntity> SignupUser(AuthSignupArguments args);
    Task<UserEntity> GetMe(ClaimsPrincipal principal);
}

public class UserService(
    UserManager<UserEntity> userManager,
    IUserFactory userFactory,
    ApplicationDbContext dbContext
) : IUserService
{
    public async Task<UserEntity> SignupUser(AuthSignupArguments args)
    {
        var existingUser = await userManager.FindByEmailAsync(args.Email);
        if (existingUser is not null) throw new EmailAlreadyInUseException(args.Email);

        var user = userFactory.Create(args);
        user = await CreateUser(user, args.Password);
        user = await AddRoleToUser(user, Roles.User);
        return user;
    }

    public async Task<UserEntity> GetMe(ClaimsPrincipal principal)
    {
        if (principal.Identity?.Name is null) throw new InvalidAccessTokenException();
        var existingUser = await userManager.FindByNameAsync(principal.Identity.Name);
        if (existingUser is null) throw new UserNotFoundException(principal.Identity.Name);
        return existingUser;
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