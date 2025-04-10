using AutoMapper;
using Bingo.Api.Data.Entities.Events;

namespace Bingo.Api.Web.Events;

public class EventsMappingProfile : Profile
{
    public EventsMappingProfile()
    {
        CreateMap<EventEntity, EventResponse>();
    }
}