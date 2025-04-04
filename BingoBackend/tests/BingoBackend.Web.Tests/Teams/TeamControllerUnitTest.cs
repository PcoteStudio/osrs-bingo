using AutoMapper;
using BingoBackend.Core.Features.Teams;
using BingoBackend.Core.Features.Teams.Exceptions;
using BingoBackend.Data.Entities;
using BingoBackend.Web.Teams;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BingoBackend.Web.Tests.Teams;

[TestFixture]
[TestOf(typeof(TeamController))]
public class TeamControllerUnitTest
{
    [SetUp]
    public void BeforeEach()
    {
        _teamServiceMock = new Mock<ITeamService>();
        _mapperMock = new Mock<IMapper>();
        _teamController = new TeamController(_teamServiceMock.Object, _mapperMock.Object);
    }

    private TeamController _teamController;
    private Mock<ITeamService> _teamServiceMock;
    private Mock<IMapper> _mapperMock;

    [Test]
    public async Task GetTeamsAsync_ShouldReturnAllTeams()
    {
        // Arrange
        var teams = Enumerable.Range(0, 3).Select(_ => new TeamEntity()).ToList();
        var teamResponses = teams.Select(_ => new TeamResponse()).ToList();
        _teamServiceMock.Setup(x => x.GetTeamsAsync())
            .ReturnsAsync(teams).Verifiable(Times.Once());
        _mapperMock.Setup(x => x.Map<List<TeamResponse>>(teams))
            .Returns(teamResponses).Verifiable(Times.Once());

        // Act
        var result = await _teamController.GetTeamsAsync();

        // Assert status code
        result.Result.Should().BeOfType<ObjectResult>();
        var objResult = (result.Result as ObjectResult)!;
        objResult.StatusCode.Should().Be(StatusCodes.Status200OK);

        // Assert response content
        objResult.Value.Should().BeOfType<List<TeamResponse>>();
        var actualTeamResponses = objResult.Value as List<TeamResponse>;
        actualTeamResponses.Should().BeSameAs(teamResponses);
    }

    [Test]
    public async Task GetTeamsAsync_ShouldReturnAnEmptyTeamList()
    {
        // Arrange
        var teams = new List<TeamEntity>();
        var teamResponses = new List<TeamResponse>();
        _teamServiceMock.Setup(x => x.GetTeamsAsync())
            .ReturnsAsync(teams).Verifiable(Times.Once());
        _mapperMock.Setup(x => x.Map<List<TeamResponse>>(teams))
            .Returns(teamResponses).Verifiable(Times.Once());

        // Act
        var result = await _teamController.GetTeamsAsync();

        // Assert status code
        result.Result.Should().BeOfType<ObjectResult>();
        var objResult = (result.Result as ObjectResult)!;
        objResult.StatusCode.Should().Be(StatusCodes.Status200OK);

        // Assert response content
        objResult.Value.Should().BeOfType<List<TeamResponse>>();
        var actualTeamResponses = objResult.Value as List<TeamResponse>;
        actualTeamResponses.Should().BeSameAs(teamResponses);
    }

    [Test]
    public async Task GetTeamAsync_ShouldReturnTheSpecifiedTeam()
    {
        // Arrange
        var team = new TeamEntity { Id = Random.Shared.Next() };
        var teamResponse = new TeamResponse();
        _teamServiceMock.Setup(x => x.GetTeamAsync(team.Id))
            .ReturnsAsync(team).Verifiable(Times.Once());
        _mapperMock.Setup(x => x.Map<TeamResponse>(team))
            .Returns(teamResponse).Verifiable(Times.Once());

        // Act
        var result = await _teamController.GetTeamAsync(team.Id);

        // Assert status code
        result.Result.Should().BeOfType<ObjectResult>();
        var objResult = (result.Result as ObjectResult)!;
        objResult.StatusCode.Should().Be(StatusCodes.Status200OK);

        // Assert response content
        objResult.Value.Should().BeOfType<TeamResponse>();
        var actualTeamResponse = objResult.Value as TeamResponse;
        actualTeamResponse.Should().BeSameAs(teamResponse);
    }

    [Test]
    [TestCaseSource(nameof(GetCommonTeamExceptionsAndExpectedStatusCode))]
    public async Task GetTeamAsync_ShouldReturnExpectedHttpStatusCodeOnKnownErrors(Exception exception,
        int expectedStatusCode)
    {
        var teamId = Random.Shared.Next();
        _teamServiceMock.Setup(x => x.GetTeamAsync(teamId))
            .ThrowsAsync(exception).Verifiable(Times.Once());

        // Act
        var result = await _teamController.GetTeamAsync(teamId);

        // Assert status code
        result.Result.Should().BeAssignableTo<StatusCodeResult>();
        var statusCodeResult = (result.Result as StatusCodeResult)!;
        statusCodeResult.StatusCode.Should().Be(expectedStatusCode);
    }

    private static IEnumerable<TestCaseData> GetCommonTeamExceptionsAndExpectedStatusCode()
    {
        // TODO Add forbidden access
        yield return new TestCaseData(new TeamNotFoundException(Random.Shared.Next()), StatusCodes.Status404NotFound);
    }
}