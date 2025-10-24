using Bingo.Api.Data.Entities;
using Bingo.Api.Web.Drops;

namespace Bingo.Api.Web.Items;

public static class ItemMapper
{
    public static ItemShortResponse ToShortResponse(this ItemEntity entity)
    {
        return new ItemShortResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Image = entity.Image ?? string.Empty
        };
    }

    public static List<ItemShortResponse> ToShortResponseList(this IEnumerable<ItemEntity> entities)
    {
        return entities.Select(e => e.ToShortResponse()).ToList();
    }

    public static ItemResponse ToResponse(this ItemEntity entity)
    {
        return new ItemResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Image = entity.Image ?? string.Empty,
            Drops = entity.Drops.Select(d => d.ToResponse()).ToList()
        };
    }

    public static List<ItemResponse> ToResponseList(this IEnumerable<ItemEntity> entities)
    {
        return entities.Select(e => e.ToResponse()).ToList();
    }
}