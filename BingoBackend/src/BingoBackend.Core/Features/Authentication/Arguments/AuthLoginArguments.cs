using System.ComponentModel.DataAnnotations;

namespace BingoBackend.Core.Features.Authentication.Arguments;

public class AuthLoginArguments
{
    [Required] public string Username { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}