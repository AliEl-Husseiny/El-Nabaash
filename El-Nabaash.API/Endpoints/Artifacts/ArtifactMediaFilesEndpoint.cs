using Microsoft.AspNetCore.Http.HttpResults;

namespace El_Nabaash.API.Endpoints.Artifacts;

public static class ArtifactMediaFilesEndpoint
{
    // Endpoint group 
    public static IEndpointRouteBuilder MapArtifactMediaFilesEndpoints(this IEndpointRouteBuilder rout)
    {
        var publicGroup = rout.MapGroup("/api/public/artifacts/images")
            .WithTags("Artifact Media Files")
            .AddEndpointFilter<ExceptionHandlingFilter>();

        publicGroup.MapGet("/{id:int}", GetArtifactImage)
            .WithName("GetArtifactImage")
            .Produces<FileContentHttpResult>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Get an artifact image by its ID")
            .WithDescription("Retrieves the binary image data for an artifact");

        var privateGroup = rout.MapGroup("/api/private/artifacts/images")
            .WithTags("Artifact Media Files - Private")
            .RequireAuthorization()
            .AddEndpointFilter<ExceptionHandlingFilter>();


        privateGroup.MapPost("/create", CreateArtifactMediaFile)
            .DisableAntiforgery() // cross site scripting 
            .WithName(nameof(CreateArtifactMediaFile))
            .Accepts<IFormFile>("multipart/form-data")
            .Produces(StatusCodes.Status201Created)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .Produces<string>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new artifact media file")
            .WithDescription(
                "Uploads a new media file for a specific artifact. The file must be an image and cannot be empty. If 'isPrimary' is set to true, this media file will be marked as the primary image for the artifact, and any existing primary media file will be unmarked.");


        return rout;
    }

    // assign endpoints 
    // create handlers methods 

    private static async Task<Results<Created, NotFound<string>, BadRequest<string>>> CreateArtifactMediaFile(
        int artifactId,
        IFormFile file,
        bool isPrimary,
        IArtifactMediaFileService service,
        CancellationToken ct
    )
    {
        if (file is null || file.Length == 0)
            return TypedResults.BadRequest("File is required and cannot be empty");

        var media = await service.CreateArtifactMediaFileAsync(artifactId, file, isPrimary, ct);
        if (media is null) return TypedResults.NotFound($"Artifact with id {artifactId} not found");
        return TypedResults.Created($"/api/public/artifacts/images/{media.Id}");
    }

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

        if (image == null || image.Data.Length == 0)
        {
            return TypedResults.NotFound($"No media file found with id {id}");
        }

        // optional 
        // cache the media file for 3 days 
        response.Headers.CacheControl = "public, max-age=259200"; // 3 days in seconds

        return TypedResults.File(image.Data, image.ContentType, image.FileName);
    }
}