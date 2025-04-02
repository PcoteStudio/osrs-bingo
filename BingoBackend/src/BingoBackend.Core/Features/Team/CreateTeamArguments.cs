using System.ComponentModel.DataAnnotations;

namespace BingoBackend.Core.Features.Team;

public class CreateTeamArguments
{
    [Required] public string Name { get; set; } = string.Empty;
}