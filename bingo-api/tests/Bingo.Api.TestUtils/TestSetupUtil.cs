using Bingo.Api.Data;
using Bingo.Api.Shared;
using Bingo.Api.Web;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bingo.Api.TestUtils;

public enum BingoProjects
{
    Core,
    Web
}

public static class TestSetupUtil
{
    public static string GetDatabaseFileName(BingoProjects project)
    {
        return project.ToString().ToLower() + "testdb.db";
    }

    public static string GetDatabaseConnectionString(BingoProjects project)
    {
        var databaseFile = Path.Combine("data", GetDatabaseFileName(project));
        var databasePath = Path.Combine(FileSystemHelper.FindDirectoryContaining( "data"), databaseFile);
        var connectionString = $"Data Source={databasePath};";
        return connectionString;
    }

    public static ApplicationDbContext GetDbContext(BingoProjects project)
    {
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(GetDatabaseConnectionString(project))
            .Options;
        return new ApplicationDbContext(dbContextOptions);
    }

    public static async Task RecreateDatabase(BingoProjects project)
    {
        var dbContext = GetDbContext(project);
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.MigrateAsync();
    }

    public static IWebHost BuildWebHost()
    {
        return WebHost.CreateDefaultBuilder([])
            .ConfigureAppConfiguration(config =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["Sqlite:ConnectionString"] =
                            $"DataSource={{pathToData}}\\{GetDatabaseFileName(BingoProjects.Web)}"
                    });
                    config.AddUserSecrets<Program>();
                }
            )
            .UseStartup<Startup>()
            .UseUrls("http://127.0.0.1:0")
            .Build();
    }

    public static Uri GetRequiredHostUri(IWebHost webHost)
    {
        return new Uri(webHost.Services
            .GetRequiredService<IServer>()
            .Features.Get<IServerAddressesFeature>()!
            .Addresses.First()
        );
    }
}