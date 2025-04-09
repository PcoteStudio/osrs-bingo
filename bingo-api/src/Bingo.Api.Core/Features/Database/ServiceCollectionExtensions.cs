using System.ComponentModel.DataAnnotations;
using Bingo.Api.Data;
using Bingo.Api.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bingo.Api.Core.Features.Database;

public class DatabaseOptions
{
    [Required] public string ConnectionString { get; set; } = string.Empty;
}

public static class ServiceCollectionExtensions
{
    public static void AddSqliteDatabase(this IServiceCollection services)
    {
        services
            .AddOptionsWithValidateOnStart<DatabaseOptions>()
            .BindConfiguration("Sqlite");

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            const string dataFolder = "data";
            var dataPath = Path.Combine(FileSystemHelper.FindDirectoryContaining(dataFolder), dataFolder);
            var connectionString = sp
                .GetRequiredService<IOptions<DatabaseOptions>>()
                .Value.ConnectionString
                .Replace("{pathToData}", dataPath);
            options.UseSqlite(connectionString);
        });
    }
}