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
    public async Task GetTeamAsync_ShouldReturnTheSpecifiedTeam()
    {
        // Arrange
        _testDataSetup
            .AddUser()
            .AddEvent(out var eventEntity)
            .AddTeam(out var teamEntity);

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/teams/{teamEntity.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeam = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        returnedTeam.Should().NotBeNull();
        returnedTeam.Id.Should().Be(teamEntity.Id);
        returnedTeam.Name.Should().Be(teamEntity.Name);
        returnedTeam.EventId.Should().Be(eventEntity.Id);
    }

    [Test]
    public async Task GetTeamAsync_ShouldReturnNotFoundIfTeamDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser()
            .AddEvent();
        const int teamId = 1_000_000;

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/teams/{teamId}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}