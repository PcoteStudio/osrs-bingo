using Bingo.Api.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Bingo.Api.Core.Features.Players;

public static class ServiceCollectionExtensions
{
    public static void AddPlayerService(this IServiceCollection services)
    {
        if (services.IsRegistered<IPlayerService>()) return;
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddSingleton<IPlayerFactory, PlayerFactory>();
    }
}