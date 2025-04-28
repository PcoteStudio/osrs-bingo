using AutoMapper;
using Bingo.Api.Core.Features.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Web.Users;

[Route("/api/users")]
[ApiController]
public class AuthController(
    IPermissionServiceHelper permissionServiceHelper,
    IMapper mapper) : ControllerBase
{
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<UserResponse> GetMe([FromServices] IdentityContainer identityContainer)
    {
        permissionServiceHelper.EnsureHasPermissions(identityContainer.Identity);
        var user = (identityContainer.Identity as UserIdentity)!.User;
        return StatusCode(StatusCodes.Status200OK, mapper.Map<UserResponse>(user));
    }
}