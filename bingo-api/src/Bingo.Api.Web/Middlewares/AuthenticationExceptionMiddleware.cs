using Bingo.Api.Core.Features.Authentication.Exception;
using Bingo.Api.Core.Features.Users.Exceptions;
using Bingo.Api.Web.Generic.Exceptions;

namespace Bingo.Api.Web.Middlewares;

public class AuthenticationExceptionMiddleware(RequestDelegate next, ILogger<AuthenticationExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case UserIsNotLoggedInException:
                    throw new HttpException(StatusCodes.Status401Unauthorized, ex);
                case UserIsMissingPermissionException:
                    throw new HttpException(StatusCodes.Status403Forbidden, ex);
                case AccessHasNotBeenDefinedException:
                    logger.LogError(ex, ex.Message); // A fix is required
                    throw new HttpException(StatusCodes.Status403Forbidden, ex);
                default:
                    throw;
            }
        }
    }
}