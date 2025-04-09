using Bingo.Api.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Bingo.Api.Core.Features.Users;

public class JwtOptions
{
    public string ValidAudience { get; set; } = string.Empty;
    public string ValidIssuer { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
}

public static class ServiceCollectionExtensions
{
    public static void AddUserService(this IServiceCollection services)
    {
        if (services.IsRegistered<UserService>()) return;
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserFactory, UserFactory>();
    }
}