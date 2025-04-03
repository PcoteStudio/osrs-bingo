using System.ComponentModel.DataAnnotations;

namespace BingoBackend.Core.Features.Teams.Arguments;

public class TeamCreateArguments
{
    [Required] public string Name { get; set; } = string.Empty;
}