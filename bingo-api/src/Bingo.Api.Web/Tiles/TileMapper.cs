using Bingo.Api.Data.Entities;

namespace Bingo.Api.Web.Tiles;

public static class TileMapper
{
    public static TileShortResponse ToShortResponse(this TileEntity entity)
    {
        return new TileShortResponse
        {
            Id = entity.Id,
            EventId = entity.EventId,
            BoardId = entity.BoardId,
            Name = entity.Name,
            Description = entity.Description,
            GrindCountForCompletion = entity.GrindCountForCompletion,
            IsCompleted = entity.IsCompleted,
            IsActive = entity.IsActive
        };
    }

    public static List<TileShortResponse> ToShortResponseList(this IEnumerable<TileEntity> entities)
    {
        return entities.Select(e => e.ToShortResponse()).ToList();
    }

    public static TileResponse ToResponse(this TileEntity entity)
    {
        return new TileResponse
        {
            Id = entity.Id,
            EventId = entity.EventId,
            BoardId = entity.BoardId,
            Name = entity.Name,
            Description = entity.Description,
            GrindCountForCompletion = entity.GrindCountForCompletion,
            IsCompleted = entity.IsCompleted,
            IsActive = entity.IsActive
        };
    }

    public static List<TileResponse> ToResponseList(this IEnumerable<TileEntity> entities)
    {
        return entities.Select(e => e.ToResponse()).ToList();
    }
}