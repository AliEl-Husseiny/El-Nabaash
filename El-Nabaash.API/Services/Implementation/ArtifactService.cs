using El_Nabaash.API.DTOs.Artifacts.Request;
using El_Nabaash.API.DTOs.Artifacts.Response;
using El_Nabaash.API.Enums;

namespace El_Nabaash.API.Services.Implementation;

public class ArtifactService(AppDbContext dbContext) : IArtifactService
{
    public async Task<List<PublicArtifactResponseDto>> GetPublicArtifactAsync(CancellationToken ct)
    {
        return await dbContext.Artifacts
            .AsNoTracking()
            .Select(a => new PublicArtifactResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                CatalogNumber = a.CatalogNumber,
                PublicNarrative = a.PublicNarrative,
                DateDiscovered = a.DateDiscovered,
                Type = a.Type!.ToString(),
                SiteId = a.SiteId,
                SiteName = a.Site!.Name,
                PrimaryImageUrl = a.MediaFiles
                    .Where(ai => ai.IsPrimary)
                    .Select(ai => $"/api/public/artifacts/images/{ai.Id}")
                    .FirstOrDefault()
            }).ToListAsync(ct);
    }

    public async Task<List<PublicArtifactResponseDto>> GetPublicArtifactsBySiteAsync(CancellationToken ct)
    {
        return await dbContext.Artifacts
            .AsNoTracking()
            .Select(a => new PublicArtifactResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                CatalogNumber = a.CatalogNumber,
                PublicNarrative = a.PublicNarrative,
                DateDiscovered = a.DateDiscovered,
                Type = a.Type!.ToString(),
                SiteId = a.SiteId,
                SiteName = a.Site!.Name,
                PrimaryImageUrl = a.MediaFiles
                    .Where(ai => ai.IsPrimary)
                    .Select(ai => $"/api/public/artifacts/images/{ai.Id}")
                    .FirstOrDefault()
            }).ToListAsync(ct);
    }

    public async Task<List<PrivateArtifactResponseDto>> GetPrivateArtifactAsync(CancellationToken ct)
    {
        return await dbContext.Artifacts
            .AsNoTracking()
            .Select(a => new PrivateArtifactResponseDto()
            {
                Id = a.Id,
                Name = a.Name,
                CatalogNumber = a.CatalogNumber,
                PublicNarrative = a.PublicNarrative,
                DateDiscovered = a.DateDiscovered,
                Description = a.Description,
                Type = a.Type!.ToString(),
                SiteId = a.SiteId,
                SiteName = a.Site!.Name,
                PrimaryImageUrl = a.MediaFiles
                    .Where(ai => ai.IsPrimary)
                    .Select(ai => $"/api/public/artifacts/images/{ai.Id}")
                    .FirstOrDefault()
            }).ToListAsync(ct);
    }

    public async Task<List<PrivateArtifactResponseDto>> GetPrivateArtifactsBySiteAsync(CancellationToken ct)
    {
        return await dbContext.Artifacts
            .AsNoTracking()
            .Select(a => new PrivateArtifactResponseDto()
            {
                Id = a.Id,
                Name = a.Name,
                CatalogNumber = a.CatalogNumber,
                PublicNarrative = a.PublicNarrative,
                DateDiscovered = a.DateDiscovered,
                Description = a.Description,
                Type = a.Type!.ToString(),
                SiteId = a.SiteId,
                SiteName = a.Site!.Name,
                PrimaryImageUrl = a.MediaFiles
                    .Where(ai => ai.IsPrimary)
                    .Select(ai => $"/api/public/artifacts/images/{ai.Id}")
                    .FirstOrDefault()
            }).ToListAsync(ct);
    }

    public async Task<PrivateArtifactResponseDto?> CreateArtifactAsync(
        CreateArtifactRequestDto createArtifactRequestDto, CancellationToken ct)
    {
        // Validate the site exists
        var siteExists = await dbContext.Sites.AnyAsync(s => s.Id == createArtifactRequestDto.SiteId, ct);
        if (!siteExists) return null;

        // validate the artifact type string
        if (!Enum.TryParse<ArtifactType>(createArtifactRequestDto.Type, true, out var artifactType))
        {
            throw new ArgumentException($"Invalid artifact type '{createArtifactRequestDto.Type}'. " +
                                        $"Allowed values are: {string.Join(", ", Enum.GetNames(typeof(ArtifactType)))}");
        }
        
        // create the new artifact 
        var artifact = new Artifact()
        {
            Name = createArtifactRequestDto.Name,
            CatalogNumber = createArtifactRequestDto.CatalogNumber,
            Description = createArtifactRequestDto.Description,
            PublicNarrative = createArtifactRequestDto.PublicNarrative,
            DateDiscovered = createArtifactRequestDto.DateDiscovered,
            Type = artifactType.ToString(),
            SiteId = createArtifactRequestDto.SiteId
        };
        
        dbContext.Artifacts.Add(artifact);
        await dbContext.SaveChangesAsync(ct);
        
        // Return DTO
        return new PrivateArtifactResponseDto()
        {
            Id = artifact.Id,
            Name = artifact.Name,
            CatalogNumber = artifact.CatalogNumber,
            Description = artifact.Description,
            PublicNarrative = artifact.PublicNarrative,
            DateDiscovered = artifact.DateDiscovered,
            Type = artifact.Type.ToString(),
            SiteName = (await db.Sites.FindAsync([artifact.SiteId], ct))?.Name ?? string.Empty
        };
    }
}