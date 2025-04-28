using System.Net;
using System.Text.Json;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataGenerators;
using Bingo.Api.Web.Teams;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Teams;

public partial class TeamFeatureTest
{
    [Test]
    public async Task RemoveTeamPlayerAsync_ShouldReturnTheUpdatedTeam()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("team.update")
            .AddEvent()
            .AddTeam(out var originalTeam)
            .AddPlayers(Random.Shared.Next(3, 10), out var originalPlayers);

        var player = originalPlayers[Random.Shared.Next(originalPlayers.Count)];

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.DeleteAsync(
            new Uri(_baseUrl, $"/api/teams/{originalTeam.Id}/players/{player.Name}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeam = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        var updatedTeam = await _dbContext.Teams
            .Include(x => x.Players)
            .FirstAsync(x => x.Id == originalTeam.Id);
        returnedTeam.Should().NotBeNull();
        updatedTeam.Should().NotBeNull();
        returnedTeam.Players.Count.Should().Be(originalPlayers.Count - 1);
        returnedTeam.Players.Select(p => p.Name).Should()
            .BeEquivalentTo(originalPlayers.Select(p => p.Name).Except([player.Name]));
        returnedTeam.Id.Should().Be(updatedTeam.Id).And.Be(originalTeam.Id);
        returnedTeam.Name.Should().Be(updatedTeam.Name).And.Be(originalTeam.Name);
        returnedTeam.EventId.Should().Be(updatedTeam.EventId).And.Be(originalTeam.EventId);
        returnedTeam.Players.Select(p => p.Name).Should().BeEquivalentTo(updatedTeam.Players.Select(p => p.Name));
    }

    [Test]
    public async Task RemoveTeamPlayerAsync_ShouldReturnNotFoundIfTeamDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent();
        const int teamId = 1_000_000;
        var playerName = TestDataGenerator.GeneratePlayerName();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.DeleteAsync(new Uri(_baseUrl, $"/api/teams/{teamId}/players/{playerName}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task RemoveTeamPlayerAsync_ShouldReturnForbiddenIfNotTheTeamAdmin()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("team.update")
            .AddUser()
            .AddEvent()
            .AddTeam(out var originalTeam)
            .AddPlayers(Random.Shared.Next(3, 10), out var originalPlayers);

        var player = originalPlayers[Random.Shared.Next(originalPlayers.Count)];

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.DeleteAsync(
            new Uri(_baseUrl, $"/api/teams/{originalTeam.Id}/players/{player.Name}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task RemoveTeamPlayerAsync_ShouldReturnForbiddenIfMissingPermissions()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent()
            .AddTeam(out var originalTeam)
            .AddPlayers(Random.Shared.Next(3, 10), out var originalPlayers);

        var player = originalPlayers[Random.Shared.Next(originalPlayers.Count)];

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.DeleteAsync(
            new Uri(_baseUrl, $"/api/teams/{originalTeam.Id}/players/{player.Name}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task RemoveTeamPlayerAsync_ShouldReturnUnauthorizedIfNotLoggedIn()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("team.update")
            .AddEvent()
            .AddTeam(out var originalTeam)
            .AddPlayers(Random.Shared.Next(3, 10), out var originalPlayers);

        var player = originalPlayers[Random.Shared.Next(originalPlayers.Count)];

        // Act
        var response = await _client.DeleteAsync(
            new Uri(_baseUrl, $"/api/teams/{originalTeam.Id}/players/{player.Name}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Unauthorized, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}