using AutoMapper;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Web.Players;

public class PlayerMappingProfile : Profile
{
    public PlayerMappingProfile()
    {
        CreateMap<PlayerEntity, PlayerResponse>();
    }
}