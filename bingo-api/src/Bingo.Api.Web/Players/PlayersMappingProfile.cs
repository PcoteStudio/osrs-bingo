using AutoMapper;
using Bingo.Api.Data.Entities.Events;

namespace Bingo.Api.Web.Players;

public class PlayersMappingProfile : Profile
{
    public PlayersMappingProfile()
    {
        CreateMap<PlayerEntity, PlayerResponse>();
    }
}