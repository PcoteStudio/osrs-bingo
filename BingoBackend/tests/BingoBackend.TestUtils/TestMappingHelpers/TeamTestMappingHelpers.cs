using BingoBackend.Data.Team;

namespace BingoBackend.TestUtils.TestMappingHelpers;

public static class TestMappingHelpers
{
    public static string TeamEntityToTeamResponseJson(TeamEntity team)
    {
        return /* language=json */
            $$"""{ "id": {{team.Id}}, "name": "{{team.Name}}" }""";
    }

    public static string TeamEntityArrayToTeamResponseJsonArray(IEnumerable<TeamEntity> teams)
    {
        return /* language=json */
            $"[{string.Join(',', teams.Select(TeamEntityToTeamResponseJson))}]";
    }
}