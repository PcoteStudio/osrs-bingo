using System.Net;
using System.Text.Json;
using Bingo.Api.TestUtils;
using Bingo.Api.Web.Teams;
using FluentAssertions;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Teams;

public partial class TeamFeatureTest
{
    [Test]
    public async Task GetPlayerTeamsAsync_ShouldReturnThePlayerTeams()
    {
        // Arrange
        _testDataSetup
            .AddUser()
            .AddEvent(out var eventEntity)
            .AddTeam(out var team)
            .AddPlayer(out var player);

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/players/{player.Id}/teams"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeams = JsonSerializer.Deserialize<List<TeamResponse>>(responseContent, JsonSerializerOptions.Web);
        returnedTeams.Should().NotBeNull();
        returnedTeams.Count.Should().Be(1);
        var returnedTeam = returnedTeams[0];
        returnedTeam.Id.Should().Be(team.Id);
        returnedTeam.Name.Should().Be(team.Name);
        returnedTeam.EventId.Should().Be(eventEntity.Id);
    }

    [Test]
    public async Task GetPlayerTeamsAsync_ShouldReturnNotFoundIfPlayerDoesNotExist()
    {
        // Arrange
        const int playerId = 1_000_000;

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/players/{playerId}/teams"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}