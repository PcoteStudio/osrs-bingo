using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using BingoBackend.Core.Features.Authentication;
using BingoBackend.Core.Features.Players;
using BingoBackend.Core.Features.Teams;
using BingoBackend.Core.Features.Users;
using BingoBackend.Data;
using BingoBackend.Shared;
using BingoBackend.Web.Authentication;
using BingoBackend.Web.Players;
using BingoBackend.Web.Teams;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BingoBackend.Web;

public class DatabaseOptions
{
    [Required] public string ConnectionString { get; set; } = string.Empty;
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ILogger, Logger<Program>>();

        // TODO Move DB initialization to an extension
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

        services.AddMvc().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });

        // Features
        services.AddAuthenticationService();
        services.AddAuthenticationWebService();
        services.AddUserService();
        services.AddPlayerService();
        services.AddPlayerWebService();
        services.AddTeamService();
        services.AddTeamWebService();
    }

    public void Configure(IApplicationBuilder app, ILogger<Startup> logger)
    {
        app.UseRouting();
        app.Use(async (context, next) =>
        {
            try
            {
                await next.Invoke();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unhandled exception occured.");
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(ex.ToString());
            }
        });

        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        app.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(
                new { Message = "Invalid route: " + context.Request.Method + " " + context.Request.Path }
            );
        });
    }
}