namespace El_Nabaash.API.DTOs.Artifacts.Response;

public class PrivateArtifactResponseDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? CatalogNumber { get; set; }
    public string? Description { get; set; }
    public string? PublicNarrative { get; set; }
    public DateTime DateDiscovered { get; set; }
    public string? Type { get; set; }
    public int SiteId { get; set; }
    public string? SiteName { get; set; }
    public string? PrimaryImageUrl { get; set; }
}