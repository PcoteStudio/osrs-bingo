namespace Bingo.Api.Core.Features.Teams.Exceptions;

public class TeamNotFoundException(int teamId) : Exception
{
    public int TeamId { get; } = teamId;
}