namespace El_Nabaash.API.Models;

public class ArtifcatMediaFile
{
    public int Id { get; set; }
    public int ArtifactId { get; set; }
    public Artifact? Artifact { get; set; }
    
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = "image/jpeg";
    public byte[] Data { get; set; } = [];
    // option to mark one media file as primary 
    public bool IsPrimary { get; set; } = false;
}