using Bingo.Api.Core.Features.Players;
using Bingo.Api.Data;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataSetups;
using FluentAssertions;
using NUnit.Framework;

namespace Bingo.Api.Core.Tests.Integration.Features.Players;

[TestFixture]
[TestOf(typeof(PlayerRepository))]
public class PlayerRepositoryIntegrationTest
{
    [SetUp]
    public void BeforeEach()
    {
        _dbContext = TestSetupUtil.GetDbContext(BingoProjects.Core);
        _testDataSetup = TestSetupUtil.GetTestDataSetup(BingoProjects.Core);
        _playerRepository = new PlayerRepository(TestSetupUtil.GetDbContext(BingoProjects.Core));
    }

    [TearDown]
    public void AfterEach()
    {
        _dbContext.Dispose();
    }

    private ApplicationDbContext _dbContext;
    private TestDataSetup _testDataSetup;
    private IPlayerRepository _playerRepository;

    [Test]
    public async Task GetByNameAsync_ShouldReturnThePlayer()
    {
        // Arrange
        _testDataSetup
            .AddPlayers(Random.Shared.Next(4))
            .AddPlayer(out var player)
            .AddPlayers(Random.Shared.Next(3));

        // Act
        var actualPlayer = await _playerRepository.GetByNameAsync(player.Name);

        // Assert
        actualPlayer.Should().NotBeNull();
        actualPlayer.Id.Should().Be(player.Id);
    }

    [Test]
    public async Task GetByNameAsync_ShouldReturnNullIfPlayerDoesNotExist()
    {
        // Act
        var actualPlayer = await _playerRepository.GetByNameAsync("Unknown");

        // Assert
        actualPlayer.Should().BeNull();
    }

    [Test]
    public async Task GetByNamesAsync_ShouldReturnThePlayers()
    {
        // Arrange
        _testDataSetup
            .AddPlayers(1)
            .AddPlayers(3, out var players)
            .AddPlayers(2);
        var names = players.Select(p => p.Name).ToList();

        // Act
        var actualPlayers = await _playerRepository.GetByNamesAsync(names);

        // Assert
        actualPlayers.Should().NotBeNull();
        actualPlayers.Select(x => x.Id).Should().BeEquivalentTo(players.Select(x => x.Id));
    }

    [Test]
    public async Task GetByNamesAsync_ShouldReturnOnlyTheExistingPlayers()
    {
        // Arrange
        _testDataSetup
            .AddPlayers(1)
            .AddPlayers(3, out var players)
            .AddPlayers(2);
        var names = players.Select(p => p.Name).ToList();
        names[1] = "Unknown";
        players.RemoveAt(1);

        // Act
        var actualPlayers = await _playerRepository.GetByNamesAsync(names);

        // Assert
        actualPlayers.Should().NotBeNull();
        actualPlayers.Select(x => x.Id).Should().BeEquivalentTo(players.Select(x => x.Id));
    }

    [Test]
    public async Task GetByNamesAsync_ShouldReturnEmptyListIfPlayersDoNotExist()
    {
        // Arrange
        _testDataSetup.AddPlayers(5);
        var names = Enumerable.Range(0, 5).Select(i => "Unknown_" + i).ToList();

        // Act
        var actualPlayers = await _playerRepository.GetByNamesAsync(names);

        // Assert
        actualPlayers.Should().NotBeNull();
        actualPlayers.Should().BeEmpty();
    }
}