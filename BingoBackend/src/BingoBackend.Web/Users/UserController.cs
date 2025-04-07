using AutoMapper;
using BingoBackend.Core.Features.Users;
using BingoBackend.Core.Features.Users.Arguments;
using BingoBackend.Core.Features.Users.Exceptions;
using BingoBackend.Web.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BingoBackend.Web.Users;

[Route("/api/users")]
[ApiController]
public class UserController(IUserService userService, IMapper mapper) : ControllerBase
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
    public async Task<ActionResult<TokenResponse>> Login(UserLoginArguments args)
    {
        try
        {
            var tokenModel = await userService.LoginUser(args);
            var tokenResponse = mapper.Map<TokenResponse>(tokenModel);
            return StatusCode(StatusCodes.Status200OK, tokenResponse);
        }
        catch (UserNotFoundException ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest);
        }
        catch (InvalidCredentialsException ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}