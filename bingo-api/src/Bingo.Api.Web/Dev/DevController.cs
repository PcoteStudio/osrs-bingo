#if DEBUG || DEVELOPMENT

using AutoMapper;
using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Core.Features.Dev;
using Bingo.Api.Web.Events;
using Bingo.Api.Web.Generic.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Web.Dev;

[Route("/api/dev")]
[ApiController]
public class DevController(
    IPermissionServiceHelper permissionServiceHelper,
    IDevService devService,
    IMapper mapper,
    IHostEnvironment environment)
    : ControllerBase
{
    [HttpGet("ping")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<string> Ping([FromServices] IdentityContainer identityContainer)
    {
        EnsureHasAccess(identityContainer.Identity, "dev.ping");
        return StatusCode(StatusCodes.Status200OK, "pong");
    }

    [HttpPost("seed")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventResponse>> SeedAsync([FromServices] IdentityContainer identityContainer)
    {
        EnsureHasAccess(identityContainer.Identity, "dev.database.seed");

        var user = (identityContainer.Identity as UserIdentity)!.User;
        var newEvent = await devService.SeedEventAsync(user);
        return StatusCode(StatusCodes.Status201Created, mapper.Map<EventResponse>(newEvent));
    }

    [HttpPost("drop")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DropDatabaseAsync([FromServices] IdentityContainer identityContainer)
    {
        EnsureHasAccess(identityContainer.Identity, "dev.database.drop");

        await devService.DropDatabaseAsync();
        return StatusCode(StatusCodes.Status200OK);
    }

    private void EnsureHasAccess(IIdentity? identity, params string[] permissions)
    {
        if (!environment.IsDevelopment())
            throw new HttpException(StatusCodes.Status404NotFound);

        permissionServiceHelper.EnsureHasPermissions(identity, permissions);
    }
}

#endif