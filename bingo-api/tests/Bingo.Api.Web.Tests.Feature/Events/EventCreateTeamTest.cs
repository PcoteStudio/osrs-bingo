using System.Net;
using System.Text.Json;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataGenerators;
using Bingo.Api.Web.Teams;
using FluentAssertions;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Events;

public partial class EventFeatureTest
{
    [Test]
    public async Task CreateTeam_ShouldReturnTheCreatedTeam()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent(out var eventEntity);
        var args = TestDataGenerator.GenerateTeamCreateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(new Uri(_baseUrl, $"/api/events/{eventEntity.Id}/teams"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Created, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeam = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        returnedTeam.Should().NotBeNull();
        var savedTeam = _dbContext.Teams.FirstOrDefault(t => t.Id == returnedTeam.Id);
        savedTeam.Should().NotBeNull();
        returnedTeam.Id.Should().Be(savedTeam.Id);
        returnedTeam.Name.Should().Be(savedTeam.Name);
    }

    [Test]
    public async Task CreateTeam_ShouldReturnForbiddenIfNotTheTeamAdmin()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddUser()
            .AddEvent(out var eventEntity);
        var args = TestDataGenerator.GenerateTeamCreateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(new Uri(_baseUrl, $"/api/events/{eventEntity.Id}/teams"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task CreateTeam_ShouldReturnUnauthorizedIfNotLoggedIn()
    {
        // Arrange
        _testDataSetup
            .AddUser()
            .AddEvent(out var eventEntity);
        var args = TestDataGenerator.GenerateTeamCreateArguments();

        // Act
        var response = await _client.PostAsync(new Uri(_baseUrl, $"/api/events/{eventEntity.Id}/teams"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Unauthorized, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}