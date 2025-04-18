using System.Text.Json;
using System.Text.Json.Serialization;
using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Core.Features.Database;
using Bingo.Api.Core.Features.Dev;
using Bingo.Api.Core.Features.Events;
using Bingo.Api.Core.Features.Items;
using Bingo.Api.Core.Features.Npcs;
using Bingo.Api.Core.Features.Players;
using Bingo.Api.Core.Features.Teams;
using Bingo.Api.Core.Features.Users;
using Bingo.Api.Data;
using Bingo.Api.Web.Authentication;
using Bingo.Api.Web.Drops;
using Bingo.Api.Web.Events;
using Bingo.Api.Web.Items;
using Bingo.Api.Web.Middlewares;
using Bingo.Api.Web.Npcs;
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

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy
                    .AllowAnyOrigin() // Allow requests from ANY origin (more permissive for troubleshooting)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.AddMvc().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });

        services.AddOpenApi();
        services.AddSqliteDatabase();
        services.AddAuthenticationService();
        services.AddAuthenticationWebService();

        // Features
        services.AddDevService();
        services.AddUserService();
        services.AddEventService();
        services.AddEventWebService();
        services.AddTeamService();
        services.AddTeamWebService();
        services.AddPlayerService();
        services.AddPlayerWebService();
        services.AddDropWebService();
        services.AddItemService();
        services.AddItemWebService();
        services.AddNpcService();
        services.AddNpcWebService();
    }

    public void Configure(IApplicationBuilder app, ILogger<Startup> logger, IServer server, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseMiddleware<HttpExceptionMiddleware>();
        if (env.IsDevelopment() || env.IsEnvironment("Test")) app.UseMiddleware<DevExceptionMiddleware>();
        app.UseMiddleware<AuthenticationExceptionMiddleware>();

        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
        }

        app.UseSession();
        app.UseMiddleware<AuthenticationMiddleware>();

        app.UseCors("AllowAll");

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

        logger.LogInformation(
            $"API URL: {new Uri(new Uri(server.Features.Get<IServerAddressesFeature>()?.Addresses.First()!), "scalar/v1")}");
    }
}