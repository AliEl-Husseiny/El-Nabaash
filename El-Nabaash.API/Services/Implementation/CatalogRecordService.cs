using El_Nabaash.API.DTOs.CatalogRecord.Response;

namespace El_Nabaash.API.Services.Implementation;

public class CatalogRecordService(AppDbContext dbContext) : ICatalogRecordService
{
    public async Task<List<CatalogRecordResponseDto>?> GetCatalogRecordsByArtifactAsync(int artifactId, CancellationToken ct)
    {
        // Step A – Confirm the artifact exists
        var exists = await dbContext.Artifacts
            .AsNoTracking()
            .AnyAsync(a => a.Id == artifactId, ct);

        if (!exists)
            return null;

        // Step B – Load and project CatalogRecords
        var records = await dbContext.CatalogRecords
            .AsNoTracking()
            .Where(r => r.ArtifactId == artifactId)
            .Include(r => r.SubmittedBy)
            .Include(r => r.VerifiedBy)
            .Include(r => r.Notes)
            .ThenInclude(n => n.Author)
            .Select(r => new CatalogRecordResponseDto()
            {
                Id = r.Id,
                ArtifactId = r.ArtifactId,
                Status = r.Status,
                DateSubmitted = r.DateSubmitted,
                SubmittedBy = $"{r.SubmittedBy.FirstName} {r.SubmittedBy.LastName}",
                VerifiedBy = r.VerifiedBy != null
                    ? $"{r.VerifiedBy.FirstName} {r.VerifiedBy.LastName}"
                    : null,
                Notes = r.Notes.Select(n => new CatalogNoteResponseDto()
                {
                    Id = n.Id,
                    Content = n.Content,
                    Created = n.CreatedAt,
                    Author = $"{n.Author.FirstName} {n.Author.LastName}"
                }).ToList()
            })
            .ToListAsync(ct);

        return records;
    }
}