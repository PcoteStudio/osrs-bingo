using System.Net;
using System.Text.Json;
using Bingo.Api.TestUtils;
using Bingo.Api.Web.Drops;
using FluentAssertions;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Drops;

public partial class DropFeatureTest
{
    [Test]
    public async Task GetDropAsync_ShouldReturnTheSpecifiedDrop()
    {
        // Arrange
        _testDataSetup
            .AddNpc()
            .AddItem()
            .AddDrop(out var dropEntity);

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/drops/{dropEntity.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedDrop = JsonSerializer.Deserialize<DropResponse>(responseContent, JsonSerializerOptions.Web);
        returnedDrop.Should().NotBeNull();
        returnedDrop.Id.Should().Be(dropEntity.Id);
        returnedDrop.NpcId.Should().Be(dropEntity.NpcId);
        returnedDrop.ItemId.Should().Be(dropEntity.ItemId);
        returnedDrop.Ehc.Should().Be(dropEntity.Ehc).And.BePositive();
    }

    [Test]
    public async Task GetDropAsync_ShouldReturnNotFoundIfDropDoesNotExist()
    {
        // Arrange
        const int dropId = 1_000_000;

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/drops/{dropId}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}