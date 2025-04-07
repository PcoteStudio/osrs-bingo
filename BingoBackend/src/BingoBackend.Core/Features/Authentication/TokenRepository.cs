using BingoBackend.Core.Features.Generic;
using BingoBackend.Data;
using BingoBackend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BingoBackend.Core.Features.Authentication;

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