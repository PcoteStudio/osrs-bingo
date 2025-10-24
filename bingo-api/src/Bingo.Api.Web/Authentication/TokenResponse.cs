using JetBrains.Annotations;

namespace Bingo.Api.Web.Authentication;

[PublicAPI]
public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
}