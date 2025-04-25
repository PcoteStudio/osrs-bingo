using AutoMapper;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Web.Boards.MultiLayer;

public class MultiLayerBoardMappingProfile : Profile
{
    public MultiLayerBoardMappingProfile()
    {
        CreateMap<MultiLayerBoardEntity, MultiLayerBoardResponse>();
        CreateMap<MultiLayerBoardEntity, MultiLayerBoardShortResponse>();
    }
}