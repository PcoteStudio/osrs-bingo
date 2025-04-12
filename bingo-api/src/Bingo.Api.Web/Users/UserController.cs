using AutoMapper;
using Bingo.Api.Core.Features.Users;
using Bingo.Api.Core.Features.Users.Exceptions;
using Bingo.Api.Web.Generic.Exceptions;
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
        catch (Exception ex)
        {
            switch (ex)
            {
                case UserNotFoundException or InvalidAccessTokenException:
                    throw new HttpException(StatusCodes.Status401Unauthorized, ex);
                default:
                    throw;
            }
        }
    }
}