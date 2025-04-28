using System.Net;
using System.Text.Json;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataGenerators;
using Bingo.Api.Web.Teams;
using FluentAssertions;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Teams;

public partial class TeamFeatureTest
{
    [Test]
    public async Task UpdateTeamAsync_ShouldReturnTheUpdatedTeam()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("team.update")
            .AddEvent()
            .AddTeam(out var originalTeam);
        var args = TestDataGenerator.GenerateTeamUpdateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/teams/{originalTeam.Id}"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeam = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        var updatedTeam = await _dbContext.Teams.FindAsync(originalTeam.Id);
        returnedTeam.Should().NotBeNull();
        updatedTeam.Should().NotBeNull();
        returnedTeam.Name.Should().Be(updatedTeam.Name).And.Be(args.Name);
        returnedTeam.Id.Should().Be(updatedTeam.Id).And.Be(originalTeam.Id);
        returnedTeam.EventId.Should().Be(updatedTeam.EventId).And.Be(originalTeam.EventId);
    }

    [Test]
    public async Task UpdateTeamAsync_ShouldReturnNotFoundIfTeamDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent();
        const int teamId = 1_000_000;
        var args = TestDataGenerator.GenerateTeamUpdateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/teams/{teamId}"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task UpdateTeamAsync_ShouldReturnForbiddenIfNotTheTeamAdmin()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("team.update")
            .AddUser()
            .AddEvent()
            .AddTeam(out var originalTeam);
        var args = TestDataGenerator.GenerateTeamUpdateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/teams/{originalTeam.Id}"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task UpdateTeamAsync_ShouldReturnForbiddenIfMissingPermissions()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent()
            .AddTeam(out var originalTeam);
        var args = TestDataGenerator.GenerateTeamUpdateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/teams/{originalTeam.Id}"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task UpdateTeamAsync_ShouldReturnIfNotLoggedIn()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("team.update")
            .AddEvent()
            .AddTeam(out var originalTeam);
        var args = TestDataGenerator.GenerateTeamUpdateArguments();

        // Act
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/teams/{originalTeam.Id}"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Unauthorized, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}