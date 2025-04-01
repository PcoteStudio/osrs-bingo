using BingoBackend.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace BingoBackend.Core.Features.Team;

public static class ServiceCollectionExtensions
{
    public static void AddTeamService(this IServiceCollection services)
    {
        if (services.IsRegistered<TeamService>()) return;
        services.AddScoped<TeamService>();
        services.AddScoped<ITeamRepository, TeamRepository>();
    }
}