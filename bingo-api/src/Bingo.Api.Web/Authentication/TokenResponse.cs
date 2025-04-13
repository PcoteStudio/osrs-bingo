namespace Bingo.Api.Web.Authentication;

[Serializable]
public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
}