using AutoMapper;
using BingoBackend.Core.Features.Authentication;
using BingoBackend.Core.Features.Authentication.Arguments;
using BingoBackend.Core.Features.Users;
using BingoBackend.Core.Features.Users.Exceptions;
using BingoBackend.Web.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BingoBackend.Web.Authentication;

[Route("/api/auth")]
[ApiController]
public class AuthController(IUserService userService, ITokenService tokenService, IMapper mapper) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<ActionResult<UserResponse>> Signup(UserSignupArguments args)
    {
        try
        {
            var user = await userService.SignupUser(args);
            var userResponse = mapper.Map<UserResponse>(user);
            return StatusCode(StatusCodes.Status201Created, userResponse);
        }
        catch (EmailAlreadyInUseException ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginArguments args)
    {
        try
        {
            var tokenModel = await tokenService.Login(args);
            var tokenResponse = mapper.Map<TokenResponse>(tokenModel);
            return StatusCode(StatusCodes.Status200OK, tokenResponse);
        }
        catch (UserNotFoundException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        catch (InvalidCredentialsException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<TokenResponse>> RefreshToken(TokenRefreshArguments args)
    {
        try
        {
            var tokenModel = await tokenService.RefreshToken(args);
            var tokenResponse = mapper.Map<TokenResponse>(tokenModel);
            return StatusCode(StatusCodes.Status200OK, tokenResponse);
        }
        catch (InvalidAccessTokenException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        catch (InvalidRefreshTokenException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        catch (SecurityTokenException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
    }
}