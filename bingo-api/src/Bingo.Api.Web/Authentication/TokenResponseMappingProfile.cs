using AutoMapper;
using Bingo.Api.Core.Features.Authentication.Arguments;

namespace Bingo.Api.Web.Authentication;

public class TokenResponseMappingProfile : Profile
{
    public TokenResponseMappingProfile()
    {
        CreateMap<AuthRefreshArguments, TokenResponse>();
    }
}