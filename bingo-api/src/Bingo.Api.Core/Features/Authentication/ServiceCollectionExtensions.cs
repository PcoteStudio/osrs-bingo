using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bingo.Api.Core.Features.Authentication;

public static class ServiceCollectionExtensions
{
    public static void AddAuthenticationService(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();
        services.AddSession();
        services.AddHttpContextAccessor();
        services.TryAddScoped<IAuthService, AuthService>();
        services.TryAddSingleton<IPermissionServiceHelper, PermissionServiceHelper>();
        services.TryAddScoped<IdentityContainer>(sp =>
            new IdentityContainer(
                sp.GetRequiredService<IHttpContextAccessor>().HttpContext?.Items[typeof(IIdentity)] as IIdentity)
        );
    }
}