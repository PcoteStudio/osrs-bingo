using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Core.Features.Users;

namespace Bingo.Api.Web.Middlewares;

public class AuthenticationMiddleware(
    IUserRepository userRepository
) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Session.TryGetValue("userId", out var userId))
        {
            var id = int.Parse(userId);
            var user = await userRepository.GetCompleteByIdAsync(id);
            if (user is not null) context.Items[typeof(IIdentity)] = new UserIdentity(user);
        }

        await next(context);
    }
}