using System.Reflection;
using BingoBackend.Data;
using BingoBackend.Web;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BingoBackend.TestUtils;

public enum BingoProjects
{
    Core,
    Web
}

public static class TestSetupUtil
{
    private static IConfigurationRoot GetTestConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();
    }

    public static ApplicationDbContext GetDbContext(BingoProjects project)
    {
        var test = Assembly.GetExecutingAssembly();
        var connectionString = $@"Data Source=..\..\..\..\..\data\{project.ToString().ToLower()}testdb.db";
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connectionString)
            .Options;
        return new ApplicationDbContext(dbContextOptions);
    }

    public static void RecreateDatabase(BingoProjects project)
    {
        var dbContext = GetDbContext(project);
        dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();
    }

    public static IWebHost BuildWebHost()
    {
        return WebHost.CreateDefaultBuilder([])
            .UseEnvironment("Test")
            .ConfigureAppConfiguration(config =>
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Sqlite:ConnectionString"] = @"Data Source=..\..\..\..\..\data\webtestdb.db"
                }))
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