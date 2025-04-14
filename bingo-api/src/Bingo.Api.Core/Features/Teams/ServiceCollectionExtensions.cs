using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bingo.Api.Core.Features.Teams;

public static class ServiceCollectionExtensions
{
    public static void AddTeamService(this IServiceCollection services)
    {
        services.TryAddScoped<ITeamService, TeamService>();
        services.TryAddScoped<ITeamServiceHelper, TeamServiceHelper>();
        services.TryAddScoped<ITeamRepository, TeamRepository>();
        services.TryAddSingleton<ITeamFactory, TeamFactory>();
        services.TryAddSingleton<ITeamUtil, TeamUtil>();
    }
}