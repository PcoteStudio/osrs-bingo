using AutoMapper;
using Bingo.Api.Core.Features.Authentication.Arguments;
using Bingo.Api.Data.Entities;
using Bingo.Api.Web.Users;

namespace Bingo.Api.Web.Authentication;

public class AuthenticationMappingProfile : Profile
{
    public AuthenticationMappingProfile()
    {
        CreateMap<AuthRefreshArguments, TokenResponse>();
        CreateMap<UserEntity, UserResponse>();
    }
}