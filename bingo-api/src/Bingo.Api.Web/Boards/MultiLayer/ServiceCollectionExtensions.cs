namespace Bingo.Api.Web.Boards.MultiLayer;

public static class ServiceCollectionExtensions
{
    public static void AddMultiLayerBoardWebService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MultiLayerBoardMappingProfile));
    }
}