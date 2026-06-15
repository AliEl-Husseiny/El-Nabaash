namespace El_Nabaash.API.DTOs.Artifacts.Request;

public class PublicArtifactResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CatalogNumber { get; set; } = string.Empty;
    public string? PublicNarrative { get; set; }
    public DateTime DateDiscovered { get; set; }
    public string Type { get; set; } = string.Empty;
    public string SiteName { get; set; } = string.Empty;
    public string? PrimaryImageUrl { get; set; }
}
