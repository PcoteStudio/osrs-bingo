using Bingo.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bingo.Api.Core.Features.Users;

public static class ServiceCollectionExtensions
{
    public static void AddUserService(this IServiceCollection services)
    {
        services.TryAddScoped<IUserService, UserService>();
        services.TryAddScoped<IUserRepository, UserRepository>();
        services.TryAddSingleton<IUserUtil, UserUtil>();
        services.TryAddSingleton<IUserFactory, UserFactory>();
        services.TryAddSingleton<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();
    }
}