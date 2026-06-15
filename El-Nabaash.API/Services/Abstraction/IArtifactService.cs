using El_Nabaash.API.DTOs.Artifacts.Request;
using El_Nabaash.API.DTOs.Artifacts.Response;
using PublicArtifactResponseDto = El_Nabaash.API.DTOs.Artifacts.Response.PublicArtifactResponseDto;

namespace El_Nabaash.API.Services.Abstraction;

public interface IArtifactService
{
    // public
    Task<List<PublicArtifactResponseDto>> GetPublicArtifactAsync(CancellationToken ct);
    Task<List<PublicArtifactResponseDto>> GetPublicArtifactsBySiteAsync(CancellationToken ct);
    
    // private
    Task<List<PrivateArtifactResponseDto>> GetPrivateArtifactAsync(CancellationToken ct);
    Task<List<PrivateArtifactResponseDto>> GetPrivateArtifactsBySiteAsync(CancellationToken ct);
    Task<PrivateArtifactResponseDto?> CreateArtifactAsync(CreateArtifactRequestDto createArtifactRequest, CancellationToken ct);
    Task<PublicArtifactResponseDto?> GetPublicArtifactByIdAsync(int id, CancellationToken ct);

}