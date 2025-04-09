using Bingo.Api.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Bingo.Api.Core.Features.Teams;

public static class ServiceCollectionExtensions
{
    public static void AddTeamService(this IServiceCollection services)
    {
        if (services.IsRegistered<TeamService>()) return;
        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddSingleton<ITeamFactory, TeamFactory>();
        services.AddSingleton<ITeamUtil, TeamUtil>();
    }
}