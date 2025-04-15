using System.Net;
using System.Net.Http.Headers;
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
    public async Task AddTeamPlayersAsync_ShouldReturnTheUpdatedTeam()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent()
            .AddTeam(out var originalTeam);
        var args = TestDataGenerator.GenerateTeamPlayersArguments(5);
        var postContent = JsonSerializer.Serialize(args);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(
            new Uri(_baseUrl, $"/api/teams/{originalTeam.Id}/players"),
            stringContent);

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
    public async Task AddTeamPlayersAsync_ShouldReturnNotFoundIfTeamDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent();
        const int teamId = 1_000_000;
        var args = TestDataGenerator.GenerateTeamPlayersArguments(5);
        var postContent = JsonSerializer.Serialize(args);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(
            new Uri(_baseUrl, $"/api/teams/{teamId}/players"),
            stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var expectedContent = HttpResponseGenerator.GetExpectedJsonResponse(HttpStatusCode.NotFound);
        Expect.EquivalentJsonWithPrettyOutput(responseContent, expectedContent);
    }
}