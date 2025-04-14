namespace Bingo.Api.Core.Features.Players.Arguments;

public class PlayerCreateArguments
{
    public string Name { get; set; } = string.Empty;
    public List<int> TeamIds { get; set; } = [];
}