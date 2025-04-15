using Bingo.Api.Core.Features.Generic;
using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Core.Features.Users;

public interface IUserRepository : IGenericRepository<UserEntity>
{
    Task<UserEntity?> GetByEmailAsync(string email);
    Task<UserEntity?> GetByNameAsync(string username);
    Task<UserEntity?> GetCompleteByIdAsync(int id);
    Task<UserEntity?> GetCompleteByNameAsync(string username);
}

public class UserRepository(ApplicationDbContext dbContext)
    : GenericRepository<UserEntity>(dbContext), IUserRepository
{
    public Task<UserEntity?> GetByEmailAsync(string email)
    {
        return DbContext.Users
            .FirstOrDefaultAsync(p => p.Email == email);
    }

    public Task<UserEntity?> GetByNameAsync(string username)
    {
        return DbContext.Users
            .FirstOrDefaultAsync(p => p.Username == username);
    }

    public Task<UserEntity?> GetCompleteByIdAsync(int id)
    {
        return DbContext.Users
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task<UserEntity?> GetCompleteByNameAsync(string username)
    {
        return DbContext.Users
            .FirstOrDefaultAsync(p => p.Username == username);
    }
}