using Bingo.Api.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Data;

public class ApplicationDbContext : IdentityDbContext<UserEntity>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<TokenEntity> Tokens { get; set; }
    public DbSet<TeamEntity> Teams { get; set; }
    public DbSet<PlayerEntity> Players { get; set; }
}