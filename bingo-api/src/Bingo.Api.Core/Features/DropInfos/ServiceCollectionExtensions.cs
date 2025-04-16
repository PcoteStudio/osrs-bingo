using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bingo.Api.Core.Features.DropInfos;

public static class ServiceCollectionExtensions
{
    public static void AddDropInfoService(this IServiceCollection services)
    {
        services.TryAddScoped<IDropInfoService, DropInfoService>();
        services.TryAddScoped<IDropInfoServiceHelper, DropInfoServiceHelper>();
        services.TryAddScoped<IDropInfoRepository, DropInfoRepository>();
        services.TryAddSingleton<IDropInfoFactory, DropInfoFactory>();
        services.TryAddSingleton<IDropInfoUtil, DropInfoUtil>();
    }
}