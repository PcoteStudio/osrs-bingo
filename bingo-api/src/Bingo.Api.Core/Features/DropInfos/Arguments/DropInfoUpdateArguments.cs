namespace Bingo.Api.Core.Features.DropInfos.Arguments;

public class DropInfoUpdateArguments
{
    public int NpcId { get; set; }
    public int ItemId { get; set; }
    public double? DropRate { get; set; }
}