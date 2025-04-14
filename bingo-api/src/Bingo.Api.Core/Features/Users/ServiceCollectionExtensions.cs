using Bingo.Api.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Bingo.Api.Core.Features.Users;

public static class ServiceCollectionExtensions
{
    public static void AddUserService(this IServiceCollection services)
    {
        if (services.IsRegistered<UserService>()) return;
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserFactory, UserFactory>();
    }
}