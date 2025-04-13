namespace Bingo.Api.Core.Features.Authentication;

public class TokensModel
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}