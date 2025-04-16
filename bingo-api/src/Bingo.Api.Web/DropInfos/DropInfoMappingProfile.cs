using AutoMapper;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Web.DropInfos;

public class DropInfoMappingProfile : Profile
{
    public DropInfoMappingProfile()
    {
        CreateMap<DropInfoEntity, DropInfoResponse>();
        CreateMap<DropInfoEntity, DropInfoShortResponse>();
    }
}