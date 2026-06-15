using El_Nabaash.API.DTOs.Artifacts.Response;
using Microsoft.AspNetCore.Http.HttpResults;

namespace El_Nabaash.API.Endpoints.Artifacts;

public static class ArtifactEndpoints
{
    // groups 

    public static IEndpointRouteBuilder MapArtifactEndpoints(this IEndpointRouteBuilder route)
    {
        var publicGroup = route.MapGroup("api/public/artifacts")
            .WithTags("Artifact - Public")
            .AddEndpointFilter<ExceptionHandlingFilter>()
            .AllowAnonymous();
        
        var privateGroup = route.MapGroup("api/private/artifacts")
            .WithTags("Artifact - Private")
            .AddEndpointFilter<ExceptionHandlingFilter>()
            .RequireAuthorization();
        
        publicGroup.MapGet("", GetPublicArtifact)
            .WithName("GetPublicArtifacts")
            .WithSummary("Get all public artifacts")
            .WithDescription("Retrieves a list of all artifacts that are marked as public, including their details and primary image URLs.")
            .Produces(StatusCodes.Status404NotFound);

        
        publicGroup.MapGet("/{id:int}",GetPublicArtifactsBySite)
            .WithName(nameof(GetPublicArtifactsBySite))
            .WithSummary("Get public artifacts by site ID")
            .WithDescription("Retrieves a list of all public artifacts associated with a specific site ID")
            .Produces<List<PublicArtifactResponseDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            privateGroup.MapGet("", GetPrivateArtifact)
            .WithName("GetPrivateArtifacts")
            .WithSummary("Get all private artifacts")
            .WithDescription("Retrieves a list of all artifacts, including their details and primary image URLs. This endpoint requires authentication and is intended for internal use.")
            .Produces(StatusCodes.Status404NotFound);
        
        return route;
    }

    // handlers
    private static async Task<Results<Ok<List<PublicArtifactResponseDto>>,NotFound>> GetPublicArtifact(
        IArtifactService artifactService,
        CancellationToken ct
        )
    {
        var artifacts = await artifactService.GetPublicArtifactAsync(ct);
        if(artifacts.Count == 0)
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.Ok(artifacts);
    }
    
    private static async Task<Results<Ok<List<PrivateArtifactResponseDto>>,NotFound>> GetPrivateArtifact(
        IArtifactService artifactService,
        CancellationToken ct
    )
    {
        var artifacts = await artifactService.GetPrivateArtifactAsync(ct);
        if(artifacts.Count == 0)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(artifacts);
    }

    private static async Task<Results<Ok<List<PublicArtifactResponseDto>>, NotFound>> GetPublicArtifactsBySite
        (
            int siteId,
            IArtifactService service,
            CancellationToken ct
        )
    {
        var artifacts = await service.GetPublicArtifactsBySiteAsync(ct);
        if (artifacts.Count == 0)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(artifacts);
    }
}