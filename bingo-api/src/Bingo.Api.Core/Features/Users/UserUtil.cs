namespace Bingo.Api.Core.Features.Users;

public interface IUserUtil
{
    string NormalizeEmail(string email);
    string NormalizeUsername(string username);
}

public class UserUtil : IUserUtil
{
    public string NormalizeEmail(string email)
    {
        return email.ToLower();
    }

    public string NormalizeUsername(string username)
    {
        return username.ToLower();
    }
}