using AutoMapper;
using Bingo.Api.Core.Features.Teams;
using Bingo.Api.Core.Features.Teams.Exceptions;
using Bingo.Api.TestUtils.TestDataGenerators;
using Bingo.Api.Web.Teams;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Bingo.Api.Web.Tests.Teams;

[TestFixture]
[TestOf(typeof(TeamController))]
public class TeamControllerUnitTest
{
    [SetUp]
    public void BeforeEach()
    {
        _teamServiceMock = new Mock<ITeamService>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<TeamController>>();
        _teamController = new TeamController(_teamServiceMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    private TeamController _teamController;
    private Mock<ITeamService> _teamServiceMock;
    private Mock<IMapper> _mapperMock;
    private Mock<ILogger<TeamController>> _loggerMock;

    [Test]
    public async Task GetRequiredTeamAsync_ShouldReturnTheSpecifiedTeam()
    {
        // Arrange
        var team = TestDataGenerator.GenerateTeamEntity();
        var teamResponse = new TeamResponse();

        _teamServiceMock.Setup(x => x.GetRequiredTeamAsync(team.Id))
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
    public async Task GetRequiredTeamAsync_ShouldReturnExpectedHttpStatusCodeOnKnownErrors(Exception exception,
        int expectedStatusCode)
    {
        var teamId = Random.Shared.Next();
        _teamServiceMock.Setup(x => x.GetRequiredTeamAsync(teamId))
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