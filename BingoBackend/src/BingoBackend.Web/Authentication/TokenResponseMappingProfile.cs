using AutoMapper;
using BingoBackend.Core.Features.Authentication;

namespace BingoBackend.Web.Authentication;

public class TokenResponseMappingProfile : Profile
{
    public TokenResponseMappingProfile()
    {
        CreateMap<TokenModel, TokenResponse>();
    }
}