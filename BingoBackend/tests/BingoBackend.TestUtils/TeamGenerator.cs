using BingoBackend.Core.Features.Team;
using BingoBackend.Data.Team;

namespace BingoBackend.TestUtils;

public static class TeamGenerator
{
    public static TeamCreateArguments CreateRandomTeamArguments()
    {
        return new TeamCreateArguments
        {
            Name = StringGenerator.GenerateRandomLength(1, 100)
        };
    }

    public static TeamEntity CreateRandomTeamEntity()
    {
        return new TeamFactory().Create(CreateRandomTeamArguments());
    }

    public static IEnumerable<TeamEntity> CreateRandomTeamEntities(int count)
    {
        return Enumerable.Range(0, count)
            .Select(x => CreateRandomTeamEntity())
            .ToList();
    }
}