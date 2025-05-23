﻿using System.Net;
using System.Text.Json;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataGenerators;
using Bingo.Api.Web.Boards.MultiLayer;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Boards.MultiLayer;

public partial class MultiLayerBoardFeatureTest
{
    [Test]
    public async Task CreateMultiLayerBoard_ShouldReturnTheCreatedMultiLayerBoard()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("board.create")
            .AddEvent(out var eventEntity);
        var args = TestDataGenerator.GenerateMultiLayerBoardCreateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(new Uri(_baseUrl, $"/api/events/{eventEntity.Id}/mlboard"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Created, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedBoard =
            JsonSerializer.Deserialize<MultiLayerBoardResponse>(responseContent, JsonSerializerOptions.Web);
        returnedBoard.Should().NotBeNull();
        var savedBoard = _dbContext.MultiLayerBoards
            .Include(b => b.Layers)
            .FirstOrDefault(b => b.Id == returnedBoard.Id);
        savedBoard.Should().NotBeNull();
        returnedBoard.Id.Should().Be(savedBoard.Id);
        returnedBoard.Width.Should().Be(savedBoard.Width);
        returnedBoard.Height.Should().Be(savedBoard.Height);
        returnedBoard.Depth.Should().Be(savedBoard.Depth);
        returnedBoard.Layers.Count.Should().Be(savedBoard.Layers.Count);
        returnedBoard.Layers.Count.Should().Be(args.Depth);
    }

    [Test]
    public async Task CreateMultiLayerBoard_ShouldReturnForbiddenIfNotTheTeamAdmin()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("board.create")
            .AddUser()
            .AddEvent(out var eventEntity);
        var args = TestDataGenerator.GenerateMultiLayerBoardCreateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(new Uri(_baseUrl, $"/api/events/{eventEntity.Id}/mlboard"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task CreateMultiLayerBoard_ShouldReturnForbiddenIfMissingPermissions()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent(out var eventEntity);
        var args = TestDataGenerator.GenerateMultiLayerBoardCreateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(new Uri(_baseUrl, $"/api/events/{eventEntity.Id}/mlboard"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task CreateMultiLayerBoard_ShouldReturnUnauthorizedIfNotLoggedIn()
    {
        // Arrange
        _testDataSetup
            .AddUser()
            .AddPermissions("board.create")
            .AddEvent(out var eventEntity);
        var args = TestDataGenerator.GenerateMultiLayerBoardCreateArguments();

        // Act
        var response = await _client.PostAsync(new Uri(_baseUrl, $"/api/events/{eventEntity.Id}/mlboard"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Unauthorized, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}