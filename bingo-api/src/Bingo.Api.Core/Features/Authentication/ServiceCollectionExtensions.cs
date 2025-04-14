using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bingo.Api.Core.Features.Authentication;

public class ConfigureCookieAuthenticationOptions(IOptions<SessionAuthOptions> sessionAuthOptions)
    : IConfigureNamedOptions<CookieAuthenticationOptions>
{
    public void Configure(CookieAuthenticationOptions options)
    {
        options.Cookie.Name = sessionAuthOptions.Value.CookieName;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(sessionAuthOptions.Value.ExpiryInMinutes);
        options.SlidingExpiration = true;
    }

    public void Configure(string? name, CookieAuthenticationOptions options)
    {
        Configure(options);
    }
}

public class ConfigureSessionOptions(IOptions<SessionAuthOptions> sessionAuthOptions)
    : IConfigureNamedOptions<SessionOptions>
{
    public void Configure(SessionOptions options)
    {
        options.IdleTimeout = TimeSpan.FromMinutes(sessionAuthOptions.Value.ExpiryInMinutes);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    }

    public void Configure(string? name, SessionOptions options)
    {
        Configure(options);
    }
}

[Serializable]
public class SessionAuthOptions
{
    public required string CookieName { get; set; } = string.Empty;
    public required int ExpiryInMinutes { get; set; }
}

public static class ServiceCollectionExtensions
{
    public static void AddAuthenticationService(this IServiceCollection services)
    {
        services
            .AddOptionsWithValidateOnStart<SessionAuthOptions>()
            .BindConfiguration("Session");

        services.AddIdentity<UserEntity, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
        services.ConfigureOptions<ConfigureCookieAuthenticationOptions>();

        services.AddSession();
        services.ConfigureOptions<ConfigureSessionOptions>();

        services.AddScoped<IAuthService, AuthService>();
    }
}