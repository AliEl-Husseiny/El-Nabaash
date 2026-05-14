using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace El_Nabaash.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    // Optional: additional DbSets for your other entities
    // public DbSet<SomeEntity> SomeEntities { get; set; }
    
    public DbSet<Artifact> Artifacts { get; set; }
    public DbSet<Site> Sites { get; set; }
    public DbSet<ArtifactMediaFile> ArtifactMediaFiles { get; set; }
    public DbSet<CatalogRecord> CatalogRecords { get; set; }
    public DbSet<CatalogNote> CatalogNotes { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<CatalogRecord>()
            .HasOne(cr => cr.SubmittedBy)
            .WithMany(u => u.SubmittedCatalogRecords)
            .HasForeignKey(cr => cr.SubmittedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<CatalogRecord>()
            .HasOne(cr => cr.VerifiedBy)
            .WithMany(u => u.VerifiedCatalogRecords)
            .HasForeignKey(cr => cr.VerifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<Artifact>()
            .Property(a =>a.Type)
            .HasConversion<string>();  // converts the enum <-> string automatically  
    }
}
