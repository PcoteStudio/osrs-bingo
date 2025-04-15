using System.Net;
using System.Net.Http.Headers;
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
        var teamArgs = TestDataGenerator.GenerateTeamCreateArguments();
        var postContent = JsonSerializer.Serialize(teamArgs);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(new Uri(_baseUrl, $"/api/events/{eventEntity.Id}/teams"), stringContent);

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
}