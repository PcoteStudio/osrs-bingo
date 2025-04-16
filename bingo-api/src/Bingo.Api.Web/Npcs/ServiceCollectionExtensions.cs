namespace Bingo.Api.Web.Npcs;

public static class ServiceCollectionExtensions
{
    public static void AddNpcWebService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(NpcMappingProfile));
    }
}