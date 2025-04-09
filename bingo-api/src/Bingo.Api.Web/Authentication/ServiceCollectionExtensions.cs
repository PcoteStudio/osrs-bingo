namespace Bingo.Api.Web.Authentication;

public static class ServiceCollectionExtensions
{
    public static void AddAuthenticationWebService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AuthenticationMappingProfile));
    }
}