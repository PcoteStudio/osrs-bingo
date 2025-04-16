namespace Bingo.Api.Core.Features.Drops.Exceptions;

public class DropNotFoundException(int dropId) : Exception($"Drop {dropId} not found.")
{
    public int DropId { get; } = dropId;
}