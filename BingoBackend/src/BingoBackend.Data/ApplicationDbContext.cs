using BingoBackend.Data.Team;
using Microsoft.EntityFrameworkCore;

namespace BingoBackend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<TeamEntity> Teams { get; set; }
}