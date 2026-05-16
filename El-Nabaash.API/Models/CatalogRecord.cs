using El_Nabaash.API.Enums;
using Microsoft.Build.Framework;

namespace El_Nabaash.API.Models;

public class CatalogRecord
{
    public int Id { get; set; }
    [Required] public int ArtifactId { get; set; }
    public Artifact? Artifact { get; set; }
    [Required] public string SubmittedById { get; set; } = string.Empty; // fk to Application User
    public ApplicationUser SubmittedBy { get; set; } = null!;
    public string? VerifiedById { get; set; } //fk to ApplicationUser
    public ApplicationUser? VerifiedBy { get; set; } // navigation property to ApplicationUser
    [Required]
    public string Status { get; set; } = CatalogStatus.Draft.ToString(); // default to Draft
    [Required]
    public DateTime DateSubmitted { get; set; } = DateTime.UtcNow;
    public ICollection<CatalogNote> Notes { get; set; } = new List<CatalogNote>();

}