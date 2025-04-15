using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bingo.Api.Core.Features.Events;

public static class ServiceCollectionExtensions
{
    public static void AddEventService(this IServiceCollection services)
    {
        services.TryAddScoped<IEventService, EventService>();
        services.TryAddScoped<IEventServiceHelper, EventServiceHelper>();
        services.TryAddScoped<IEventRepository, EventRepository>();
        services.TryAddSingleton<IEventFactory, EventFactory>();
        services.TryAddSingleton<IEventUtil, EventUtil>();
    }
}