namespace Bingo.Api.Core.Features.Npcs.Arguments;

public class NpcCreateArguments
{
    public double? KillsPerHours { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
}