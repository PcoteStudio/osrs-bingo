using System.Security.Claims;
using Bingo.Api.Core.Features.Players;
using Bingo.Api.Core.Features.Teams.Arguments;
using Bingo.Api.Core.Features.Teams.Exceptions;
using Bingo.Api.Core.Features.Users;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Bingo.Api.Data.Entities.Events;

namespace Bingo.Api.Core.Features.Teams;

public interface ITeamService
{
    Task<UserEntity> EnsureIsTeamAdminAsync(ClaimsPrincipal principal, int teamId);
    Task<TeamEntity> CreateTeamAsync(int eventId, TeamCreateArguments args);
    Task<TeamEntity> GetRequiredCompleteTeamAsync(int teamId);
    Task<TeamEntity> UpdateTeamAsync(int teamId, TeamUpdateArguments args);
    Task<TeamEntity> AddTeamPlayersAsync(int teamId, TeamPlayersArguments args);
    Task<TeamEntity> UpdateTeamPlayersAsync(int teamId, TeamPlayersArguments args);
    Task<TeamEntity> RemoveTeamPlayerAsync(int teamId, string playerName);
}

public class TeamService(
    ITeamFactory teamFactory,
    ITeamRepository teamRepository,
    ITeamUtil teamUtil,
    IUserService userService,
    IPlayerService playerService,
    ApplicationDbContext dbContext
) : ITeamService
{
    public async Task<UserEntity> EnsureIsTeamAdminAsync(ClaimsPrincipal principal, int teamId)
    {
        var user = await userService.GetRequiredMeAsync(principal);
        var team = await GetRequiredCompleteTeamAsync(teamId);
        if (!team.Event.Administrators.Contains(user))
            throw new UserIsNotATeamAdminException(teamId, principal.Identity!.Name!);
        return user;
    }

    public async Task<TeamEntity> CreateTeamAsync(int eventId, TeamCreateArguments args)
    {
        var team = teamFactory.Create(eventId, args);
        teamRepository.Add(team);
        await dbContext.SaveChangesAsync();
        return team;
    }

    public virtual async Task<TeamEntity> GetRequiredCompleteTeamAsync(int teamId)
    {
        var team = await teamRepository.GetCompleteByIdAsync(teamId);
        if (team == null) throw new TeamNotFoundException(teamId);
        return team;
    }

    public async Task<TeamEntity> UpdateTeamAsync(int teamId, TeamUpdateArguments args)
    {
        var team = await GetRequiredCompleteTeamAsync(teamId);
        teamUtil.UpdateTeam(team, args);
        dbContext.Update(team);
        await dbContext.SaveChangesAsync();
        return team;
    }

    public async Task<TeamEntity> AddTeamPlayersAsync(int teamId, TeamPlayersArguments args)
    {
        var team = await GetRequiredCompleteTeamAsync(teamId);
        var players = await playerService.GetOrCreatePlayersByNamesAsync(args.PlayerNames);
        var newPlayers = players
            .Where(newPlayer => team.Players
                .All(teamPlayer => newPlayer.Id != teamPlayer.Id));
        team.Players.AddRange(newPlayers);
        dbContext.Update(team);
        await dbContext.SaveChangesAsync();
        return team;
    }

    public async Task<TeamEntity> UpdateTeamPlayersAsync(int teamId, TeamPlayersArguments args)
    {
        var team = await GetRequiredCompleteTeamAsync(teamId);
        var players = await playerService.GetOrCreatePlayersByNamesAsync(args.PlayerNames);
        team.Players = players;
        dbContext.Update(team);
        await dbContext.SaveChangesAsync();
        return team;
    }

    public async Task<TeamEntity> RemoveTeamPlayerAsync(int teamId, string playerName)
    {
        var team = await GetRequiredCompleteTeamAsync(teamId);
        var player = await playerService.GetOrCreatePlayerByNameAsync(playerName);
        if (team.Players.Contains(player))
        {
            team.Players.Remove(player);
            dbContext.Update(team);
            await dbContext.SaveChangesAsync();
        }

        return team;
    }
}