using AutoMapper;
using Bingo.Api.Core.Features.Users;
using Bingo.Api.Core.Features.Users.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Web.Users;

[Route("/api/users")]
[ApiController]
public class AuthController(
    IUserService userService,
    IMapper mapper,
    ILogger<AuthController> logger) : ControllerBase
{
    [Authorize]
    [HttpPost("me")]
    public async Task<IActionResult> GetMe()
    {
        try
        {
            var user = await userService.GetMe(User);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<UserResponse>(user));
        }
        catch (UserNotFoundException ex)
        {
            logger.LogWarning(ex.Message, ex);
            return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (InvalidAccessTokenException ex)
        {
            logger.LogWarning(ex.Message, ex);
            return StatusCode(StatusCodes.Status403Forbidden);
        }
    }
}