namespace El_Nabaash.API.DTOs.CatalogRecord.Response;

public class CatalogRecordResponseDto
{
    public int Id { get; set; }
    public int ArtifactId { get; set; }

    public string Status { get; set; } = string.Empty;
    public DateTime DateSubmitted { get; set; }

    public string SubmittedBy { get; set; } = string.Empty;
    public string? VerifiedBy { get; set; }

    public List<CatalogNoteResponseDto> Notes { get; set; } = new();
}