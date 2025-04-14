using System.Security.Claims;
using Bingo.Api.Core.Features.Teams.Exceptions;
using Bingo.Api.Core.Features.Users;
using Bingo.Api.Data.Entities;
using Bingo.Api.Data.Entities.Events;

namespace Bingo.Api.Core.Features.Teams;

public interface ITeamServiceHelper
{
    Task<UserEntity> EnsureIsTeamAdminAsync(ClaimsPrincipal principal, int teamId);
    Task<List<TeamEntity>> GetAllRequiredByIdsAsync(ICollection<int> teamIds);
    Task<TeamEntity> GetRequiredCompleteTeamAsync(int teamId);
}

public class TeamServiceHelper(
    ITeamRepository teamRepository,
    IUserService userService
) : ITeamServiceHelper
{
    public async Task<UserEntity> EnsureIsTeamAdminAsync(ClaimsPrincipal principal, int teamId)
    {
        var user = await userService.GetRequiredMeAsync(principal);
        var team = await GetRequiredCompleteTeamAsync(teamId);
        if (!team.Event.Administrators.Contains(user))
            throw new UserIsNotATeamAdminException(teamId, principal.Identity!.Name!);
        return user;
    }

    public virtual async Task<List<TeamEntity>> GetAllRequiredByIdsAsync(ICollection<int> teamIds)
    {
        var teams = await teamRepository.GetAllByIdsAsync(teamIds);
        foreach (var teamId in teamIds)
            if (teams.All(t => t.Id != teamId))
                throw new TeamNotFoundException(teamId);
        return teams;
    }

    public virtual async Task<TeamEntity> GetRequiredCompleteTeamAsync(int teamId)
    {
        var team = await teamRepository.GetCompleteByIdAsync(teamId);
        if (team == null) throw new TeamNotFoundException(teamId);
        return team;
    }
}