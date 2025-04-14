using AutoMapper;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Web.Events;

public class EventMappingProfile : Profile
{
    public EventMappingProfile()
    {
        CreateMap<EventEntity, EventResponse>();
    }
}