using System.ComponentModel.DataAnnotations;

namespace BingoBackend.Core.Features.Players;

public class PlayerCreateArguments
{
    [Required] public string Name { get; set; } = string.Empty;
}