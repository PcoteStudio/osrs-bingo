using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Bingo.Api.Web;

public class Program
{
    internal static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using var scope = host.Services.CreateScope();
        var sp = scope.ServiceProvider;
        new DbSeeder(sp.GetRequiredService<UserManager<UserEntity>>(),
                sp.GetRequiredService<RoleManager<IdentityRole>>(),
                sp.GetRequiredService<IWebHostEnvironment>(),
                sp.GetRequiredService<ILogger>())
            .Seed().Wait();

        host.Run();
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