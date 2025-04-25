namespace Bingo.Api.Web.Tiles;

public static class ServiceCollectionExtensions
{
    public static void AddTileWebService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(TileMappingProfile));
    }
}