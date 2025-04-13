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

namespace Bingo.Api.Web.Tests.Teams;

public class TeamFeatureTest
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
    public async Task GetEventTeamsAsync_ShouldReturnTheSpecifiedTeam()
    {
        // Arrange
        _testDataSetup
            .AddUser()
            .AddEvent(out var eventEntity)
            .AddTeam(out var teamEntity);

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/teams/{teamEntity.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeam = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        returnedTeam.Should().NotBeNull();
        returnedTeam.Name.Should().Be(teamEntity.Name);
        returnedTeam.EventId.Should().Be(eventEntity.Id);
        returnedTeam.Id.Should().Be(teamEntity.Id);
    }

    [Test]
    public async Task GetTeamAsync_ShouldReturnNotFoundIfTeamDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser()
            .AddEvent();
        const int teamId = 1_000_000;

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/teams/{teamId}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var expectedContent = HttpResponseGenerator.GetExpectedJsonResponse(HttpStatusCode.NotFound);
        Expect.EquivalentJsonWithPrettyOutput(responseContent, expectedContent);
    }

    [Test]
    public async Task UpdateTeam_ShouldReturnTheUpdatedTeam()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent(out var eventEntity)
            .AddTeam(out var originalTeam);
        var teamArgs = TestDataGenerator.GenerateTeamUpdateArguments();
        var postContent = JsonSerializer.Serialize(teamArgs);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", userWithSecrets.AccessToken);

        // Act
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/teams/{originalTeam.Id}"),
            stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var updatedTeam = await _dbContext.Teams.FindAsync(originalTeam.Id);
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeam = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        returnedTeam.Should().NotBeNull();
        updatedTeam.Should().NotBeNull();
        returnedTeam.Name.Should().Be(teamArgs.Name).And.Subject.Should().Be(updatedTeam.Name);
        returnedTeam.Id.Should().Be(originalTeam.Id).And.Subject.Should().Be(updatedTeam.Id);
        returnedTeam.EventId.Should().Be(eventEntity.Id).And.Subject.Should().Be(updatedTeam.EventId);
    }

    [Test]
    public async Task UpdateTeam_ShouldReturnNotFoundIfTeamDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent();
        const int teamId = 1_000_000;
        var teamArgs = TestDataGenerator.GenerateTeamUpdateArguments();
        var postContent = JsonSerializer.Serialize(teamArgs);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", userWithSecrets.AccessToken);

        // Act
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/teams/{teamId}"),
            stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var expectedContent = HttpResponseGenerator.GetExpectedJsonResponse(HttpStatusCode.NotFound);
        Expect.EquivalentJsonWithPrettyOutput(responseContent, expectedContent);
    }

    [Test]
    public async Task AddTeamPlayers_ShouldReturnTheUpdatedTeam()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent()
            .AddTeam(out var teamEntity);
        var args = TestDataGenerator.GenerateTeamPlayersArguments(5);
        var postContent = JsonSerializer.Serialize(args);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", userWithSecrets.AccessToken);

        // Act
        var response = await _client.PostAsync(
            new Uri(_baseUrl, $"/api/teams/{teamEntity.Id}/players"),
            stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeam = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        Assert.That(returnedTeam, Is.Not.Null);
        Assert.That(returnedTeam.Players, Has.Count.EqualTo(args.PlayerNames.Count));
        Assert.That(returnedTeam.Players.Select(p => p.Name), Is.EquivalentTo(args.PlayerNames));
    }

    [Test]
    public async Task AddTeamPlayers_ShouldReturnNotFoundIfTeamDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent();
        const int teamId = 1_000_000;
        var args = TestDataGenerator.GenerateTeamPlayersArguments(5);
        var postContent = JsonSerializer.Serialize(args);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", userWithSecrets.AccessToken);

        // Act
        var response = await _client.PostAsync(
            new Uri(_baseUrl, $"/api/teams/{teamId}/players"),
            stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var expectedContent = HttpResponseGenerator.GetExpectedJsonResponse(HttpStatusCode.NotFound);
        Expect.EquivalentJsonWithPrettyOutput(responseContent, expectedContent);
    }
}