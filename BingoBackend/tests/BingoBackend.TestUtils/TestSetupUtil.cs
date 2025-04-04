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

public static class TestSetupUtil
{
    private static IConfigurationRoot GetTestConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();
    }

    public static ApplicationDbContext GetDbContext()
    {
        var connectionString = GetTestConfiguration().GetValue<string>("Sqlite:ConnectionString");
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connectionString)
            .Options;
        return new ApplicationDbContext(dbContextOptions);
    }

    public static void RecreateDatabase()
    {
        var dbContext = GetDbContext();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();
    }

    public static IWebHost BuildWebHost()
    {
        return WebHost.CreateDefaultBuilder([])
            .UseEnvironment("Test")
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