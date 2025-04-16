using AutoMapper;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Web.Drops;

public class DropMappingProfile : Profile
{
    public DropMappingProfile()
    {
        CreateMap<DropEntity, DropResponse>();
        CreateMap<DropEntity, DropShortResponse>();
    }
}