using System.Net;
using System.Text.Json;
using Bingo.Api.TestUtils;
using Bingo.Api.Web.Events;
using Bingo.Api.Web.Teams;
using FluentAssertions;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Events;

public partial class EventFeatureTest
{
    [Test]
    public async Task GetEventAsync_ShouldReturnTheSpecifiedEvent()
    {
        // Arrange
        _testDataSetup
            .AddUser()
            .AddEvent()
            .AddEvent(out var eventEntity)
            .AddTeam(out var teamEntity1)
            .AddPlayers(Random.Shared.Next(3))
            .AddTeam(out var teamEntity2);

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/events/{eventEntity.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedEvent = JsonSerializer.Deserialize<EventResponse>(responseContent, JsonSerializerOptions.Web);
        returnedEvent.Should().NotBeNull();
        returnedEvent.Id.Should().Be(eventEntity.Id);
        returnedEvent.Name.Should().Be(eventEntity.Name);
        returnedEvent.Teams.Should().BeEquivalentTo([teamEntity1.ToResponse(), teamEntity2.ToResponse()]);
    }

    [Test]
    public async Task GetEventAsync_ShouldReturnNotFoundIfEventDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser()
            .AddEvent(out var eventEntity);
        const int eventId = 1_000_000;

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/events/{eventId}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}