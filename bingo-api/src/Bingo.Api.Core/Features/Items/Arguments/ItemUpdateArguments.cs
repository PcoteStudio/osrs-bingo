namespace Bingo.Api.Core.Features.Items.Arguments;

public class ItemUpdateArguments
{
    public double? KillsPerHours { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
}