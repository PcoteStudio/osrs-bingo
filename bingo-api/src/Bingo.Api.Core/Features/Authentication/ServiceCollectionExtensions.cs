using System.Text;
using Bingo.Api.Core.Features.Users;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Bingo.Api.Core.Features.Authentication;

public static class ServiceCollectionExtensions
{
    public static void AddAuthenticationService(this IServiceCollection services)
    {
        services
            .AddOptionsWithValidateOnStart<JwtOptions>()
            .BindConfiguration("JWT");

        services.AddIdentity<UserEntity, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }
        ).AddJwtBearer();

        services.AddOptions<JwtBearerOptions>().Configure<IOptions<JwtOptions>>((bearerOptions, jwtOptions) =>
        {
            bearerOptions.SaveToken = true;
            bearerOptions.RequireHttpsMetadata = false;
            bearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = jwtOptions.Value.ValidAudience,
                ValidIssuer = jwtOptions.Value.ValidIssuer,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Secret))
            };
        });

        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IAuthService, AuthService>();
    }
}