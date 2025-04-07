using Microsoft.Extensions.DependencyInjection;

namespace BingoBackend.Core.Features.Users;

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
    }
}