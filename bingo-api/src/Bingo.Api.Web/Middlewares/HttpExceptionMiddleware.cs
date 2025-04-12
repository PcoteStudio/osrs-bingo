using System.Reflection;
using System.Text.Json;
using Bingo.Api.Web.Generic.Exceptions;

namespace Bingo.Api.Web.Middlewares;

public class HttpExceptionMiddleware(RequestDelegate next, ILogger<HttpExceptionMiddleware> logger)
{
    private static readonly string[] ExcludedExceptionFields =
        ["TargetSite", "StackTrace", "Message", "Data", "InnerException", "HelpLink", "Source", "HResult"];

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (HttpException ex)
        {
            logger.LogDebug(ex, "An expected error occurred:" + ex.Message);
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(
                JsonSerializer.Serialize(new { status = ex.StatusCode, message = ex.Message })
            );
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            logger.LogError(ex, "An unexpected error occurred: " + ex.Message);
            foreach (var propertyInfo in ex.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                         .Where(f => !ExcludedExceptionFields.Contains(f.Name)))
                logger.LogError("ERROR_DETAIL:" + propertyInfo.Name + "=" + propertyInfo.GetValue(ex));

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(new
                    {
                        status = StatusCodes.Status500InternalServerError,
                        message =
                            $"An unexpected error occurred and was logged with the reference ID {context.TraceIdentifier}"
                    }
                )
            );
        }
    }
}