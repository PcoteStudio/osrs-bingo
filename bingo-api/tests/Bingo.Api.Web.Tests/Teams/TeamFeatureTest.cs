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
using Microsoft.EntityFrameworkCore;

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

    #region RemoveTeamPlayerAsync

    [Test]
    public async Task RemoveTeamPlayerAsync_ShouldReturnTheUpdatedTeam()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent()
            .AddTeam(out var originalTeam)
            .AddPlayers(Random.Shared.Next(3, 10), out var originalPlayers);

        var player = originalPlayers[Random.Shared.Next(originalPlayers.Count)];

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.DeleteAsync(
            new Uri(_baseUrl, $"/api/teams/{originalTeam.Id}/players/{player.Name}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeam = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        returnedTeam.Should().NotBeNull();
        returnedTeam.Players.Count.Should().Be(originalPlayers.Count - 1);
        returnedTeam.Players.Select(p => p.Name).Should()
            .BeEquivalentTo(originalPlayers.Select(p => p.Name).Except([player.Name]));

        // Assert db content
        var updatedTeam = await _dbContext.Teams
            .Include(x => x.Players)
            .FirstAsync(x => x.Id == originalTeam.Id);
        updatedTeam.Should().NotBeNull();
        updatedTeam.Name.Should().Be(returnedTeam.Name);
        updatedTeam.Id.Should().Be(returnedTeam.Id);
        updatedTeam.EventId.Should().Be(returnedTeam.EventId);
        updatedTeam.Players.Select(p => p.Name).Should().BeEquivalentTo(returnedTeam.Players.Select(p => p.Name));
    }

    [Test]
    public async Task RemoveTeamPlayerAsync_ShouldReturnNotFoundIfTeamDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent();
        const int teamId = 1_000_000;
        var playerName = TestDataGenerator.GeneratePlayerName();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.DeleteAsync(new Uri(_baseUrl, $"/api/teams/{teamId}/players/{playerName}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var expectedContent = HttpResponseGenerator.GetExpectedJsonResponse(HttpStatusCode.NotFound);
        Expect.EquivalentJsonWithPrettyOutput(responseContent, expectedContent);
    }

    #endregion

    #region UpdateTeamPlayersAsync

    [Test]
    public async Task UpdateTeamPlayersAsync_ShouldReturnTheUpdatedTeam()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent()
            .AddTeam(out var originalTeam)
            .AddPlayers(Random.Shared.Next(3), out var originalPlayers);

        var args = TestDataGenerator.GenerateTeamPlayersArguments(Random.Shared.Next(5));
        var postContent = JsonSerializer.Serialize(args);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(
            new Uri(_baseUrl, $"/api/teams/{originalTeam.Id}/players"),
            stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeam = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        returnedTeam.Should().NotBeNull();
        returnedTeam.Players.Count.Should().Be(args.PlayerNames.Count);
        returnedTeam.Players.Select(p => p.Name).Should().BeEquivalentTo(args.PlayerNames);

        // Assert db content
        var updatedTeam = await _dbContext.Teams
            .Include(x => x.Players)
            .FirstAsync(x => x.Id == originalTeam.Id);
        updatedTeam.Should().NotBeNull();
        updatedTeam.Name.Should().Be(returnedTeam.Name);
        updatedTeam.Id.Should().Be(returnedTeam.Id);
        updatedTeam.EventId.Should().Be(returnedTeam.EventId);
        updatedTeam.Players.Select(p => p.Name).Should().BeEquivalentTo(args.PlayerNames);
    }

    [Test]
    public async Task UpdateTeamPlayersAsync_ShouldReturnNotFoundIfTeamDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent();
        const int teamId = 1_000_000;
        var args = TestDataGenerator.GenerateTeamPlayersArguments(Random.Shared.Next(5));
        var postContent = JsonSerializer.Serialize(args);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(
            new Uri(_baseUrl, $"/api/teams/{teamId}/players"),
            stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var expectedContent = HttpResponseGenerator.GetExpectedJsonResponse(HttpStatusCode.NotFound);
        Expect.EquivalentJsonWithPrettyOutput(responseContent, expectedContent);
    }

    #endregion

    #region AddTeamPlayersAsync

    [Test]
    public async Task AddTeamPlayersAsync_ShouldReturnTheUpdatedTeam()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent()
            .AddTeam(out var originalTeam);
        var args = TestDataGenerator.GenerateTeamPlayersArguments(5);
        var postContent = JsonSerializer.Serialize(args);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(
            new Uri(_baseUrl, $"/api/teams/{originalTeam.Id}/players"),
            stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeam = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        returnedTeam.Should().NotBeNull();
        returnedTeam.Players.Count.Should().Be(args.PlayerNames.Count);
        returnedTeam.Players.Select(p => p.Name).Should().BeEquivalentTo(args.PlayerNames);

        // Assert db content
        var updatedTeam = await _dbContext.Teams
            .Include(x => x.Players)
            .FirstAsync(x => x.Id == originalTeam.Id);
        updatedTeam.Should().NotBeNull();
        updatedTeam.Name.Should().Be(returnedTeam.Name);
        updatedTeam.Id.Should().Be(returnedTeam.Id);
        updatedTeam.EventId.Should().Be(returnedTeam.EventId);
        updatedTeam.Players.Select(p => p.Name).Should().BeEquivalentTo(args.PlayerNames);
    }

    [Test]
    public async Task AddTeamPlayersAsync_ShouldReturnNotFoundIfTeamDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent();
        const int teamId = 1_000_000;
        var args = TestDataGenerator.GenerateTeamPlayersArguments(5);
        var postContent = JsonSerializer.Serialize(args);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
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

    #endregion

    #region UpdateTeamAsync

    [Test]
    public async Task UpdateTeamAsync_ShouldReturnTheUpdatedTeam()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent()
            .AddTeam(out var originalTeam);
        var teamArgs = TestDataGenerator.GenerateTeamUpdateArguments();
        var postContent = JsonSerializer.Serialize(teamArgs);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/teams/{originalTeam.Id}"),
            stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedTeam = JsonSerializer.Deserialize<TeamResponse>(responseContent, JsonSerializerOptions.Web);
        returnedTeam.Should().NotBeNull();
        returnedTeam.Name.Should().Be(teamArgs.Name);
        returnedTeam.Id.Should().Be(originalTeam.Id);
        returnedTeam.EventId.Should().Be(originalTeam.EventId);

        // Assert db content
        var updatedTeam = await _dbContext.Teams.FindAsync(originalTeam.Id);
        updatedTeam.Should().NotBeNull();
        updatedTeam.Name.Should().Be(returnedTeam.Name);
        updatedTeam.Id.Should().Be(returnedTeam.Id);
        updatedTeam.EventId.Should().Be(returnedTeam.EventId);
    }

    [Test]
    public async Task UpdateTeamAsync_ShouldReturnNotFoundIfTeamDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddEvent();
        const int teamId = 1_000_000;
        var teamArgs = TestDataGenerator.GenerateTeamUpdateArguments();
        var postContent = JsonSerializer.Serialize(teamArgs);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/teams/{teamId}"),
            stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var expectedContent = HttpResponseGenerator.GetExpectedJsonResponse(HttpStatusCode.NotFound);
        Expect.EquivalentJsonWithPrettyOutput(responseContent, expectedContent);
    }

    #endregion

    #region GetTeamAsync

    [Test]
    public async Task GetTeamAsync_ShouldReturnTheSpecifiedTeam()
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

    #endregion
}