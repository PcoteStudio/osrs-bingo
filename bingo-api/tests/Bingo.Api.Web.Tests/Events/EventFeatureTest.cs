using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Bingo.Api.Data;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataGenerators;
using Bingo.Api.TestUtils.TestDataSetups;
using Bingo.Api.Web.Teams;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;

namespace Bingo.Api.Web.Tests.Events;

public class EventFeatureTest
{
    private Uri _baseUrl;
    private HttpClient _client;
    private ApplicationDbContext _dbContext;
    private IWebHost _host;
    private TestDataSetup _testDataSetup;

    [OneTimeSetUp]
    public void BeforeAll()
    {
        _host = TestSetupUtil.BuildWebHost();
        _host.Start();
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
    public async Task CreateTeam_ShouldReturnTheCreatedTeam()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent(out var eventEntity);
        var teamArgs = TestDataGenerator.GenerateTeamCreateArguments();
        var postContent = JsonSerializer.Serialize(teamArgs);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", userWithSecrets.AccessToken);

        // Act
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