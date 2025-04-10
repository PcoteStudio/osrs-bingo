namespace Bingo.Api.Web.Events;

public static class ServiceCollectionExtensions
{
    public static void AddPlayerWebService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(EventsMappingProfile));
    }
}