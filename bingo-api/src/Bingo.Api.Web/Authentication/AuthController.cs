using AutoMapper;
using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Core.Features.Authentication.Arguments;
using Bingo.Api.Core.Features.Users;
using Bingo.Api.Core.Features.Users.Exceptions;
using Bingo.Api.Web.Generic.Exceptions;
using Bingo.Api.Web.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Bingo.Api.Web.Authentication;

[Route("/api/auth")]
[ApiController]
public class AuthController(
    IUserService userService,
    IAuthService authService,
    IOptions<CookieOptions> cookieOptions,
    IMapper mapper) : ControllerBase
{
    private const string RefreshTokenKey = "refresh_token";

    [HttpPost("signup")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponse>> SignupAsync(AuthSignupArguments args)
    {
        try
        {
            var user = await userService.SignupUserAsync(args);
            var userResponse = mapper.Map<UserResponse>(user);
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
    public async Task<ActionResult<TokenResponse>> LoginAsync(AuthLoginArguments args)
    {
        var tokenModel = await authService.LoginAsync(args);
        var tokenResponse = mapper.Map<TokenResponse>(tokenModel);
        Response.Cookies.Append(RefreshTokenKey, tokenModel.RefreshToken, cookieOptions.Value);
        return StatusCode(StatusCodes.Status200OK, tokenResponse);
    }

    [Authorize]
    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenResponse>> RefreshTokenAsync(AuthRefreshArguments args)
    {
        try
        {
            var tokenModel = await authService.RefreshTokenAsync(Request.Cookies[RefreshTokenKey], args);
            var tokenResponse = mapper.Map<TokenResponse>(tokenModel);
            Response.Cookies.Append(RefreshTokenKey, tokenModel.RefreshToken, cookieOptions.Value);
            return StatusCode(StatusCodes.Status200OK, tokenResponse);
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case InvalidRefreshTokenException or SecurityTokenException:
                    throw new HttpException(StatusCodes.Status400BadRequest, ex);
                default:
                    throw;
            }
        }
    }

    [Authorize]
    [HttpPost("revoke")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RevokeAsync()
    {
        await authService.RevokeTokenAsync(User);
        Response.Cookies.Delete(RefreshTokenKey);
        return StatusCode(StatusCodes.Status200OK);
    }
}