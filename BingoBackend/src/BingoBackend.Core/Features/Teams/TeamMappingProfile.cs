using AutoMapper;
using BingoBackend.Data.Entities;

namespace BingoBackend.Core.Features.Teams;

public class TeamMappingProfile : Profile
{
    public TeamMappingProfile()
    {
        CreateMap<TeamEntity, Team>().ReverseMap();
    }
}