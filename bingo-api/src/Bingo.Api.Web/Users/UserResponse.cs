namespace Bingo.Api.Web.Users;

[Serializable]
public class UserResponse : PublicUserResponse
{
    public string Email { get; set; } = string.Empty;
}