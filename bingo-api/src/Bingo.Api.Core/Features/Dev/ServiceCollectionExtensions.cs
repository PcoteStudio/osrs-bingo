using Bingo.Api.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bingo.Api.Core.Features.Dev;

public static class ServiceCollectionExtensions
{
    public static void AddDevService(this IServiceCollection services)
    {
        services.TryAddScoped<IDbSeeder, DbSeeder>();
        services.TryAddScoped<IDevService, DevService>();
    }
}