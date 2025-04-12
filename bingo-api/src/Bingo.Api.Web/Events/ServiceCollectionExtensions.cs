namespace Bingo.Api.Web.Events;

public static class ServiceCollectionExtensions
{
    public static void AddEventWebService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(EventMappingProfile));
    }
}