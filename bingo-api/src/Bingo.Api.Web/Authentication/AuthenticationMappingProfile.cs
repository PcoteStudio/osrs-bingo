using AutoMapper;
using Bingo.Api.Core.Features.Authentication;

namespace Bingo.Api.Web.Authentication;

public class AuthenticationMappingProfile : Profile
{
    public AuthenticationMappingProfile()
    {
        CreateMap<TokensModel, TokenResponse>();
    }
}