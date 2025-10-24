using JetBrains.Annotations;

namespace Bingo.Api.Web.Drops;

[PublicAPI]
public class DropShortResponse
{
    public int Id { get; set; }
    public int NpcId { get; set; }
    public int ItemId { get; set; }
    public double? DropRate { get; set; }
    public double? Ehc { get; set; }
}