using Bingo.Api.Data.Entities;
using Bingo.Api.Web.Teams;
using Bingo.Api.Web.Users;

namespace Bingo.Api.Web.Events;

public static class EventMapper
{
    public static EventShortResponse ToShortResponse(this EventEntity entity)
    {
        return new EventShortResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            StartTime = entity.StartTime,
            EndTime = entity.EndTime
        };
    }

    public static List<EventShortResponse> ToShortResponseList(this IEnumerable<EventEntity> entities)
    {
        return entities.Select(e => e.ToShortResponse()).ToList();
    }

    public static EventResponse ToResponse(this EventEntity entity)
    {
        return new EventResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            StartTime = entity.StartTime,
            EndTime = entity.EndTime,
            Teams = entity.Teams.Select(t => t.ToResponse()).ToList(),
            Administrators = entity.Administrators.Select(a => a.ToPublicResponse()).ToList()
        };
    }

    public static List<EventResponse> ToResponseList(this IEnumerable<EventEntity> entities)
    {
        return entities.Select(e => e.ToResponse()).ToList();
    }
}