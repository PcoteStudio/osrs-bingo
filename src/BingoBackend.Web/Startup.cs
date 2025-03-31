using System.Text.Json;
using System.Text.Json.Serialization;
using BingoBackend.Core.Features.Team;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTeamService();
        services.AddMvc().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });
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