using Microsoft.AspNetCore.Http;

namespace Bingo.Api.Core.Features.Npcs.Arguments;

public class NpcUpdateArguments
{
    public double? KillsPerHour { get; set; }
    public string Name { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
}