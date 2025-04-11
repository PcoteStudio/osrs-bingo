using Bingo.Api.Core.Features.Generic;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.Authentication;

public interface ITokenRepository : IGenericRepository<TokenEntity>
{
    Task<TokenEntity?> GetByUsernameAsync(string name);
}

public class TokenRepository(ApplicationDbContext dbContext)
    : GenericRepository<TokenEntity>(dbContext), ITokenRepository
{
    public Task<TokenEntity?> GetByUsernameAsync(string username)
    {
        return DbContext.Tokens.FirstOrDefaultAsync(t => t.Username == username);
    }
}