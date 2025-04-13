namespace Bingo.Api.Core.Features.Authentication.Arguments;

public class AuthLoginArguments
{
    public required string Username { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty;
}