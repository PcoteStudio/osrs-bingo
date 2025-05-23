﻿using System.Net;
using Bingo.Api.Core.Features.Authentication.Arguments;
using Bingo.Api.TestUtils;
using FluentAssertions;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Authentication;

public partial class AuthenticationFeatureTest
{
    [Test]
    public async Task LoginAsync_ShouldAutomaticallyGiveAccessToGetMe()
    {
        // Arrange
        _testDataSetup.AddUser(out var userWithSecrets);
        var args = new AuthLoginArguments
        {
            Username = userWithSecrets.User.Username,
            Password = userWithSecrets.Password
        };

        // Act
        var loginResponse = await _client.PostAsync(new Uri(_baseUrl, "/api/auth/login"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, loginResponse);

        // Assert response content
        var responseContent = await loginResponse.Content.ReadAsStringAsync();
        responseContent.Should().BeEmpty();

        loginResponse.Headers.Should().ContainKey("Set-Cookie");
        var setCookie = loginResponse.Headers.First(h => h.Key == "Set-Cookie");
        setCookie.Value.Count().Should().BeGreaterThan(0);
        setCookie.Value.First().Should().StartWith(".AspNetCore.Session=");

        // Act
        var getMeResponse = await _client.GetAsync(new Uri(_baseUrl, "/api/users/me"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, getMeResponse);
    }

    [Test]
    public async Task LoginAsync_ShouldReturnBadRequestOnInvalidCredentials()
    {
        // Arrange
        _testDataSetup.AddUser(out var userWithSecrets);
        var args = new AuthLoginArguments
        {
            Username = userWithSecrets.User.Username,
            Password = "wrong_password" + userWithSecrets.Password
        };

        // Act
        var loginResponse = await _client.PostAsync(new Uri(_baseUrl, "/api/auth/login"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.BadRequest, loginResponse);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(loginResponse);
        loginResponse.Headers.Should().NotContainKey("Set-Cookie");

        // Act
        var meResponse = await _client.GetAsync(new Uri(_baseUrl, "/api/users/me"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Unauthorized, meResponse);
    }
}