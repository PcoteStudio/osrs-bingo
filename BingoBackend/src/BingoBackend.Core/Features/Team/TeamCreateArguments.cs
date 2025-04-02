using System.ComponentModel.DataAnnotations;

namespace BingoBackend.Core.Features.Team;

public class TeamCreateArguments
{
    [Required] public string Name { get; set; } = string.Empty;
}