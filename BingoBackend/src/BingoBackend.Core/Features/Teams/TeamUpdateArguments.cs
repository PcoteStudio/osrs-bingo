using System.ComponentModel.DataAnnotations;

namespace BingoBackend.Core.Features.Teams;

public class TeamUpdateArguments
{
    [Required] public string Name { get; set; } = string.Empty;
    public List<string> Players { get; set; } = [];
}