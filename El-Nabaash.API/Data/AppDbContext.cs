using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace El_Nabaash.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    // Optional: additional DbSets for your other entities
    // public DbSet<SomeEntity> SomeEntities { get; set; }
}