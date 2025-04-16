namespace Bingo.Api.Web.DropInfos;

[Serializable]
public class DropInfoShortResponse
{
    public int Id { get; set; }
    public int NpcId { get; set; }
    public int ItemId { get; set; }
    public double? DropRate { get; set; }
    public double? Ehc { get; set; }
}