using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Core.Features.Items;
using Bingo.Api.Core.Features.Items.Arguments;
using Bingo.Api.Core.Features.Items.Exceptions;
using Bingo.Api.Web.Generic.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Bingo.Api.Web.Items;

[Route("/api/items")]
[ApiController]
public class ItemController(
    IPermissionServiceHelper permissionServiceHelper,
    IItemService itemService,
    IItemServiceHelper itemServiceHelper) : ControllerBase
{
    [HttpGet("{itemId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ItemResponse>> GetItemAsync([FromRoute] int itemId)
    {
        try
        {
            var item = await itemServiceHelper.GetRequiredCompleteByIdAsync(itemId);
            return StatusCode(StatusCodes.Status200OK, item.ToResponse());
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case ItemNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ItemResponse>>> GetItemsAsync()
    {
        var items = await itemService.GetItemsAsync();
        return StatusCode(StatusCodes.Status200OK, items.ToResponseList());
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ItemResponse>> CreateItemAsync(
        [FromServices] IdentityContainer identityContainer,
        [FromForm] ItemCreateArguments args)
    {
        try
        {
            permissionServiceHelper.EnsureHasPermissions(identityContainer.Identity, "item.create");
            var item = await itemService.CreateItemAsync(args);
            return StatusCode(StatusCodes.Status201Created, item.ToResponse());
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case ItemAlreadyExistsException:
                    throw new HttpException(StatusCodes.Status400BadRequest, ex);
                default:
                    throw;
            }
        }
    }

    [HttpPut("{itemId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ItemResponse>> UpdateItemAsync(
        [FromServices] IdentityContainer identityContainer,
        [FromRoute] int itemId,
        [FromForm] ItemUpdateArguments args)
    {
        try
        {
            permissionServiceHelper.EnsureHasPermissions(identityContainer.Identity, "item.update");
            var item = await itemService.UpdateItemAsync(itemId, args);
            return StatusCode(StatusCodes.Status200OK, item.ToResponse());
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case ItemNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }

    [HttpDelete("{itemId:min(0)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ItemResponse>> RemoveItemAsync(
        [FromServices] IdentityContainer identityContainer,
        [FromRoute] int itemId)
    {
        try
        {
            permissionServiceHelper.EnsureHasPermissions(identityContainer.Identity, "item.delete");
            var item = await itemService.RemoveItemAsync(itemId);
            return StatusCode(StatusCodes.Status204NoContent, item.ToResponse());
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case ItemNotFoundException:
                    throw new HttpException(StatusCodes.Status404NotFound, ex);
                default:
                    throw;
            }
        }
    }
}