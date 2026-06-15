using El_Nabaash.API.DTOs.Catalog.Request;
using El_Nabaash.API.DTOs.CatalogRecord.Response;

namespace El_Nabaash.API.Services.Implementation;

public class CatalogRecordService(AppDbContext dbContext) : ICatalogRecordService
{
    public async Task<List<CatalogRecordResponseDto>?> GetCatalogRecordsByArtifactAsync(int artifactId,
        CancellationToken ct)
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

    public async Task<CatalogRecordResponseDto?> GetCatalogRecordByIdAsync(int id, CancellationToken ct)
    {
        var record = await dbContext.CatalogRecords
            .AsNoTracking()
            .Where(r => r.Id == id)
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
            .FirstOrDefaultAsync(ct);

        return record;
    }

    public async Task<CatalogRecordResponseDto?> CreateCatalogRecordAsync(CreateCatalogRecordRequestDto request,
        string userId, CancellationToken ct)
    {
        // Step A — Validate artifact
        var artifact = await dbContext.Artifacts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.ArtifactId, ct);

        if (artifact is null)
            return null;

        // Step B — Create entity
        var record = new CatalogRecord
        {
            ArtifactId = request.ArtifactId,
            Status = request.Status,
            SubmittedById = userId,
            DateSubmitted = DateTime.UtcNow
        };

        dbContext.CatalogRecords.Add(record);
        await dbContext.SaveChangesAsync(ct);

        // Step C — Re-load with navigation properties
        var created = await dbContext.CatalogRecords
            .AsNoTracking()
            .Include(r => r.SubmittedBy)
            .FirstAsync(r => r.Id == record.Id, ct);

        // Step D — Project into DTO
        return new CatalogRecordResponseDto()
        {
            Id = created.Id,
            ArtifactId = created.ArtifactId,
            Status = created.Status,
            DateSubmitted = created.DateSubmitted,
            SubmittedBy = $"{created.SubmittedBy.FirstName} {created.SubmittedBy.LastName}",
            VerifiedBy = null,
            Notes = new List<CatalogNoteResponseDto>()
        };
    }

    public async Task<bool> UpdateCatalogRecordAsync(int id, UpdateCatalogRecordRequestDto request, CancellationToken ct)
    {
        // Step A — Retrieve existing record
        var record = await dbContext.CatalogRecords.FindAsync([id], ct);

        if (record is null)
            return false;

        // Step B — Update fields
        record.Status = request.Status;

        // Step C — Update or clear VerifiedById
        if (string.IsNullOrWhiteSpace(request.VerifiedById))
        {
            record.VerifiedById = null;
        }
        else
        {
            // Optionally validate verified user ID exists
            var userExists = await dbContext.Users.AnyAsync(u => u.Id == request.VerifiedById, ct);
            if (!userExists)
                return false;

            record.VerifiedById = request.VerifiedById;
        }

        // Step D — Save changes
        await dbContext.SaveChangesAsync(ct);

        return true;
    }

    public async Task<bool> DeleteCatalogRecordAsync(int id, CancellationToken ct)
    {
        // Step A — Lookup record
        var record = await dbContext.CatalogRecords.FindAsync([id], ct);

        if (record is null)
            return false;

        // Step B — Delete and save
        dbContext.CatalogRecords.Remove(record);
        await dbContext.SaveChangesAsync(ct);

        return true;
    }
}
