namespace Bingo.Api.Core.Features.Teams.Exceptions;

public class UserIsNotATeamAdminException(int teamId, string username) : Exception
{
    public int TeamId { get; } = teamId;
    public string Username { get; } = username;
}