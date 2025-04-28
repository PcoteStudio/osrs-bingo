using System.Net;
using System.Text.Json;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataGenerators;
using Bingo.Api.Web.Items;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Items;

public partial class ItemFeatureTest
{
    [Test]
    public async Task CreateItem_ShouldReturnTheCreatedItem()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("item.create");
        var args = TestDataGenerator.GenerateItemCreateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/items"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Created, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedItem = JsonSerializer.Deserialize<ItemResponse>(responseContent, JsonSerializerOptions.Web);
        returnedItem.Should().NotBeNull();
        var savedItem = _dbContext.Items.Include(i => i.Drops).FirstOrDefault(t => t.Id == returnedItem.Id);
        savedItem.Should().NotBeNull();
        returnedItem.Id.Should().Be(savedItem.Id);
        returnedItem.Name.Should().Be(savedItem.Name).And.Be(args.Name);
        returnedItem.Image.Should().Be(savedItem.Image).And.Be(args.Image);
        returnedItem.Drops.Count.Should().Be(savedItem.Drops.Count).And.Be(0);
    }

    [Test]
    public async Task CreateItem_ShouldReturnForbiddenIfMissingPermissions()
    {
        // Arrange
        _testDataSetup.AddUser(out var userWithSecrets);
        var args = TestDataGenerator.GenerateItemCreateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/items"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task CreateItem_ShouldReturnUnauthorizedIfNotLoggedIn()
    {
        // Arrange
        _testDataSetup.AddUser();
        var args = TestDataGenerator.GenerateItemCreateArguments();

        // Act
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/items"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Unauthorized, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}