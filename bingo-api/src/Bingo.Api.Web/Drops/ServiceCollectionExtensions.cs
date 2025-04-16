namespace Bingo.Api.Web.Drops;

public static class ServiceCollectionExtensions
{
    public static void AddDropWebService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DropMappingProfile));
    }
}