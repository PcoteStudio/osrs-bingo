namespace Bingo.Api.Core.Features.Items.Exceptions;

public class ItemNotFoundException(int itemId) : Exception($"Item {itemId} not found.")
{
    public int ItemId { get; } = itemId;
}