namespace BingoBackend.Core.Features.Users.Exceptions;

public class UserNotFoundException(string username) : Exception
{
    public string Username { get; } = username;
}