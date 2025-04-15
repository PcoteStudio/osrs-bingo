using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bingo.Api.Core.Features.Seeder;

public static class ServiceCollectionExtensions
{
    public static void AddSeederService(this IServiceCollection services)
    {
        services.TryAddScoped<ISeederService, SeederService>();
    }
}