using AutoMapper;
using BingoBackend.Data.Entities;

namespace BingoBackend.Web.Teams;

public class TeamResponseMappingProfile : Profile
{
    public TeamResponseMappingProfile()
    {
        CreateMap<TeamEntity, TeamResponse>();
    }
}