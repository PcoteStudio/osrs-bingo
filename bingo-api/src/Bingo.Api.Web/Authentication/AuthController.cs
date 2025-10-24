using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Core.Features.Authentication.Arguments;
using Bingo.Api.Core.Features.Users;
using Bingo.Api.Core.Features.Users.Exceptions;
using Bingo.Api.Web.Generic.Exceptions;
using Bingo.Api.Web.Users;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Web.Authentication;

[Route("/api/auth")]
[ApiController]
public class AuthController(
    IUserService userService,
    IAuthService authService) : ControllerBase
{
    [HttpPost("signup")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponse>> SignupAsync(AuthSignupArguments args)
    {
        try
        {
            var user = await userService.SignupUserAsync(args);
            var userResponse = user.ToResponse();
            return StatusCode(StatusCodes.Status201Created, userResponse);
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case EmailAlreadyInUseException or UsernameAlreadyInUseException:
                    throw new HttpException(StatusCodes.Status400BadRequest, ex);
                default:
                    throw;
            }
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> LoginAsync(AuthLoginArguments args)
    {
        try
        {
            var user = await authService.LoginAsync(args);
            HttpContext.Session.SetString("userId", user.Id.ToString());
            return StatusCode(StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case InvalidCredentialsException:
                    throw new HttpException(StatusCodes.Status400BadRequest, ex);
                default:
                    throw;
            }
        }
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult Logout()
    {
        HttpContext.Session.Remove("userId");
        return StatusCode(StatusCodes.Status200OK);
    }
}