using AutoMapper;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Web.Tiles;

public class TileMappingProfile : Profile
{
    public TileMappingProfile()
    {
        CreateMap<TileEntity, TileResponse>();
        CreateMap<TileEntity, TileShortResponse>();
    }
}