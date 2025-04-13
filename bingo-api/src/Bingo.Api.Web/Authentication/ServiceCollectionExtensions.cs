namespace Bingo.Api.Web.Authentication;

public static class ServiceCollectionExtensions
{
    public static void AddAuthenticationWebService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AuthenticationMappingProfile));
        services.AddOptions<CookieOptions>().Configure(options =>
        {
            options.HttpOnly = true;
            options.IsEssential = true;
            options.Secure = true;
            options.SameSite = SameSiteMode.Strict;
            options.Domain = "localhost"; // TODO implement dynamic domain
            options.Expires = DateTime.UtcNow.AddDays(14);
        });
    }
}