namespace BingoBackend.Web.Team;

public static class ServiceCollectionExtensions
{
    public static void AddTeamWebService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(TeamResponseMappingProfile));
    }
}