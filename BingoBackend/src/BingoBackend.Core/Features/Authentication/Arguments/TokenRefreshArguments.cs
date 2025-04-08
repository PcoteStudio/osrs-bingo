namespace BingoBackend.Core.Features.Authentication.Arguments;

public class TokenRefreshArguments
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}