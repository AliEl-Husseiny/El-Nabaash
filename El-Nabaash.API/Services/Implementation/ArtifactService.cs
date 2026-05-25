using El_Nabaash.API.DTOs.Artifacts.Response;

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
}