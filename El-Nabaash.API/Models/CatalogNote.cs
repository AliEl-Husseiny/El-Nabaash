using Microsoft.Build.Framework;

namespace El_Nabaash.API.Models;

public class CatalogNote
{
    public int Id { get; set; }

    [Required]
    public int CatalogRecordId { get; set; }
    
    public CatalogRecord? CatalogRecord { get; set; } // Navigation property to CatalogRecord

    public string AuthorId { get; set; } = string.Empty; // FK to ApplicationUser
    public ApplicationUser Author { get; set; } = null!; // Navigation property to ApplicationUser
}