using AutoMapper;
using BingoBackend.Web.Teams;

namespace BingoBackend.Web.Teams;

public class TeamResponseMappingProfile : Profile
{
    public TeamResponseMappingProfile()
    {
        CreateMap<Core.Features.Teams.Team, TeamResponse>();
    }
}