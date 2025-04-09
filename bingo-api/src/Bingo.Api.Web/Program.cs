using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Web;

public class Program
{
    private static async Task CreateAndSeedDatabase(IHost host)
    {
        var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
        await new DbSeeder(
            scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>(),
            scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(),
            scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>(),
            scope.ServiceProvider.GetRequiredService<ILogger>()
        ).Seed();
    }

    internal static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        await CreateAndSeedDatabase(host);
        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostContext, builder) =>
            {
                if (hostContext.HostingEnvironment.IsDevelopment())
                    builder.AddUserSecrets<Program>();
            })
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}