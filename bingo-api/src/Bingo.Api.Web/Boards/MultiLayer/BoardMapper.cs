using Bingo.Api.Data.Entities;

namespace Bingo.Api.Web.Boards.MultiLayer;

public static class BoardMapper
{
    public static MultiLayerBoardShortResponse ToShortResponse(this MultiLayerBoardEntity entity)
    {
        return new MultiLayerBoardShortResponse
        {
            Id = entity.Id,
            Width = entity.Width,
            Height = entity.Height,
            Depth = entity.Depth
        };
    }

    public static List<MultiLayerBoardShortResponse> ToShortResponseList(
        this IEnumerable<MultiLayerBoardEntity> entities)
    {
        return entities.Select(e => e.ToShortResponse()).ToList();
    }

    public static MultiLayerBoardResponse ToResponse(this MultiLayerBoardEntity entity)
    {
        return new MultiLayerBoardResponse
        {
            Id = entity.Id,
            Width = entity.Width,
            Height = entity.Height,
            Depth = entity.Depth,
            Layers = entity.Layers.Select(l => l.ToResponse()).ToList()
        };
    }

    public static List<MultiLayerBoardResponse> ToResponseList(this IEnumerable<MultiLayerBoardEntity> entities)
    {
        return entities.Select(e => e.ToResponse()).ToList();
    }
}