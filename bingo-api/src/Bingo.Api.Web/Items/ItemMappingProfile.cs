using AutoMapper;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Web.Items;

public class ItemMappingProfile : Profile
{
    public ItemMappingProfile()
    {
        CreateMap<ItemEntity, ItemResponse>();
        CreateMap<ItemEntity, ItemShortResponse>();
    }
}