using AutoMapper;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Web.Users;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserEntity, UserResponse>().MaxDepth(1);
        CreateMap<UserEntity, UserPublicResponse>().MaxDepth(1);
    }
}