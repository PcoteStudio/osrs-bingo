using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Bingo.Api.Shared;
using Bingo.Api.TestUtils.TestDataSetups;
using Bingo.Api.Web;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Identity;
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
    private static string GetDatabaseFileName(BingoProjects project)
    {
        return project.ToString().ToLower() + "testdb.db";
    }

    private static string GetDatabaseConnectionString(BingoProjects project)
    {
        var databaseFile = Path.Combine("data", GetDatabaseFileName(project));
        var databasePath = Path.Combine(FileSystemHelper.FindDirectoryContaining("data"), databaseFile);
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

    private static Dictionary<string, string?> GetConfigurationValues(BingoProjects project)
    {
        return new Dictionary<string, string?>
        {
            ["Sqlite:ConnectionString"] = $"DataSource={{pathToData}}\\{GetDatabaseFileName(project)}"
        };
    }

    public static IWebHost BuildWebHost(BingoProjects project = BingoProjects.Web)
    {
        return WebHost.CreateDefaultBuilder([])
            .ConfigureServices(services =>
            {
                // Reduce the iteration count when hashing passwords to speed up user login in tests
                services.Configure<PasswordHasherOptions>(config => config.IterationCount = 1);
            })
            .ConfigureAppConfiguration(config =>
            {
                config.AddInMemoryCollection(GetConfigurationValues(project));
                config.AddUserSecrets<Program>();
            })
            .UseStartup<Startup>()
            .UseUrls("http://127.0.0.1:0")
            .Build();
    }

    public static TestDataSetup GetTestDataSetup(BingoProjects project)
    {
        var sp = BuildWebHost(project).Services;
        return new TestDataSetup(
            sp.GetRequiredService<ApplicationDbContext>(),
            sp.GetRequiredService<UserManager<UserEntity>>(),
            sp.GetRequiredService<RoleManager<IdentityRole>>(),
            sp.GetRequiredService<IAuthService>()
        );
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