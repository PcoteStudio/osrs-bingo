using System.ComponentModel.DataAnnotations;

namespace BingoBackend.Core.Features.Users.Arguments;

public class UserSignupArguments
{
    [Required] [MaxLength(30)] public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required] [MaxLength(255)] public string Password { get; set; } = string.Empty;
}