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
        _testDataSetup = new TestDataSetup(_dbContext);
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
        _testDataSetup.AddEvent(out var eventEntity);
        var teamArgs = TestDataGenerator.GenerateTeamCreateArguments();
        var postContent = JsonSerializer.Serialize(teamArgs);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

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

    [Test]
    public async Task GetEventTeamsAsync_ShouldReturnAllTeams()
    {
        // Arrange
        _testDataSetup
            .AddEvent(out var eventEntity)
            .AddTeams(3, out var teams);

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/events/{eventEntity.Id}/teams"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeams = JsonSerializer.Deserialize<List<TeamResponse>>(responseContent, JsonSerializerOptions.Web);
        returnedTeams.Should().NotBeNull();
        returnedTeams.Count.Should().Be(teams.Count);
        returnedTeams.Select(x => x.Id).Should().BeEquivalentTo(teams.Select(x => x.Id));
    }

    [Test]
    public async Task GetEventTeamsAsync_ShouldReturnTheSpecifiedTeam()
    {
        // Arrange
        _testDataSetup
            .AddEvent(out var eventEntity)
            .AddTeam(out var teamEntity);

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/events/{eventEntity.Id}/teams/{teamEntity.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeam = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        returnedTeam.Should().NotBeNull();
        returnedTeam.Name.Should().Be(teamEntity.Name);
        returnedTeam.EventId.Should().Be(eventEntity.Id);
        returnedTeam.Id.Should().Be(teamEntity.Id);
        returnedTeam.Players.Count.Should().Be(teamEntity.Players.Count);
    }

    [Test]
    public async Task GetEventTeamsAsync_ShouldReturnNotFoundIfTeamDoesNotExist()
    {
        // Arrange
        _testDataSetup.AddEvent(out var eventEntity);
        const int teamId = 1_000_000;

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/events/{eventEntity.Id}/teams/{teamId}"));

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
            .AddEvent(out var eventEntity)
            .AddTeam(out var teamEntity);
        var teamArgs = TestDataGenerator.GenerateTeamUpdateArguments();
        var postContent = JsonSerializer.Serialize(teamArgs);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/events/{eventEntity.Id}/teams/{teamEntity.Id}"),
            stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeam = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        returnedTeam.Should().NotBeNull();
        returnedTeam.Name.Should().Be(teamArgs.Name);
        returnedTeam.EventId.Should().Be(eventEntity.Id);
        returnedTeam.Id.Should().Be(teamEntity.Id);
        returnedTeam.Players.Count.Should().Be(teamEntity.Players.Count);
    }

    [Test]
    public async Task UpdateTeam_ShouldReturnNotFoundIfTeamDoesNotExist()
    {
        // Arrange
        _testDataSetup.AddEvent(out var eventEntity);
        const int teamId = 1_000_000;
        var teamArgs = TestDataGenerator.GenerateTeamUpdateArguments();
        var postContent = JsonSerializer.Serialize(teamArgs);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/events/{eventEntity.Id}/teams/{teamId}"),
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
            .AddEvent(out var eventEntity)
            .AddTeam(out var teamEntity);
        var args = TestDataGenerator.GenerateTeamPlayersArguments(5);
        var postContent = JsonSerializer.Serialize(args);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        var response = await _client.PostAsync(
            new Uri(_baseUrl, $"/api/events/{eventEntity.Id}/teams/{teamEntity.Id}/players"),
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
        _testDataSetup.AddEvent(out var eventEntity);
        const int teamId = 1_000_000;
        var args = TestDataGenerator.GenerateTeamPlayersArguments(5);
        var postContent = JsonSerializer.Serialize(args);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        var response = await _client.PostAsync(
            new Uri(_baseUrl, $"/api/events/{eventEntity.Id}/teams/{teamId}/players"),
            stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var expectedContent = HttpResponseGenerator.GetExpectedJsonResponse(HttpStatusCode.NotFound);
        Expect.EquivalentJsonWithPrettyOutput(responseContent, expectedContent);
    }
}