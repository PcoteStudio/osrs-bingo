using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BingoBackend.Core.Features.Authentication.Arguments;
using BingoBackend.Core.Features.Users;
using BingoBackend.Core.Features.Users.Exceptions;
using BingoBackend.Data;
using BingoBackend.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BingoBackend.Core.Features.Authentication;

public interface ITokenService
{
    Task<TokenRefreshArguments> Login(UserLoginArguments args);
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
    Task<TokenRefreshArguments> RefreshToken(TokenRefreshArguments tokenRefreshArguments);
}

public class TokenService(
    IOptions<JwtOptions> jwtOptions,
    UserManager<UserEntity> userManager,
    ITokenRepository tokenRepository,
    ApplicationDbContext dbContext) : ITokenService
{
    public async Task<TokenRefreshArguments> Login(UserLoginArguments args)
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

        var accessToken = GenerateAccessToken(authClaims);
        var refreshToken = GenerateRefreshToken();
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

        return new TokenRefreshArguments
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var authSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(jwtOptions.Value.Secret));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = jwtOptions.Value.ValidIssuer,
            Audience = jwtOptions.Value.ValidAudience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(60),
            SigningCredentials = new SigningCredentials
                (authSigningKey, SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Value.ValidAudience,
            ValidIssuer = jwtOptions.Value.ValidIssuer,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Secret))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }

    public async Task<TokenRefreshArguments> RefreshToken(TokenRefreshArguments tokenRefreshArguments)
    {
        var principal = GetPrincipalFromExpiredToken(tokenRefreshArguments.AccessToken);
        if (principal.Identity is null) throw new InvalidAccessTokenException();
        var username = principal.Identity.Name;

        var tokenInfo = dbContext.Tokens.SingleOrDefault(u => u.Username == username);
        if (tokenInfo == null
            || tokenInfo.RefreshToken != tokenRefreshArguments.RefreshToken
            || tokenInfo.ExpiredAt <= DateTime.UtcNow)
            throw new InvalidRefreshTokenException();

        var newAccessToken = GenerateAccessToken(principal.Claims);
        var newRefreshToken = GenerateRefreshToken();
        tokenInfo.RefreshToken = newRefreshToken;
        await dbContext.SaveChangesAsync();

        return new TokenRefreshArguments
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
}