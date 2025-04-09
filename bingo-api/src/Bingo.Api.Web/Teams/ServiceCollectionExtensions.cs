namespace Bingo.Api.Web.Teams;

public static class ServiceCollectionExtensions
{
    public static void AddTeamWebService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(TeamsMappingProfile));
    }
}