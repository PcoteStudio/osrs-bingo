using Bingo.Api.Core.Features.Events;
using Bingo.Api.Core.Features.Events.Arguments;
using Bingo.Api.Core.Features.Teams;
using Bingo.Api.Core.Features.Teams.Arguments;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Bingo.Api.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bingo.Api.Core.Features.Dev;

public interface IDevService
{
    Task<EventEntity> SeedEventAsync(UserEntity eventAdmin);
    Task DropDatabaseAsync();
}

public class DevService(
    IEventService eventService,
    ITeamService teamService,
    IDbSeeder dbSeeder,
    ApplicationDbContext dbContext,
    ILogger<DevService> logger
) : IDevService
{
    public async Task<EventEntity> SeedEventAsync(UserEntity eventAdmin)
    {
        logger.LogInformation($"Seeding a new event for user {eventAdmin.Username}");

        var newEvent = await eventService.CreateEventAsync(eventAdmin, new EventCreateArguments
        {
            Name = RandomUtil.GetPrefixedRandomHexString("EventName_", Random.Shared.Next(5, 20))
        });
        for (var i = 0; i < Random.Shared.Next(0, 10); i++)
        {
            var newTeam = await teamService.CreateTeamAsync(newEvent.Id, new TeamCreateArguments
            {
                Name = RandomUtil.GetPrefixedRandomHexString("TeamName_", Random.Shared.Next(5, 20))
            });
            await teamService.AddTeamPlayersAsync(newTeam.Id, new TeamPlayersArguments
            {
                PlayerNames = Enumerable.Range(0, Random.Shared.Next(0, 50)).Select(_ =>
                    RandomUtil.GetPrefixedRandomHexString("PlayerName_", Random.Shared.Next(5, 20))
                ).ToList()
            });
        }

        return newEvent;
    }

    public async Task DropDatabaseAsync()
    {
        logger.LogInformation("Dropping database");

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.MigrateAsync();
        dbContext.ChangeTracker.Clear();

        await dbSeeder.SeedDevUsersAsync();
    }
}