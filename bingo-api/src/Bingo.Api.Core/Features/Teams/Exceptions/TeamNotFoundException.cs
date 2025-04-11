namespace Bingo.Api.Core.Features.Teams.Exceptions;

public class TeamNotFoundException(int teamId) : Exception($"Team {teamId} not found.")
{
    public int TeamId { get; } = teamId;
}