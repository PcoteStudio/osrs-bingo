using Bingo.Api.Web.Middlewares;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bingo.Api.Web.Authentication;

public static class ServiceCollectionExtensions
{
    public static void AddAuthenticationWebService(this IServiceCollection services)
    {
        services.TryAddScoped<AuthenticationMiddleware>();
    }
}