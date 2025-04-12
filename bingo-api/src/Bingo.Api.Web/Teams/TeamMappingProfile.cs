using AutoMapper;
using Bingo.Api.Data.Entities.Events;

namespace Bingo.Api.Web.Teams;

public class TeamMappingProfile : Profile
{
    public TeamMappingProfile()
    {
        CreateMap<TeamEntity, TeamResponse>();
    }
}