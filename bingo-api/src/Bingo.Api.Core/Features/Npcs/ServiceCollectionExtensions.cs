using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bingo.Api.Core.Features.Npcs;

public static class ServiceCollectionExtensions
{
    public static void AddNpcService(this IServiceCollection services)
    {
        services.TryAddScoped<INpcService, NpcService>();
        services.TryAddScoped<INpcServiceHelper, NpcServiceHelper>();
        services.TryAddScoped<INpcRepository, NpcRepository>();
        services.TryAddSingleton<INpcFactory, NpcFactory>();
        services.TryAddSingleton<INpcUtil, NpcUtil>();
    }
}