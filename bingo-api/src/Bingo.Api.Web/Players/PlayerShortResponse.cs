using JetBrains.Annotations;

namespace Bingo.Api.Web.Players;

[PublicAPI]
public class PlayerShortResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}