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
    Task<UserEntity> SignupUserAsync(AuthSignupArguments args);
    Task<UserEntity> GetRequiredMeAsync(ClaimsPrincipal principal);
}

public class UserService(
    UserManager<UserEntity> userManager,
    IUserFactory userFactory) : IUserService
{
    public async Task<UserEntity> SignupUserAsync(AuthSignupArguments args)
    {
        var existingUser = await userManager.FindByEmailAsync(args.Email);
        if (existingUser is not null) throw new EmailAlreadyInUseException(args.Email);
        existingUser = await userManager.FindByNameAsync(args.Username);
        if (existingUser is not null) throw new UsernameAlreadyInUseException(args.Username);

        var user = userFactory.Create(args);

        user = await CreateUserAsync(user, args.Password);
        user = await AddRoleToUserAsync(user, Roles.User);
        return user;
    }

    public async Task<UserEntity> GetRequiredMeAsync(ClaimsPrincipal principal)
    {
        return (await userManager.FindByNameAsync(principal.Identity.NotNull().Name.NotNull())).NotNull();
    }

    private async Task<UserEntity> CreateUserAsync(UserEntity user, string password)
    {
        var createUserResult = await userManager.CreateAsync(user, password);

        if (createUserResult.Succeeded) return user;

        throw new Exception($"Failed to create user ${user.Email}. Errors: {string.Join(", ",
            createUserResult.Errors.Select(e => e.Description))}");
    }

    private async Task<UserEntity> AddRoleToUserAsync(UserEntity user, Roles role)
    {
        if (await userManager.IsInRoleAsync(user, role.ToString())) return user;

        var addRoleToUserResult = await userManager.AddToRoleAsync(user, Roles.User.ToString());
        if (addRoleToUserResult.Succeeded) return user;

        throw new Exception($"Failed to add the ${role} role to the user ${user.Email}. Errors : {string.Join(",",
            addRoleToUserResult.Errors.Select(e => e.Description))}");
    }
}