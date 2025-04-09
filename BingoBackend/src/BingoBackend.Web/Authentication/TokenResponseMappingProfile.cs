using AutoMapper;
using BingoBackend.Core.Features.Authentication.Arguments;

namespace BingoBackend.Web.Authentication;

public class TokenResponseMappingProfile : Profile
{
    public TokenResponseMappingProfile()
    {
        CreateMap<AuthRefreshArguments, TokenResponse>();
    }
}