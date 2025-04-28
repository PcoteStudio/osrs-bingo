using System.Net;
using System.Text.Json;
using Bingo.Api.TestUtils;
using Bingo.Api.Web.Items;
using FluentAssertions;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Items;

public partial class ItemFeatureTest
{
    [Test]
    public async Task GetItemAsync_ShouldReturnTheSpecifiedItem()
    {
        // Arrange
        _testDataSetup.AddItem(out var itemEntity);

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/items/{itemEntity.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedItem = JsonSerializer.Deserialize<ItemResponse>(responseContent, JsonSerializerOptions.Web);
        returnedItem.Should().NotBeNull();
        returnedItem.Id.Should().Be(itemEntity.Id);
        returnedItem.Name.Should().Be(itemEntity.Name);
        returnedItem.Image.Should().Be(itemEntity.Image);
    }

    [Test]
    public async Task GetItemAsync_ShouldReturnNotFoundIfItemDoesNotExist()
    {
        // Arrange
        const int itemId = 1_000_000;

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/items/{itemId}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}