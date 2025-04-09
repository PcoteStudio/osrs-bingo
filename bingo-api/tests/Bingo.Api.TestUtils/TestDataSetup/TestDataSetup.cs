using Bingo.Api.Data;
using Bingo.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Bingo.Api.TestUtils.TestDataSetup;

public partial class TestDataSetup(ApplicationDbContext dbContext, UserManager<UserEntity>? userManager = null, RoleManager<IdentityRole>? roleManager = null)
{
}