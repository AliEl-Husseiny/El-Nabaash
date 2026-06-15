using El_Nabaash.API.DTOs.CatalogRecord.Response;
using Microsoft.AspNetCore.Http.HttpResults;

namespace El_Nabaash.API.Endpoints.Artifacts;

public class CatalogRecordsEndpoint
{
    
    
    
    
    
    
    
    // Handlers
    private static async Task<Results<Ok<List<CatalogRecordResponseDto>>, NotFound>>
        GetCatalogRecordsByArtifact(
            int artifactId,
            ICatalogRecordService service,
            CancellationToken ct)
    {
        var records = await service.GetCatalogRecordsByArtifactAsync(artifactId, ct);

        if (records is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(records);
    }
}