using AutoMapper;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Web.Players;

public class PlayersMappingProfile : Profile
{
    public PlayersMappingProfile()
    {
        CreateMap<PlayerEntity, PlayerResponse>();
    }
}