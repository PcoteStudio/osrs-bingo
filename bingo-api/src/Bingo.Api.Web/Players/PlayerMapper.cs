using Bingo.Api.Data.Entities;
using Bingo.Api.Web.Teams;

namespace Bingo.Api.Web.Players;

public static class TeamMapper
{
    public static PlayerShortResponse ToShortResponse(this PlayerEntity entity)
    {
        return new PlayerShortResponse
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }

    public static List<PlayerShortResponse> ToShortResponseList(this IEnumerable<PlayerEntity> entities)
    {
        return entities.Select(e => e.ToShortResponse()).ToList();
    }

    public static PlayerResponse ToResponse(this PlayerEntity entity)
    {
        return new PlayerResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Teams = entity.Teams.Select(t => t.ToShortResponse()).ToList()
        };
    }

    public static List<PlayerResponse> ToResponseList(this IEnumerable<PlayerEntity> entities)
    {
        return entities.Select(e => e.ToResponse()).ToList();
    }
}