using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bingo.Api.Core.Features.Items;

public static class ServiceCollectionExtensions
{
    public static void AddItemService(this IServiceCollection services)
    {
        services.TryAddScoped<IItemService, ItemService>();
        services.TryAddScoped<IItemServiceHelper, ItemServiceHelper>();
        services.TryAddScoped<IItemRepository, ItemRepository>();
        services.TryAddSingleton<IItemFactory, ItemFactory>();
        services.TryAddSingleton<IItemUtil, ItemUtil>();
    }
}