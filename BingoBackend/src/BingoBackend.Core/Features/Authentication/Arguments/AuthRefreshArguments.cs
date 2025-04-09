namespace BingoBackend.Core.Features.Authentication.Arguments;

public class AuthRefreshArguments
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}