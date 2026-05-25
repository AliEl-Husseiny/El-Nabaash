using El_Nabaash.API.DTOs.Artifacts.Response;

namespace El_Nabaash.API.Services.Abstraction;

public interface IArtifactService
{
    Task<List<PublicArtifactResponseDto>> GetPublicArtifactAsync(CancellationToken ct);
}