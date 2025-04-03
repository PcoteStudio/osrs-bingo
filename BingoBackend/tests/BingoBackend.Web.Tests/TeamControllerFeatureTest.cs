using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using BingoBackend.Data;
using BingoBackend.TestUtils;
using BingoBackend.TestUtils.TestDataSetup;
using BingoBackend.Web.Teams;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;

namespace BingoBackend.Web.Tests;

public class Tests
{
    private Uri _baseUrl;
    private HttpClient _client;
    private ApplicationDbContext _dbContext;
    private IWebHost _host;
    private TestDataSetup _testDataSetup;

    [OneTimeSetUp]
    public void BeforeAll()
    {
        TestSetup.RecreateDatabase();

        // Add a random amount of teams to simulate an existing DB
        var testDataSetup = new TestDataSetup(TestSetup.GetApplicationDbContext());
        testDataSetup.AddTeams(Random.Shared.Next(5, 15), out var teamEntities);

        _host = TestSetup.BuildWebHost();
        _host.Start();
        _baseUrl = TestSetup.GetRequiredHostUri(_host);
    }

    [OneTimeTearDown]
    public void AfterAll()
    {
        _host.Dispose();
    }

    [SetUp]
    public void BeforeEach()
    {
        _dbContext = TestSetup.GetApplicationDbContext();
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
        var teamArgs = TestDataSetup.GenerateTeamCreateArguments();
        var postContent = JsonSerializer.Serialize(teamArgs);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/teams"), stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Created, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeam = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        Assert.That(returnedTeam, Is.Not.Null);
        var savedTeam = await _dbContext.Teams.FindAsync(returnedTeam.Id);
        returnedTeam.Should().BeEquivalentTo(savedTeam);
    }

    [Test]
    public async Task ListTeams_ShouldReturnAllTeams()
    {
        // Arrange
        _testDataSetup.AddTeams(3);

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, "/api/teams"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var persistedTeams = _dbContext.Teams.ToList();
        Assert.That(persistedTeams, Has.Count.GreaterThanOrEqualTo(3));
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeams = JsonSerializer.Deserialize<List<TeamResponse>>(responseContent, JsonSerializerOptions.Web);
        returnedTeams.Should().BeEquivalentTo(persistedTeams);
    }

    [Test]
    public async Task GetTeam_ShouldReturnTheSpecifiedTeam()
    {
        // Arrange
        _testDataSetup.AddTeam(out var teamEntity);

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/teams/{teamEntity.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeam = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        Assert.That(returnedTeam, Is.Not.Null);
        returnedTeam.Should().BeEquivalentTo(teamEntity);
    }

    [Test]
    public async Task GetTeam_ShouldReturnNotFoundIfTeamDoesNotExist()
    {
        // Arrange
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
        _testDataSetup.AddTeam(out var teamEntity);
        var teamArgs = TestDataSetup.GenerateTeamUpdateArguments();
        teamEntity.Name = teamArgs.Name;
        var postContent = JsonSerializer.Serialize(teamArgs);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/teams/{teamEntity.Id}"), stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeam = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        Assert.That(returnedTeam, Is.Not.Null);
        returnedTeam.Should().BeEquivalentTo(teamEntity);
    }

    [Test]
    public async Task UpdateTeam_ShouldReturnNotFoundIfTeamDoesNotExist()
    {
        // Arrange
        const int teamId = 1_000_000;
        var teamArgs = TestDataSetup.GenerateTeamUpdateArguments();
        var postContent = JsonSerializer.Serialize(teamArgs);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/teams/{teamId}"), stringContent);

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
        _testDataSetup.AddTeam(out var teamEntity);
        var args = TestDataSetup.GenerateTeamPlayersArguments(5);
        var postContent = JsonSerializer.Serialize(args);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        var response = await _client.PostAsync(new Uri(_baseUrl, $"/api/teams/{teamEntity.Id}/players"), stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeam = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        Assert.That(returnedTeam, Is.Not.Null);
        // Assert.That(returnedTeam.Players, Has.Count.EqualTo(args.PlayerNames.Count));
        // TODO Validate names
    }

    [Test]
    public async Task AddTeamPlayers_ShouldReturnNotFoundIfTeamDoesNotExist()
    {
        // Arrange
        const int teamId = 1_000_000;
        var args = TestDataSetup.GenerateTeamPlayersArguments(5);
        var postContent = JsonSerializer.Serialize(args);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        var response = await _client.PostAsync(new Uri(_baseUrl, $"/api/teams/{teamId}/players"), stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var expectedContent = HttpResponseGenerator.GetExpectedJsonResponse(HttpStatusCode.NotFound);
        Expect.EquivalentJsonWithPrettyOutput(responseContent, expectedContent);
    }
}