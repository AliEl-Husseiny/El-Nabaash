namespace El_Nabaash.API.DTOs.Catalog.Request;

public class UpdateCatalogRecordRequestDto
{
    public string Status { get; set; } = string.Empty;
    public string? VerifiedById { get; set; }
}