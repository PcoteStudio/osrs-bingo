using AutoMapper;
using Bingo.Api.Data.Entities.Events;

namespace Bingo.Api.Web.Players;

public class PlayerMappingProfile : Profile
{
    public PlayerMappingProfile()
    {
        CreateMap<PlayerEntity, PlayerResponse>();
    }
}