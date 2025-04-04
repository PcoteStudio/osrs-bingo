using AutoMapper;
using BingoBackend.Data.Entities;

namespace BingoBackend.Web.Players;

public class PlayerResponseMappingProfile : Profile
{
    public PlayerResponseMappingProfile()
    {
        CreateMap<PlayerEntity, PlayerResponse>();
    }
}