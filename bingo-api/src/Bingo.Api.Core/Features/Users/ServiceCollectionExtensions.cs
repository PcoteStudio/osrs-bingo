using Bingo.Api.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Bingo.Api.Core.Features.Users;

[Serializable]
public class JwtOptions
{
    public required string ValidAudience { get; set; } = string.Empty;
    public required string ValidIssuer { get; set; } = string.Empty;
    public required string Secret { get; set; } = string.Empty;
    public required int ExpiryInHours { get; set; }
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