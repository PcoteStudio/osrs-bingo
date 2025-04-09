using Bingo.Api.Core.Features.Generic;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.Authentication;

public interface ITokenRepository : IGenericRepository<TokenEntity>
{
    Task<TokenEntity?> GetByUsernameAsync(string name);
}

public class TokenRepository(ApplicationDbContext context)
    : GenericRepository<TokenEntity>(context), ITokenRepository
{
    public Task<TokenEntity?> GetByUsernameAsync(string username)
    {
        return Context.Tokens
            .FirstOrDefaultAsync(t => t.Username == username);
    }
}