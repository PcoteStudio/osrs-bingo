using AutoMapper;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Web.Npcs;

public class NpcMappingProfile : Profile
{
    public NpcMappingProfile()
    {
        CreateMap<NpcEntity, NpcResponse>();
        CreateMap<NpcEntity, NpcShortResponse>();
    }
}