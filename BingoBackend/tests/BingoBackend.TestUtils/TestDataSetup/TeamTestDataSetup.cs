using BingoBackend.Core.Features.Team;
using BingoBackend.Data.Team;

namespace BingoBackend.TestUtils.TestDataSetup;

public partial class TestDataSetup
{
    private readonly TeamFactory _teamFactory = new();

    public TestDataSetup AddTeam()
    {
        return AddTeam(out _);
    }

    public TestDataSetup AddTeam(out TeamEntity teamEntity)
    {
        return AddTeam(GenerateTeamCreateArguments(), out teamEntity);
    }

    public TestDataSetup AddTeam(TeamCreateArguments args)
    {
        return AddTeam(args, out _);
    }

    public TestDataSetup AddTeam(TeamCreateArguments args, out TeamEntity teamEntity)
    {
        return AddTeam(_teamFactory.Create(args), out teamEntity);
    }

    public TestDataSetup AddTeam(TeamEntity team)
    {
        return AddTeam(team, out _);
    }

    public TestDataSetup AddTeam(TeamEntity team, out TeamEntity teamEntity)
    {
        dbContext.Teams.Add(team);
        dbContext.SaveChanges();
        teamEntity = team;
        return this;
    }

    public TestDataSetup AddTeams(int count)
    {
        return AddTeams(count, out _);
    }

    public TestDataSetup AddTeams(int count, out List<TeamEntity> teamEntities)
    {
        teamEntities = Enumerable.Range(0, count).Select(_ =>
        {
            AddTeam(out var teamEntity);
            return teamEntity;
        }).ToList();
        return this;
    }

    public static TeamCreateArguments GenerateTeamCreateArguments()
    {
        return new TeamCreateArguments
        {
            Name = "Name_" + StringGenerator.GenerateRandomLength(1, 100)
        };
    }
}