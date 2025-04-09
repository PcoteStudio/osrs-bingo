using AutoMapper;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Web.Players;

public class PlayerResponseMappingProfile : Profile
{
    public PlayerResponseMappingProfile()
    {
        CreateMap<PlayerEntity, PlayerResponse>();
    }
}