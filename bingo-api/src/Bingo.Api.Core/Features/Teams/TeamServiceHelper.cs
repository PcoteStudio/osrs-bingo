using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Core.Features.Authentication.Exception;
using Bingo.Api.Core.Features.Teams.Exceptions;
using Bingo.Api.Core.Features.Users;
using Bingo.Api.Core.Features.Users.Exceptions;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Teams;

public interface ITeamServiceHelper
{
    Task EnsureIsTeamAdminAsync(IIdentity? identity, int teamId);
    Task<List<TeamEntity>> GetAllRequiredByIdsAsync(ICollection<int> teamIds);
    Task<TeamEntity> GetRequiredCompleteAsync(int teamId);
}

public class TeamServiceHelper(
    ITeamRepository teamRepository,
    IUserService userService
) : ITeamServiceHelper
{
    public async Task EnsureIsTeamAdminAsync(IIdentity? identity, int teamId)
    {
        switch (identity)
        {
            case null:
                throw new UserIsNotLoggedInException();
            case UserIdentity userIdentity:
            {
                var team = await GetRequiredCompleteAsync(teamId);
                if (team.Event.Administrators.Any(a => a.Id == userIdentity.UserId))
                    return;
                throw new UserIsNotATeamAdminException(teamId, userIdentity.User.Username);
            }
            default:
                throw new AccessHasNotBeenDefinedException();
        }
    }

    public virtual async Task<List<TeamEntity>> GetAllRequiredByIdsAsync(ICollection<int> teamIds)
    {
        var teams = await teamRepository.GetAllByIdsAsync(teamIds);
        foreach (var teamId in teamIds)
            if (teams.All(t => t.Id != teamId))
                throw new TeamNotFoundException(teamId);
        return teams;
    }

    public virtual async Task<TeamEntity> GetRequiredCompleteAsync(int teamId)
    {
        var team = await teamRepository.GetCompleteByIdAsync(teamId);
        if (team == null) throw new TeamNotFoundException(teamId);
        return team;
    }
}