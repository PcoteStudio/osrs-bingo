namespace Bingo.Api.Web.DropInfos;

public static class ServiceCollectionExtensions
{
    public static void AddDropInfoWebService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DropInfoMappingProfile));
    }
}