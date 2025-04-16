namespace Bingo.Api.Web.Items;

public static class ServiceCollectionExtensions
{
    public static void AddItemWebService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ItemMappingProfile));
    }
}