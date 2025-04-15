namespace Bingo.Api.Web.Players;

[Serializable]
public class PlayerShortResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}