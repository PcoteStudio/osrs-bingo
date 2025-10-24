using Bingo.Api.Data.Entities;
using Bingo.Api.Web.Events;
using Bingo.Api.Web.Players;

namespace Bingo.Api.Web.Teams;

public static class TeamMapper
{
    public static TeamShortResponse ToShortResponse(this TeamEntity entity)
    {
        return new TeamShortResponse
        {
            Id = entity.Id,
            EventId = entity.EventId,
            Name = entity.Name
        };
    }

    public static List<TeamShortResponse> ToShortResponseList(this IEnumerable<TeamEntity> entities)
    {
        return entities.Select(e => e.ToShortResponse()).ToList();
    }

    public static TeamResponse ToResponse(this TeamEntity entity)
    {
        return new TeamResponse
        {
            Id = entity.Id,
            EventId = entity.EventId,
            Name = entity.Name,
            Event = entity.Event.ToShortResponse(),
            Players = entity.Players.Select(p => p.ToResponse()).ToList()
        };
    }

    public static List<TeamResponse> ToResponseList(this IEnumerable<TeamEntity> entities)
    {
        return entities.Select(e => e.ToResponse()).ToList();
    }
}