using Bingo.Api.Core.Features.Database;
using Bingo.Api.Shared;
using Microsoft.Extensions.Options;
using NeoSmart.Caching.Sqlite;

namespace Bingo.Api.Web.Cache;

public static class ServiceCollectionExtensions
{
    public static void AddSqliteDistributedCacheService(this IServiceCollection services)
    {
        services.AddSqliteCache(_ => { });
        services.AddOptions<SqliteCacheOptions>().Configure<IOptions<DatabaseOptions>>((sqlOptions, dbOptions) =>
        {
            const string dataFolder = "data";
            var dataPath = Path.Combine(FileSystemHelper.FindDirectoryContaining(dataFolder), dataFolder);
            sqlOptions.CachePath = dbOptions.Value.CachePath.Replace("{pathToData}", dataPath);
        });
    }
}