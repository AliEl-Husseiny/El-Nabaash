namespace El_Nabaash.API.DTOs.Catalog.Request;

public class CreateCatalogRecordRequestDto
{
    public int ArtifactId { get; set; }
    public string Status { get; set; } = string.Empty;
}