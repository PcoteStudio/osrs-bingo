using Bingo.Api.Data;

namespace Bingo.Api.Core.Features.Seeder;

public interface ISeederService
{
    Task SeedEventsAsync();
}

public class SeederService(
    ApplicationDbContext dbContext
) : ISeederService
{
    public async Task SeedEventsAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}