using System.ComponentModel.DataAnnotations;

namespace BingoBackend.Core.Features.Users.Arguments;

public class UserLoginArguments
{
    [Required] public string Username { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}