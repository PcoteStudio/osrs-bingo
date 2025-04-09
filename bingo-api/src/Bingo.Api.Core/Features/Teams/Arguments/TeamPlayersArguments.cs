using System.ComponentModel.DataAnnotations;

namespace Bingo.Api.Core.Features.Teams.Arguments;

public class TeamPlayersArguments
{
    [Required] public List<string> PlayerNames { get; set; } = [];
}