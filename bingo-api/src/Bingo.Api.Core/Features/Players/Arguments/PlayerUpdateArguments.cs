namespace Bingo.Api.Core.Features.Players.Arguments;

public class PlayerUpdateArguments
{
    public string Name { get; set; } = string.Empty;
    public List<int> TeamIds { get; set; } = [];
}