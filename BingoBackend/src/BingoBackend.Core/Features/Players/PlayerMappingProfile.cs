using AutoMapper;
using BingoBackend.Data.Entities;

namespace BingoBackend.Core.Features.Players;

public class PlayerMappingProfile : Profile
{
    public PlayerMappingProfile()
    {
        CreateMap<PlayerEntity, Player>().ReverseMap();
    }
}