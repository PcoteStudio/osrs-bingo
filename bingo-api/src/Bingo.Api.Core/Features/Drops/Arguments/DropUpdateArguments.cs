namespace Bingo.Api.Core.Features.Drops.Arguments;

public class DropUpdateArguments
{
    public int NpcId { get; set; }
    public int ItemId { get; set; }
    public double? DropRate { get; set; }
}