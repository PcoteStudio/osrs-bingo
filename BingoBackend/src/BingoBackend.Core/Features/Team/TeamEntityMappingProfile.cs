using AutoMapper;
using BingoBackend.Data.Team;

namespace BingoBackend.Core.Features.Team;

public class TeamEntityMappingProfile : Profile
{
    public TeamEntityMappingProfile()
    {
        CreateMap<TeamEntity, Team>().ReverseMap();
    }
}