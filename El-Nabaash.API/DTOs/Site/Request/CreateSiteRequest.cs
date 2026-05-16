using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace El_Nabaash.API.DTOs.Site.Request;

public class CreateSiteRequest
{
    [Required , MaxLength(200)]
    public string? Name { get; set; } = string.Empty;
    [Required , MaxLength(100)]
    public string? Location { get; set; } = string.Empty;
    [MaxLength(100)]
    public string? Coordinates { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
    public string? PublicNarrative { get; set; }
    
    [MaxLength(2000)]
    [JsonPropertyName("AeonNarrative")]
    public string? ElNabaashNarrative { get; set; }

}