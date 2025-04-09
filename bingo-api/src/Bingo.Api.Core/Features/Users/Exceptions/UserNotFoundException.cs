namespace Bingo.Api.Core.Features.Users.Exceptions;

public class UserNotFoundException(string username) : Exception
{
    public string Username { get; } = username;
}