using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Bingo.Api.Core.Features.Authentication.Arguments;
using Bingo.Api.Data;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataSetups;
using Bingo.Api.Web.Authentication;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;

namespace Bingo.Api.Web.Tests.Authentication;

public class AuthenticationFeatureTest
{
    private Uri _baseUrl;
    private HttpClient _client;
    private ApplicationDbContext _dbContext;
    private IWebHost _host;
    private TestDataSetup _testDataSetup;

    [OneTimeSetUp]
    public async Task BeforeAll()
    {
        _host = TestSetupUtil.BuildWebHost();
        await _host.StartAsync();
        _baseUrl = TestSetupUtil.GetRequiredHostUri(_host);
    }

    [OneTimeTearDown]
    public void AfterAll()
    {
        _host.Dispose();
    }

    [SetUp]
    public void BeforeEach()
    {
        _dbContext = TestSetupUtil.GetDbContext(BingoProjects.Web);
        _testDataSetup = TestSetupUtil.GetTestDataSetup(BingoProjects.Web);
        _client = new HttpClient();
    }

    [TearDown]
    public void AfterEach()
    {
        _dbContext.Dispose();
        _client.Dispose();
    }

    [Test]
    [Ignore("TODO Sessions")]
    public async Task Login_ShouldReturnNewTokens()
    {
        // Arrange
        _testDataSetup.AddUser(out var userWithPassword);
        var loginArgs = new AuthLoginArguments
        {
            Username = userWithPassword.User.UserName!,
            Password = userWithPassword.Password
        };
        var postContent = JsonSerializer.Serialize(loginArgs);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        var loginResponse = await _client.PostAsync(new Uri(_baseUrl, "/api/auth/login"), stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, loginResponse);

        // Assert response content
        var responseContent = await loginResponse.Content.ReadAsStringAsync();
        var returnedTokens = JsonSerializer.Deserialize<TokenResponse>(responseContent, JsonSerializerOptions.Web);
        returnedTokens.Should().NotBeNull();
        returnedTokens.AccessToken.Length.Should().BeGreaterThan(0);

        loginResponse.Headers.Should().ContainKey("Set-Cookie");
        var setCookie = loginResponse.Headers.First(h => h.Key == "Set-Cookie");
        setCookie.Value.Count().Should().BeGreaterThan(0);
        setCookie.Value.First().Should().StartWith("refresh_token=");

        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", returnedTokens.AccessToken);

        // Act
        var meResponse = await _client.GetAsync(new Uri(_baseUrl, "/api/users/me"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, meResponse);
    }
}