using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using BingoBackend.Data;
using BingoBackend.TestUtils;
using BingoBackend.TestUtils.TestDataSetup;
using BingoBackend.TestUtils.TestMappingHelpers;
using BingoBackend.Web.Team;
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
        var postContent = /* language=json */
            $$"""{ "name": "{{teamArgs.Name}}" }""";
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/teams"), stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Created, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var teamResponse = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        Assert.That(teamResponse, Is.Not.Null);
        var savedTeam = await _dbContext.Teams.FindAsync(teamResponse.Id);
        Assert.That(savedTeam, Is.Not.Null);
        Assert.That(savedTeam.Name, Is.EqualTo(teamArgs.Name));
        var expectedContent = TestMappingHelpers.TeamEntityToTeamResponseJson(savedTeam);
        Expect.EquivalentJsonWithPrettyOutput(responseContent, expectedContent);
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
        var expectedContent = TestMappingHelpers.TeamEntityArrayToTeamResponseJsonArray(persistedTeams);
        Expect.EquivalentJsonWithPrettyOutput(responseContent, expectedContent);
    }

    [Test]
    public async Task GetTeam_ShouldReturnTheSpecifiedTeam()
    {
        // Arrange
        _testDataSetup.AddTeams(10, out var teamEntities);
        var targetTeam = teamEntities[Random.Shared.Next(teamEntities.Count)];

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/teams/{targetTeam.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var expectedContent = TestMappingHelpers.TeamEntityToTeamResponseJson(targetTeam);
        Expect.EquivalentJsonWithPrettyOutput(responseContent, expectedContent);
    }

    [Test]
    public async Task GetTeam_ShouldReturnNotFoundIfTeamDoesNotExist()
    {
        // Arrange
        _testDataSetup.AddTeams(10, out var teamEntities);
        var teamId = Random.Shared.Next(teamEntities.Count * 1_000, teamEntities.Count * 10_000);

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/teams/{teamId}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var expectedContent = HttpResponseGenerator.GetExpectedJsonResponse(HttpStatusCode.NotFound);
        Expect.EquivalentJsonWithPrettyOutput(responseContent, expectedContent);
    }
}