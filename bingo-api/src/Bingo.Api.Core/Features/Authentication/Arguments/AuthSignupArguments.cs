using System.ComponentModel.DataAnnotations;

namespace Bingo.Api.Core.Features.Authentication.Arguments;

public class AuthSignupArguments
{
    [MaxLength(30)] public required string Username { get; set; }

    [MaxLength(255)] [EmailAddress] public required string Email { get; set; }

    [MaxLength(255)] public required string Password { get; set; }
}