using Bingo.Api.Core.Features.Teams.Arguments;
using Bingo.Api.Data.Entities;
using Bingo.Api.Shared;
using Bingo.Api.Web.Teams;

namespace Bingo.Api.TestUtils.TestDataGenerators;

public static partial class TestDataGenerator
{
    public static string GenerateTeamName()
    {
        return RandomUtil.GetPrefixedRandomHexString("TName_", Random.Shared.Next(5, 25));
    }

    public static TeamEntity GenerateTeamEntity()
    {
        return new TeamEntity
        {
            Id = Random.Shared.Next(),
            EventId = Random.Shared.Next(),
            Name = GenerateTeamName(),
            Players = []
        };
    }

    public static List<TeamEntity> GenerateTeamEntities(int count)
    {
        return Enumerable.Range(0, count).Select(_ => GenerateTeamEntity()).ToList();
    }

    public static TeamResponse GenerateTeamResponse()
    {
        return new TeamResponse
        {
            Id = Random.Shared.Next(),
            Name = GenerateTeamName()
        };
    }

    public static List<TeamResponse> GenerateTeamResponses(int count)
    {
        return Enumerable.Range(0, count).Select(_ => GenerateTeamResponse()).ToList();
    }

    public static TeamCreateArguments GenerateTeamCreateArguments()
    {
        return new TeamCreateArguments
        {
            Name = GenerateTeamName()
        };
    }

    public static List<TeamCreateArguments> GenerateTeamCreateArguments(int count)
    {
        return Enumerable.Range(0, count).Select(_ => GenerateTeamCreateArguments()).ToList();
    }

    public static TeamUpdateArguments GenerateTeamUpdateArguments()
    {
        return new TeamUpdateArguments
        {
            Name = GenerateTeamName()
        };
    }

    public static List<TeamUpdateArguments> GenerateTeamUpdateArguments(int count)
    {
        return Enumerable.Range(0, count).Select(_ => GenerateTeamUpdateArguments()).ToList();
    }

    public static TeamPlayersArguments GenerateTeamPlayersArguments(int count)
    {
        return new TeamPlayersArguments
        {
            PlayerNames = Enumerable.Range(0, count).Select(_ => GeneratePlayerName()).ToList()
        };
    }
}