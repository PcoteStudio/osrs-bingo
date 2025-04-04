using AutoMapper;
using BingoBackend.Core.Features.Teams;
using BingoBackend.TestUtils.TestDataSetup;
using BingoBackend.Web.Teams;

namespace BingoBackend.Web.Tests.Teams;

[TestFixture]
[TestOf(typeof(TeamResponseMappingProfile))]
public class TeamMappingProfileUnitTest
{
    [OneTimeSetUp]
    public void BeforeAll()
    {
        _teamFactory = new TeamFactory();
        _mapper = new MapperConfiguration(
            c => c.AddMaps(typeof(TeamResponseMappingProfile).Assembly)
        ).CreateMapper();
    }

    private IMapper _mapper;
    private ITeamFactory _teamFactory;

    [Test]
    public void TeamEntity_ShouldBeProperlyMappedToATeamResponse()
    {
        // Arrange
        var teamEntity = _teamFactory.Create(TestDataSetup.GenerateTeamCreateArguments());

        // Act
        var teamResponse = _mapper.Map<TeamResponse>(teamEntity);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(teamResponse.Id, Is.EqualTo(teamEntity.Id));
            Assert.That(teamResponse.Name, Is.EqualTo(teamEntity.Name));
        });
    }
}