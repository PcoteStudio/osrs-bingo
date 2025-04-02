using AutoMapper;
using BingoBackend.Core.Features.Team;
using BingoBackend.TestUtils.TestDataSetup;

namespace BingoBackend.Core.Tests.Features.Team;

[TestFixture]
[TestOf(typeof(TeamEntityMappingProfile))]
public class TeamEntityMappingProfileTest
{
    [OneTimeSetUp]
    public void BeforeAll()
    {
        _teamFactory = new TeamFactory();
        _mapper = new MapperConfiguration(
            c => c.AddMaps(typeof(TeamEntityMappingProfile).Assembly)
        ).CreateMapper();
    }

    private IMapper _mapper;
    private ITeamFactory _teamFactory;

    [Test]
    public void TeamEntityToTeam_ShouldReturnAProperlyMappedTeam()
    {
        // Arrange
        var teamEntity = _teamFactory.Create(TestDataSetup.GenerateTeamCreateArguments());

        // Act
        var team = _mapper.Map<Core.Features.Team.Team>(teamEntity);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(team.Id, Is.EqualTo(teamEntity.Id));
            Assert.That(team.Name, Is.EqualTo(teamEntity.Name));
        });
    }
}