using Bingo.Api.Core.Features.Teams;
using Bingo.Api.Data;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataSetups;
using FluentAssertions;
using NUnit.Framework;

namespace Bingo.Api.Core.Tests.Integration.Features.Teams;

[TestFixture]
[TestOf(typeof(TeamRepository))]
public class TeamRepositoryIntegrationTest
{
    [SetUp]
    public void BeforeEach()
    {
        _dbContext = TestSetupUtil.GetDbContext(BingoProjects.Core);
        _testDataSetup = TestSetupUtil.GetTestDataSetup(BingoProjects.Core);
        _teamRepository = new TeamRepository(TestSetupUtil.GetDbContext(BingoProjects.Core));
    }

    [TearDown]
    public void AfterEach()
    {
        _dbContext.Dispose();
    }

    private ApplicationDbContext _dbContext;
    private TestDataSetup _testDataSetup;
    private ITeamRepository _teamRepository;

    [Test]
    public async Task GetCompleteByIdAsync_ShouldReturnTheFullTeam()
    {
        // Arrange
        _testDataSetup
            .AddUser()
            .AddEvent()
            .AddTeam(out var team)
            .AddPlayers(2)
            .AddTeam()
            .AddPlayers(3);

        // Act
        var actualTeam = await _teamRepository.GetCompleteByIdAsync(team.Id);

        // Assert
        actualTeam.Should().NotBeNull();
        actualTeam.Id.Should().Be(team.Id);
        actualTeam.Players.Count.Should().Be(team.Players.Count);
        actualTeam.Players[0].Id.Should().Be(team.Players[0].Id);
        actualTeam.Players[1].Id.Should().Be(team.Players[1].Id);
        actualTeam.Event.Id.Should().Be(team.EventId);
    }

    [Test]
    public async Task GetCompleteByIdAsync_ShouldReturnNullIfTeamDoesNotExist()
    {
        // Act
        var actualTeam = await _teamRepository.GetCompleteByIdAsync(1_000_000);

        // Assert
        actualTeam.Should().BeNull();
    }
}