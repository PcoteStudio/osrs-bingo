using Microsoft.AspNetCore.Http;

namespace Bingo.Api.Core.Features.Items.Arguments;

public class ItemCreateArguments
{
    public string Name { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
}