namespace Bingo.Api.Core.Features.Users.Exceptions;

public class UsernameAlreadyInUseException(string username) : Exception($"Username {username} already in use.")
{
    public string Username { get; } = username;
}