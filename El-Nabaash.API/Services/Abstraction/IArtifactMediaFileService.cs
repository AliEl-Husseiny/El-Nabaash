namespace El_Nabaash.API.Services.Abstraction;

public interface IArtifactMediaFileService
{
    Task<ArtifactMediaFile?> CreateArtifactMediaFileAsync(
        int artifactId,
        IFormFile file,
        bool isPrimary,
        CancellationToken ct
    );
}