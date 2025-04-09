using AutoMapper;
using Bingo.Api.Data.Entities;
using Bingo.Api.Web.Users;

namespace Bingo.Api.Web.Authentication;

public class UserResponseMappingProfile : Profile
{
    public UserResponseMappingProfile()
    {
        CreateMap<UserEntity, UserResponse>();
    }
}