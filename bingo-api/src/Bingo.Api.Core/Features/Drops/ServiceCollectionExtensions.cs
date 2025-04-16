using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bingo.Api.Core.Features.Drops;

public static class ServiceCollectionExtensions
{
    public static void AddDropService(this IServiceCollection services)
    {
        services.TryAddScoped<IDropService, DropService>();
        services.TryAddScoped<IDropServiceHelper, DropServiceHelper>();
        services.TryAddScoped<IDropRepository, DropRepository>();
        services.TryAddSingleton<IDropFactory, DropFactory>();
        services.TryAddSingleton<IDropUtil, DropUtil>();
    }
}