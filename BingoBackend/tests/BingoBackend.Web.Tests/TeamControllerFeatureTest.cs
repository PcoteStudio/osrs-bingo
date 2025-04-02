using System.Net;
using System.Net.Http.Headers;
using BingoBackend.Data;
using BingoBackend.TestUtils;
using BingoBackend.TestUtils.TestDataSetup;
using BingoBackend.TestUtils.TestMappingHelpers;
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
        TestSetup.RecreateDatabase();
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

        // Assert db changes
        var persistedTeams = _dbContext.Teams.ToList();
        Assert.That(persistedTeams, Has.Count.EqualTo(1));
        Assert.That(persistedTeams[0].Name, Is.EqualTo(teamArgs.Name));

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var expectedContent = TestMappingHelpers.TeamEntityToTeamResponseJson(persistedTeams[0]);
        Expect.EquivalentJsonWithPrettyOutput(responseContent, expectedContent);
    }

    [Test]
    public async Task ListTeams_ShouldReturnAllTeams()
    {
        // Arrange
        _testDataSetup.AddTeams(3, out var teamEntities);

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, "/api/teams"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert no db changes
        var persistedTeams = _dbContext.Teams.ToList();
        Assert.That(persistedTeams, Is.EqualTo(teamEntities));

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var expectedContent = TestMappingHelpers.TeamEntityArrayToTeamResponseJsonArray(teamEntities);
        Expect.EquivalentJsonWithPrettyOutput(responseContent, expectedContent);
    }
}