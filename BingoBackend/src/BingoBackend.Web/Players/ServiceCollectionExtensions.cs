namespace BingoBackend.Web.Players;

public static class ServiceCollectionExtensions
{
    public static void AddPlayerWebService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(PlayerResponse));
    }
}