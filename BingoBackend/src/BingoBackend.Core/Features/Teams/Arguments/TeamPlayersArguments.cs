using System.ComponentModel.DataAnnotations;

namespace BingoBackend.Core.Features.Teams.Arguments;

public class TeamPlayersArguments
{
    [Required] public List<string> PlayerNames { get; set; } = [];
}