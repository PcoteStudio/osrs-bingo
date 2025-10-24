using JetBrains.Annotations;

namespace Bingo.Api.Web.Users;

[PublicAPI]
public class UserPublicResponse
{
    public string Username { get; set; } = string.Empty;
}