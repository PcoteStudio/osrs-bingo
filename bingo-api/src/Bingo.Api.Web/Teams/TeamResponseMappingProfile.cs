using AutoMapper;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Web.Teams;

public class TeamResponseMappingProfile : Profile
{
    public TeamResponseMappingProfile()
    {
        CreateMap<TeamEntity, TeamResponse>();
    }
}