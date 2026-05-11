using Microsoft.EntityFrameworkCore;

namespace El_Nabaash.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    //the models that our database will use and generate tables for in database
    
}