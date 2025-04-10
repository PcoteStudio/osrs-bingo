using AutoMapper;
using Bingo.Api.Data.Entities.Events;

namespace Bingo.Api.Web.Teams;

public class TeamsMappingProfile : Profile
{
    public TeamsMappingProfile()
    {
        CreateMap<TeamEntity, TeamResponse>();
    }
}