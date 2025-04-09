using System.Text.Json;
using System.Text.Json.Serialization;
using Bingo.Api.Data;
using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Core.Features.Database;
using Bingo.Api.Core.Features.Players;
using Bingo.Api.Core.Features.Teams;
using Bingo.Api.Core.Features.Users;
using Bingo.Api.Web.Authentication;
using Bingo.Api.Web.Players;
using Bingo.Api.Web.Teams;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace Bingo.Api.Web;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ILogger, Logger<Program>>();

        services.AddMvc().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });
        
        // Swagger
        services.AddOpenApi();

        // Features
        services.AddSqliteDatabase();
        services.AddAuthenticationService();
        services.AddAuthenticationWebService();
        services.AddUserService();
        services.AddPlayerService();
        services.AddPlayerWebService();
        services.AddTeamService();
        services.AddTeamWebService();
    }

    public void Configure(IApplicationBuilder app, ILogger<Startup> logger, IServer server)
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

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapOpenApi();
            endpoints.MapScalarApiReference();
        });

        app.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(
                new { Message = "Invalid route: " + context.Request.Method + " " + context.Request.Path }
            );
        });
        
        logger.LogInformation($"API URL: {new Uri(new Uri(server.Features.Get<IServerAddressesFeature>()?.Addresses.First()!), "scalar/v1") }");
    }
}