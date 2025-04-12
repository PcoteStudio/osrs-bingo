using AutoMapper;
using Bingo.Api.Core.Features.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Web.Users;

[Route("/api/users")]
[ApiController]
public class AuthController(
    IUserService userService,
    IMapper mapper) : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserResponse>> GetMeAsync()
    {
        var user = await userService.GetRequiredMeAsync(User);
        return StatusCode(StatusCodes.Status200OK, mapper.Map<UserResponse>(user));
    }
}