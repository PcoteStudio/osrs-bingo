using BingoBackend.Data.Team;
using Microsoft.EntityFrameworkCore;

namespace BingoBackend.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<TeamEntity> Teams { get; set; }
}