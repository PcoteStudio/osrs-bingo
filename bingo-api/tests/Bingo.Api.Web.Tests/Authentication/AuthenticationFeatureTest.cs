using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Bingo.Api.Core.Features.Authentication.Arguments;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataSetup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

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
        _testDataSetup = new TestDataSetup(
            _dbContext,
            _host.Services.GetRequiredService<UserManager<UserEntity>>(),
            _host.Services.GetRequiredService<RoleManager<IdentityRole>>()
        );
        _client = new HttpClient();
    }

    [TearDown]
    public void AfterEach()
    {
        _dbContext.Dispose();
        _client.Dispose();
    }

    [Test]
    public async Task Login_ShouldReturnNewTokens()
    {
        // Arrange
        var loginArgs = new AuthLoginArguments
        {
            Username = "user@local.host",
            Password = "Password1!"
        };
        _testDataSetup.AddUser(out var user, new TestDataSetup.AddUserArguments
        {
            Name = "user",
            Email = loginArgs.Username,
            Password = loginArgs.Password
        });
        var postContent = JsonSerializer.Serialize(loginArgs);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/auth/login"), stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);
    }
}