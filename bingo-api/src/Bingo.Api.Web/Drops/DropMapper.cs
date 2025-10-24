using Bingo.Api.Data.Entities;
using Bingo.Api.Web.Items;
using Bingo.Api.Web.Npcs;

namespace Bingo.Api.Web.Drops;

public static class DropMapper
{
    public static DropShortResponse ToShortResponse(this DropEntity entity)
    {
        return new DropShortResponse
        {
            Id = entity.Id,
            NpcId = entity.NpcId,
            ItemId = entity.ItemId,
            DropRate = entity.DropRate,
            Ehc = entity.Ehc
        };
    }

    public static List<DropShortResponse> ToShortResponseList(this IEnumerable<DropEntity> entities)
    {
        return entities.Select(e => e.ToShortResponse()).ToList();
    }

    public static DropResponse ToResponse(this DropEntity entity)
    {
        return new DropResponse
        {
            Id = entity.Id,
            NpcId = entity.NpcId,
            ItemId = entity.ItemId,
            DropRate = entity.DropRate,
            Ehc = entity.Ehc,
            Npc = entity.Npc.ToShortResponse(),
            Item = entity.Item.ToShortResponse()
        };
    }

    public static List<DropResponse> ToResponseList(this IEnumerable<DropEntity> entities)
    {
        return entities.Select(e => e.ToResponse()).ToList();
    }
}