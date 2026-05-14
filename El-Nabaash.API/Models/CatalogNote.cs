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

    [Required]
    public string Content { get; set; } = string.Empty; // The content of the note
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp for when the note was created

    public ICollection<CatalogNote> Notes { get; set; } = [];

}