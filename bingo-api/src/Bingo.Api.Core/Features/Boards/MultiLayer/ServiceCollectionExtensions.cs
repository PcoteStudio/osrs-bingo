using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bingo.Api.Core.Features.Boards.MultiLayer;

public static class ServiceCollectionExtensions
{
    public static void AddMultiLayerBoardService(this IServiceCollection services)
    {
        services.TryAddScoped<IMultiLayerBoardService, MultiLayerBoardService>();
        services.TryAddScoped<IMultiLayerBoardRepository, MultiLayerBoardRepository>();
        services.TryAddSingleton<IMultiLayerBoardUtil, MultiLayerBoardUtil>();
        services.TryAddSingleton<IMultiLayerBoardFactory, MultiLayerBoardFactory>();
        services.TryAddSingleton<IBoardLayerFactory, BoardLayerFactory>();
    }
}