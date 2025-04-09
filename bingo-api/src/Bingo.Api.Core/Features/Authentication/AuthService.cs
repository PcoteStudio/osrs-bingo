using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Bingo.Api.Core.Features.Authentication.Arguments;
using Bingo.Api.Core.Features.Users;
using Bingo.Api.Core.Features.Users.Exceptions;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Bingo.Api.Core.Features.Authentication;

public interface IAuthService
{
    Task<AuthRefreshArguments> Login(AuthLoginArguments args);
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
    Task<AuthRefreshArguments> RefreshToken(AuthRefreshArguments args);
    Task RevokeToken(ClaimsPrincipal principal);
}

public class AuthService(
    IOptions<JwtOptions> jwtOptions,
    UserManager<UserEntity> userManager,
    ITokenRepository tokenRepository,
    ApplicationDbContext dbContext) : IAuthService
{
    public async Task<AuthRefreshArguments> Login(AuthLoginArguments args)
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

        return new AuthRefreshArguments
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var authSigningKey = new SymmetricSecurityKey(Convert.FromHexString(jwtOptions.Value.Secret));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = jwtOptions.Value.ValidIssuer,
            Audience = jwtOptions.Value.ValidAudience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
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
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromHexString(jwtOptions.Value.Secret))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }

    public async Task<AuthRefreshArguments> RefreshToken(AuthRefreshArguments args)
    {
        var principal = GetPrincipalFromExpiredToken(args.AccessToken);
        if (principal.Identity?.Name is null) throw new InvalidAccessTokenException();
        var username = principal.Identity.Name;

        var tokenInfo = dbContext.Tokens.SingleOrDefault(u => u.Username == username);
        if (tokenInfo == null
            || tokenInfo.RefreshToken != args.RefreshToken
            || tokenInfo.ExpiredAt <= DateTime.UtcNow)
            throw new InvalidRefreshTokenException();

        var newAccessToken = GenerateAccessToken(principal.Claims);
        var newRefreshToken = GenerateRefreshToken();
        tokenInfo.RefreshToken = newRefreshToken;
        await dbContext.SaveChangesAsync();

        return new AuthRefreshArguments
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task RevokeToken(ClaimsPrincipal principal)
    {
        if (principal.Identity?.Name is null) throw new InvalidAccessTokenException();
        var username = principal.Identity.Name;

        var user = dbContext.Tokens.SingleOrDefault(u => u.Username == username);
        if (user == null) throw new UserNotFoundException(username);

        user.RefreshToken = string.Empty;
        await dbContext.SaveChangesAsync();
    }
}