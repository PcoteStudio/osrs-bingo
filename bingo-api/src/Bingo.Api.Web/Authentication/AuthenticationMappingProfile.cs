using AutoMapper;
using Bingo.Api.Core.Features.Authentication.Arguments;

namespace Bingo.Api.Web.Authentication;

public class AuthenticationMappingProfile : Profile
{
    public AuthenticationMappingProfile()
    {
        CreateMap<AuthRefreshArguments, TokenResponse>();
    }
}