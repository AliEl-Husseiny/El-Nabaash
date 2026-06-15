using El_Nabaash.API.DTOs.Artifacts.Response;

namespace El_Nabaash.API.Services.Abstraction;

public interface IArtifactService
{
    // public
    Task<List<PublicArtifactResponseDto>> GetPublicArtifactAsync(CancellationToken ct);
    Task<List<PublicArtifactResponseDto>> GetPublicArtifactsBySiteAsync(CancellationToken ct);
    
    // private
    Task<List<PrivateArtifactResponseDto>> GetPrivateArtifactAsync(CancellationToken ct);
    Task<List<PrivateArtifactResponseDto>> GetPrivateArtifactsBySiteAsync(CancellationToken ct);
    
}