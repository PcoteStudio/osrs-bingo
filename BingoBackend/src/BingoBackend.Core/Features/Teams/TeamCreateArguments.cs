using System.ComponentModel.DataAnnotations;

namespace BingoBackend.Core.Features.Teams;

public class TeamCreateArguments
{
    [Required] public string Name { get; set; } = string.Empty;
}