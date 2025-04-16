namespace Bingo.Api.Core.Features.DropInfos.Exceptions;

public class DropInfoNotFoundException(int dropInfoId) : Exception($"DropInfo {dropInfoId} not found.")
{
    public int DropInfoId { get; } = dropInfoId;
}