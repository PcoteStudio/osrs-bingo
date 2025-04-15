using AutoMapper;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Web.Teams;

public class TeamMappingProfile : Profile
{
    public TeamMappingProfile()
    {
        CreateMap<TeamEntity, TeamResponse>().PreserveReferences();
        CreateMap<TeamEntity, TeamShortResponse>().PreserveReferences();
    }
}