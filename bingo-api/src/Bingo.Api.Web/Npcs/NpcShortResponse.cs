namespace Bingo.Api.Web.Npcs;

[Serializable]
public class NpcShortResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public double? KillsPerHour { get; set; }
}