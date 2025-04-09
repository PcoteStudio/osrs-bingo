using System.ComponentModel.DataAnnotations;

namespace Bingo.Api.Core.Features.Players;

public class PlayerCreateArguments
{
    [Required] public string Name { get; set; } = string.Empty;
}