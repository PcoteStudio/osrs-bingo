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
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserResponse>> GetMeAsync()
    {
        try
        {
            var user = await userService.GetRequiredMeAsync(User);
            return StatusCode(StatusCodes.Status200OK, mapper.Map<UserResponse>(user));
        }
        catch (UserNotFoundException ex)
        {
            logger.LogWarning(ex.Message, ex);
            return StatusCode(StatusCodes.Status401Unauthorized);
        }
        catch (InvalidAccessTokenException ex)
        {
            logger.LogWarning(ex.Message, ex);
            return StatusCode(StatusCodes.Status401Unauthorized);
        }
    }
}