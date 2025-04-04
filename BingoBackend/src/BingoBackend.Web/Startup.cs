using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using BingoBackend.Core.Features.Players;
using BingoBackend.Core.Features.Teams;
using BingoBackend.Data;
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
        services
            .AddOptions<DatabaseOptions>()
            .BindConfiguration("Sqlite")
            .ValidateOnStart();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlite(sp.GetRequiredService<IOptions<DatabaseOptions>>().Value.ConnectionString);
        });

        services.AddMvc().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });

        // Features
        services.AddPlayerService();
        services.AddPlayerWebService();
        services.AddTeamService();
        services.AddTeamWebService();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.Use(async (context, next) =>
        {
            try
            {
                await next.Invoke();
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(e.ToString());
            }
        });

        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
        }

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