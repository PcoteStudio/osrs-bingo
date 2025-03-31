using Microsoft.Extensions.DependencyInjection;

namespace BingoBackend.Shared;

public static class ServiceCollectionExtensions
{
    public static bool IsRegistered<TType>(this IServiceCollection services)
    {
        return services.Any(x => x.ServiceType == typeof(TType) && x.ServiceKey == null);
    }
}