using Bingo.Api.Data.Entities;
using Bingo.Api.Web.Drops;

namespace Bingo.Api.Web.Npcs;

public static class NpcMapper
{
    public static NpcShortResponse ToShortResponse(this NpcEntity entity)
    {
        return new NpcShortResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Image = entity.Image ?? string.Empty,
            KillsPerHour = entity.KillsPerHour
        };
    }

    public static List<NpcShortResponse> ToShortResponseList(this IEnumerable<NpcEntity> entities)
    {
        return entities.Select(e => e.ToShortResponse()).ToList();
    }

    public static NpcResponse ToResponse(this NpcEntity entity)
    {
        return new NpcResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Image = entity.Image ?? string.Empty,
            KillsPerHour = entity.KillsPerHour,
            Drops = entity.Drops.Select(d => d.ToResponse()).ToList()
        };
    }

    public static List<NpcResponse> ToResponseList(this IEnumerable<NpcEntity> entities)
    {
        return entities.Select(e => e.ToResponse()).ToList();
    }
}