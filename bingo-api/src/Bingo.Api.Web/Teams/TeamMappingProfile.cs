using AutoMapper;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Web.Teams;

public class TeamMappingProfile : Profile
{
    public TeamMappingProfile()
    {
        CreateMap<TeamEntity, TeamResponse>();
        CreateMap<TeamEntity, TeamShortResponse>();
    }
}