using Bingo.Api.Core.Features.Users;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Bingo.Api.Core.Features.Authentication;

public class ConfigureJwtBearerOptions(IOptions<JwtOptions> jwtOptions) : IConfigureNamedOptions<JwtBearerOptions>
{
    public void Configure(JwtBearerOptions options)
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Value.ValidAudience,
            ValidIssuer = jwtOptions.Value.ValidIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromHexString(jwtOptions.Value.Secret))
        };
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }
}

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

        services.ConfigureOptions<ConfigureJwtBearerOptions>();

        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IAuthService, AuthService>();
    }
}