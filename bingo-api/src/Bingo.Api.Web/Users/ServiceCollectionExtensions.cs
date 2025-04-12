namespace Bingo.Api.Web.Users;

public static class ServiceCollectionExtensions
{
    public static void AddUserWebService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UserMappingProfile));
    }
}