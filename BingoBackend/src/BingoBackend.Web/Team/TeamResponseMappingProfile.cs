using AutoMapper;

namespace BingoBackend.Web.Team;

public class TeamResponseMappingProfile : Profile
{
    public TeamResponseMappingProfile()
    {
        CreateMap<Core.Features.Team.Team, TeamResponse>();
    }
}