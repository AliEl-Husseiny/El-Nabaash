using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using El_Nabaash.API.Models;

namespace El_Nabaash.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    // Optional: additional DbSets for your other entities
    // public DbSet<SomeEntity> SomeEntities { get; set; }
}