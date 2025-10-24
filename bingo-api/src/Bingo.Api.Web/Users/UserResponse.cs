using JetBrains.Annotations;

namespace Bingo.Api.Web.Users;

[PublicAPI]
public class UserResponse : UserPublicResponse
{
    public string Email { get; set; } = string.Empty;
}