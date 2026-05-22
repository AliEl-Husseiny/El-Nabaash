using Microsoft.AspNetCore.Http.HttpResults;

namespace El_Nabaash.API.Endpoints.Artifacts;

public class ArtifactMediaFilesEndpoint
{
    // Endpoint group 
    // assign endpoints 
    // create handlers methods 
    private static async Task<Results<FileContentHttpResult, NotFound<string>>> GetArtifactImage(
        int id,
        AppDbContext dbContext,
        HttpResponse response,
        CancellationToken ct
    )
    {
        // find the media file in the media files table 
        var image = await dbContext.ArtifactMediaFiles.AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id, ct);
        
        if(image == null || image.Data.Length == 0) 
        {
            return TypedResults.NotFound($"No media file found with id {id}");
        }
        
        // optional 
        // cache the media file for 3 days 
        response.Headers.CacheControl = "public, max-age=259200"; // 3 days in seconds
        
        return TypedResults.File(image.Data, image.ContentType, image.FileName);
        
        
    }
}