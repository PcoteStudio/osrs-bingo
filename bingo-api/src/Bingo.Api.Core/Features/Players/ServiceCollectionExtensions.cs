using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bingo.Api.Core.Features.Players;

public static class ServiceCollectionExtensions
{
    public static void AddPlayerService(this IServiceCollection services)
    {
        services.TryAddScoped<IPlayerService, PlayerService>();
        services.TryAddScoped<IPlayerServiceHelper, PlayerServiceHelper>();
        services.TryAddScoped<IPlayerRepository, PlayerRepository>();
        services.TryAddSingleton<IPlayerFactory, PlayerFactory>();
        services.TryAddSingleton<IPlayerUtil, PlayerUtil>();
    }
}