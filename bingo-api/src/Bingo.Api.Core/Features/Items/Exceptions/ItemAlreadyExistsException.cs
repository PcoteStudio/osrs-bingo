namespace Bingo.Api.Core.Features.Items.Exceptions;

public class ItemAlreadyExistsException(string itemName) : Exception($"Item {itemName} already exists.")
{
    public string ItemName { get; } = itemName;
}