using AutoMapper;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Web.Users;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserEntity, UserResponse>();
        CreateMap<UserEntity, UserPublicResponse>();
    }
}