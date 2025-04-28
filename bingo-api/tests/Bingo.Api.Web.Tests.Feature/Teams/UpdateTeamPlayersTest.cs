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
    public async Task UpdateTeamPlayersAsync_ShouldReturnTheUpdatedTeam()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("team.update")
            .AddEvent()
            .AddTeam(out var originalTeam)
            .AddPlayers(Random.Shared.Next(3));

        var args = TestDataGenerator.GenerateTeamPlayersArguments(Random.Shared.Next(5));

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/teams/{originalTeam.Id}/players"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeam = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        returnedTeam.Should().NotBeNull();
        returnedTeam.Players.Count.Should().Be(args.PlayerNames.Count);
        returnedTeam.Players.Select(p => p.Name).Should().BeEquivalentTo(args.PlayerNames);

        // Assert db content
        var updatedTeam = await _dbContext.Teams
            .Include(x => x.Players)
            .FirstAsync(x => x.Id == originalTeam.Id);
        updatedTeam.Should().NotBeNull();
        updatedTeam.Name.Should().Be(returnedTeam.Name);
        updatedTeam.Id.Should().Be(returnedTeam.Id);
        updatedTeam.EventId.Should().Be(returnedTeam.EventId);
        updatedTeam.Players.Select(p => p.Name).Should().BeEquivalentTo(args.PlayerNames);
    }

    [Test]
    public async Task UpdateTeamPlayersAsync_ShouldReturnNotFoundIfTeamDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("team.update")
            .AddEvent();
        const int teamId = 1_000_000;
        var args = TestDataGenerator.GenerateTeamPlayersArguments(Random.Shared.Next(5));

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/teams/{teamId}/players"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }


    [Test]
    public async Task UpdateTeamPlayersAsync_ShouldReturnForbiddenIfNotTheTeamAdmin()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("team.update")
            .AddUser()
            .AddEvent()
            .AddTeam(out var team);
        var args = TestDataGenerator.GenerateTeamPlayersArguments(Random.Shared.Next(5));

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/teams/{team.Id}/players"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task UpdateTeamPlayersAsync_ShouldReturnForbiddenIfMissingPermissions()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent()
            .AddTeam(out var team);
        var args = TestDataGenerator.GenerateTeamPlayersArguments(Random.Shared.Next(5));

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/teams/{team.Id}/players"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task UpdateTeamPlayersAsync_ShouldReturnUnauthorizedIfNotLoggedIn()
    {
        // Arrange
        _testDataSetup
            .AddUser()
            .AddPermissions("team.update")
            .AddEvent()
            .AddTeam(out var team);
        var args = TestDataGenerator.GenerateTeamPlayersArguments(Random.Shared.Next(5));

        // Act
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/teams/{team.Id}/players"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Unauthorized, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}