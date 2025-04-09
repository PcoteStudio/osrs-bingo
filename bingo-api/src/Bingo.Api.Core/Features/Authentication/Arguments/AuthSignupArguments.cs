using System.ComponentModel.DataAnnotations;

namespace Bingo.Api.Core.Features.Authentication.Arguments;

public class AuthSignupArguments
{
    [Required] [MaxLength(30)] public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required] [MaxLength(255)] public string Password { get; set; } = string.Empty;
}