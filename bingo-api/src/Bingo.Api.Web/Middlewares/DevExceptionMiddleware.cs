using System.Reflection;
using Bingo.Api.Web.Generic.Exceptions;

namespace Bingo.Api.Web.Middlewares;

public class DevExceptionMiddleware(RequestDelegate next, ILogger<DevExceptionMiddleware> logger)
{
    private static readonly string[] ExcludedExceptionFields =
        ["TargetSite", "StackTrace", "Message", "Data", "InnerException", "HelpLink", "Source", "HResult"];

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (HttpException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred: " + ex.Message);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(ex.ToString());

            foreach (var propertyInfo in ex.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                         .Where(f => !ExcludedExceptionFields.Contains(f.Name)))
                await context.Response.WriteAsync("\n" + propertyInfo.Name + "=" + propertyInfo.GetValue(ex));
        }
    }
}