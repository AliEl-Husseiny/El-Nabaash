namespace El_Nabaash.API.DTOs.CatalogRecord.Response;

public class CatalogNoteResponseDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public string Author { get; set; } = string.Empty;
}