using AutoMapper;
using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Core.Features.Authentication.Arguments;
using Bingo.Api.Core.Features.Users;
using Bingo.Api.Core.Features.Users.Exceptions;
using Bingo.Api.Web.Generic.Exceptions;
using Bingo.Api.Web.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Bingo.Api.Web.Authentication;

[Route("/api/auth")]
[ApiController]
public class AuthController(
    IUserService userService,
    IAuthService authService,
    IMapper mapper,
    ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("signup")]
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
                case EmailAlreadyInUseException:
                    throw new HttpException(StatusCodes.Status400BadRequest, ex);
                default:
                    throw;
            }
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> LoginAsync(AuthLoginArguments args)
    {
        try
        {
            var tokenModel = await authService.LoginAsync(args);
            var tokenResponse = mapper.Map<TokenResponse>(tokenModel);
            return StatusCode(StatusCodes.Status200OK, tokenResponse);
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case InvalidCredentialsException:
                    throw new HttpException(StatusCodes.Status401Unauthorized, ex);
                case UserNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }

    [Authorize]
    [HttpPost("refresh")]
    public async Task<ActionResult<TokenResponse>> RefreshTokenAsync(AuthRefreshArguments args)
    {
        try
        {
            var tokenModel = await authService.RefreshTokenAsync(args);
            var tokenResponse = mapper.Map<TokenResponse>(tokenModel);
            return StatusCode(StatusCodes.Status200OK, tokenResponse);
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case InvalidAccessTokenException or InvalidRefreshTokenException or SecurityTokenException:
                    throw new HttpException(StatusCodes.Status401Unauthorized, ex);
                default:
                    throw;
            }
        }
    }

    [Authorize]
    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeAsync()
    {
        try
        {
            await authService.RevokeTokenAsync(User);
            return StatusCode(StatusCodes.Status200OK);
        }
        catch (UserNotFoundException ex)
        {
            logger.LogWarning(ex.Message, ex);
            return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (InvalidAccessTokenException ex)
        {
            logger.LogWarning(ex.Message, ex);
            return StatusCode(StatusCodes.Status401Unauthorized);
        }
    }
}