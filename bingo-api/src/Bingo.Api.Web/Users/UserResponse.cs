namespace Bingo.Api.Web.Users;

[Serializable]
public class UserResponse : UserPublicResponse
{
    public string Email { get; set; } = string.Empty;
}