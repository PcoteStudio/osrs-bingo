using Bingo.Api.Data.Entities;
using Bingo.Api.Data.Entities.Events;
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

    // Auth
    public DbSet<TokenEntity> Tokens { get; set; }

    // Events
    public DbSet<EventEntity> Events { get; set; }
    public DbSet<TeamEntity> Teams { get; set; }
    public DbSet<PlayerEntity> Players { get; set; }

    // Boards
    public DbSet<MultiLayerBoardEntity> MultiLayerBoards { get; set; }
    public DbSet<TileEntity> Tiles { get; set; }
    public DbSet<GrindProgressionEntity> GrindProgressions { get; set; }
    public DbSet<GrindEntity> Grinds { get; set; }
    public DbSet<ProgressionEntity> Progressions { get; set; }

    // Data
    public DbSet<ItemEntity> Items { get; set; }
    public DbSet<NpcEntity> Npcs { get; set; }
}