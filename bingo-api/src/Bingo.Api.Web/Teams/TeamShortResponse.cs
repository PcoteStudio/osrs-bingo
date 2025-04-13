using JetBrains.Annotations;

namespace Bingo.Api.Web.Teams;

[PublicAPI]
public class TeamShortResponse
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string Name { get; set; } = string.Empty;
}