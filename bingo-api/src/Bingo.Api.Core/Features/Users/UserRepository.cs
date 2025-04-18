using Bingo.Api.Core.Features.Generic;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.Users;

public interface IUserRepository : IGenericRepository<UserEntity>
{
    Task<UserEntity?> GetByEmailAsync(string email);
    Task<UserEntity?> GetByUsernameAsync(string username);
    Task<UserEntity?> GetCompleteByIdAsync(int id);
    Task<UserEntity?> GetCompleteByUsernameAsync(string username);
}

public class UserRepository(ApplicationDbContext dbContext, IUserUtil userUtil)
    : GenericRepository<UserEntity>(dbContext), IUserRepository
{
    public Task<UserEntity?> GetByEmailAsync(string email)
    {
        return DbContext.Users
            .FirstOrDefaultAsync(p => p.EmailNormalized == userUtil.NormalizeEmail(email));
    }

    public Task<UserEntity?> GetByUsernameAsync(string username)
    {
        return DbContext.Users
            .FirstOrDefaultAsync(p => p.UsernameNormalized == userUtil.NormalizeUsername(username));
    }

    public Task<UserEntity?> GetCompleteByIdAsync(int id)
    {
        return DbContext.Users
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task<UserEntity?> GetCompleteByUsernameAsync(string username)
    {
        return DbContext.Users
            .FirstOrDefaultAsync(p => p.UsernameNormalized == userUtil.NormalizeUsername(username));
    }
}