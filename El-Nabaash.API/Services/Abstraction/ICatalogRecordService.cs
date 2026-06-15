using El_Nabaash.API.DTOs.Catalog.Request;
using El_Nabaash.API.DTOs.CatalogRecord.Response;

namespace El_Nabaash.API.Services.Abstraction;

public interface ICatalogRecordService
{
    Task<List<CatalogRecordResponseDto>?> GetCatalogRecordsByArtifactAsync(int artifactId, CancellationToken ct);
    Task<CatalogRecordResponseDto?> GetCatalogRecordByIdAsync(int id, CancellationToken ct);

    Task<CatalogRecordResponseDto?> CreateCatalogRecordAsync(
        CreateCatalogRecordRequestDto request,
        string userId,
        CancellationToken ct);
}