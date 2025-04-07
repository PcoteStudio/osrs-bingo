using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BingoBackend.Core.Features.Authentication;
using BingoBackend.Core.Features.Users.Arguments;
using BingoBackend.Core.Features.Users.Exceptions;
using BingoBackend.Data;
using BingoBackend.Data.Constants;
using BingoBackend.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BingoBackend.Core.Features.Users;

public interface IUserService
{
    Task<UserEntity> SignupUser(UserSignupArguments args);
    Task<TokenModel> LoginUser(UserLoginArguments args);
}

public class UserService(
    UserManager<UserEntity> userManager,
    RoleManager<IdentityRole> roleManager,
    ITokenService tokenService,
    ITokenRepository tokenRepository,
    IUserFactory userFactory,
    ILogger<UserService> logger,
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

    public async Task<TokenModel> LoginUser(UserLoginArguments args)
    {
        var user = await userManager.FindByNameAsync(args.Username);
        if (user is null) throw new UserNotFoundException(args.Username);

        var isPasswordValid = await userManager.CheckPasswordAsync(user, args.Password);
        if (!isPasswordValid) throw new InvalidCredentialsException();

        List<Claim> authClaims =
        [
            new(ClaimTypes.Name, user.UserName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        var userRoles = await userManager.GetRolesAsync(user);
        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var accessToken = tokenService.GenerateAccessToken(authClaims);
        var refreshToken = tokenService.GenerateRefreshToken();
        var tokenEntity = await tokenRepository.GetByUsernameAsync(user.UserName!);
        if (tokenEntity is null)
        {
            var token = new TokenEntity
            {
                Username = user.UserName!,
                RefreshToken = refreshToken,
                ExpiredAt = DateTime.UtcNow.AddDays(7)
            };
            tokenRepository.Add(token);
        }
        else
        {
            tokenEntity.RefreshToken = refreshToken;
            tokenEntity.ExpiredAt = DateTime.UtcNow.AddDays(7);
        }

        await dbContext.SaveChangesAsync();

        return new TokenModel
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
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