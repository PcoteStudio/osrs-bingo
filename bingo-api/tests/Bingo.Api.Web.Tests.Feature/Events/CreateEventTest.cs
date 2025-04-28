using System.Net;
using System.Text.Json;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataGenerators;
using Bingo.Api.Web.Events;
using FluentAssertions;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Events;

public partial class EventFeatureTest
{
    [Test]
    public async Task CreateEvent_ShouldReturnTheCreatedEvent()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("event.create");
        var args = TestDataGenerator.GenerateEventCreateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/events"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Created, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedEvent = JsonSerializer.Deserialize<EventResponse>(responseContent, JsonSerializerOptions.Web);
        returnedEvent.Should().NotBeNull();
        var savedEvent = _dbContext.Events.FirstOrDefault(t => t.Id == returnedEvent.Id);
        savedEvent.Should().NotBeNull();
        returnedEvent.Id.Should().Be(savedEvent.Id);
        returnedEvent.Name.Should().Be(savedEvent.Name);
    }

    [Test]
    public async Task CreateEvent_ShouldReturnForbiddenIfMissingPermissions()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets);
        var args = TestDataGenerator.GenerateEventCreateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/events"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task CreateEvent_ShouldReturnUnauthorizedIfNotLoggedIn()
    {
        // Arrange
        _testDataSetup
            .AddUser()
            .AddPermissions("event.create");
        var args = TestDataGenerator.GenerateEventCreateArguments();

        // Act
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/events"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Unauthorized, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}