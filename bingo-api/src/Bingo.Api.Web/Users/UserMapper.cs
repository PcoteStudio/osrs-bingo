using Bingo.Api.Data.Entities;

namespace Bingo.Api.Web.Users;

public static class UserMapper
{
    public static UserPublicResponse ToPublicResponse(this UserEntity entity)
    {
        return new UserPublicResponse
        {
            Username = entity.Username
        };
    }

    public static List<UserPublicResponse> ToPublicResponseList(this IEnumerable<UserEntity> entities)
    {
        return entities.Select(e => e.ToPublicResponse()).ToList();
    }

    public static UserResponse ToResponse(this UserEntity entity)
    {
        return new UserResponse
        {
            Username = entity.Username,
            Email = entity.Email
        };
    }

    public static List<UserResponse> ToResponseList(this IEnumerable<UserEntity> entities)
    {
        return entities.Select(e => e.ToResponse()).ToList();
    }
}