namespace Bingo.Api.Core.Features.Events.Exceptions;

public class UserIsNotAnEventAdminException(int eventId, string username) : Exception
{
    public int EventId { get; } = eventId;
    public string Username { get; } = username;
}