namespace BingoBackend.Core.Features.Users.Exceptions;

public class EmailAlreadyInUseException(string email) : Exception($"Email {email} already in use.")
{
    public string Email { get; } = email;
}