using System.ComponentModel.DataAnnotations;

namespace Bingo.Api.Core.Features.Teams.Arguments;

public class TeamUpdateArguments
{
    [Required] public string Name { get; set; } = string.Empty;
}