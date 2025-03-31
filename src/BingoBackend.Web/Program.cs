using Microsoft.AspNetCore;

namespace BingoBackend.Web;

public class Program
{
    internal static void Main(string[] args)
    {
        WebHost
            .CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build()
            .Run();
    }
}