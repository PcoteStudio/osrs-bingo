using Bingo.Api.Data.Entities;
using Bingo.Api.Web.Tiles;

namespace Bingo.Api.Web.Boards.MultiLayer;

public static class BoardLayerMapper
{
    public static BoardLayerShortResponse ToShortResponse(this BoardLayerEntity entity)
    {
        return new BoardLayerShortResponse
        {
            Id = entity.Id,
            Width = entity.Width,
            Height = entity.Height
        };
    }

    public static List<BoardLayerShortResponse> ToShortResponseList(this IEnumerable<BoardLayerEntity> entities)
    {
        return entities.Select(e => e.ToShortResponse()).ToList();
    }

    public static BoardLayerResponse ToResponse(this BoardLayerEntity entity)
    {
        return new BoardLayerResponse
        {
            Id = entity.Id,
            Width = entity.Width,
            Height = entity.Height,
            Tiles = entity.Tiles.Select(t => t.ToResponse()).ToList()
        };
    }

    public static List<BoardLayerResponse> ToResponseList(this IEnumerable<BoardLayerEntity> entities)
    {
        return entities.Select(e => e.ToResponse()).ToList();
    }
}